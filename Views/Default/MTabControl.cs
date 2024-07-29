using SNAMP.Utils;
using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class MTabControl : TabControl
    {
        private readonly SMRStorage smrStorage;

        private readonly SplitContainer viewManagerContainer;

        public MTabControl(SMRStorage smrStorage, SplitContainer viewManagerContainer) : base()
        { 
            this.smrStorage = smrStorage;
            this.viewManagerContainer = viewManagerContainer;
            AllowDrop = true;
            Dock = DockStyle.Fill;
            Size = new Size(1316, 1);
            DrawMode = TabDrawMode.OwnerDrawFixed;
            SizeMode = TabSizeMode.Normal;
            Padding = new Point(0, 0);
            SelectedIndex = 0;
            Visible = false;
            InitializeElements();
        }

        public bool CloseTabPage(TabPage tabPage)
        {
            if (tabPage is TabPageSMR tabPageSMR)
            {
                if (!IsCanTabClosed(tabPageSMR))
                    return false;
                tabPageSMR.CloseSMRDataSMRFile(); 
            }

            else if (tabPage is TabPageFile tabPageFile)
            {
                tabPageFile.CloseSMRDataFile();
            }

            else
            {
                DialogWindow.MessageWarning("Непредвиденная ошибка с окном: " + tabPage == null ? "" : tabPage?.Text);
            }

            if (TabPages.Contains(tabPage))
                TabPages.Remove(tabPage);

            if (TabPages.Count == 0)
            {
                if (viewManagerContainer.SplitterDistance == 500 || viewManagerContainer.SplitterDistance < 100)
                    viewManagerContainer.SplitterDistance = viewManagerContainer.Panel1MinSize;
                Visible = false;
            }
            return true;
        }

        private int GetHoverTabIndex()
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
                    return i;
            }

            return -1;
        }

        private bool IsCanTabClosed(TabPageSMR tabPageSMR)
        {
            if (!tabPageSMR.SMRDataSMRFile.DataSMR.IsSaveData())
            {
                DialogResult dialogResult = DialogWindow.MessageWarning($"Сохранить внесенные изменения в {tabPageSMR.Text} перед закрытием?");

                if (dialogResult == DialogResult.Yes)
                    tabPageSMR.SaveSMRDataSMRFile();

                else if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
                    return false;
            }

            return true;
        }

        private void ShowTabPage(TabPage tabPage)
        {
            if (TabPages.Count == 0 && viewManagerContainer.SplitterDistance < 100)
            {
                viewManagerContainer.SplitterDistance = 500;
            }
                

            if (!TabPages.Contains(tabPage))
                TabPages.Add(tabPage);

            SelectTab(tabPage);

            Visible = true;
        }

        private void InitializeElements()
        {
            DrawItem += OnDrawItem;
            MouseDown += OnMouseDownTabControl;
            MouseUp += OnMouseUpTabControl;
            MouseMove += OnMouseMoveTabControl;
            DragOver += OnDragOverTabControl;

            smrStorage.OpenSMRDataSMRFileHandler += OnOpenSMRDataSMRFile;
            smrStorage.OpenSMRDataFileHandler += OnOpenSMRDataFile;
            smrStorage.FormLoadHandler += OnFormLoad;
            smrStorage.FormClosingHandler += OnFormClosing;
            smrStorage.FormClosedHandler += OnFormClosed;
        }

        private void SwapTabPages(TabPage tabPageSource, TabPage tabPageTarget)
        {
            int index_src = TabPages.IndexOf(tabPageSource);
            int index_dst = TabPages.IndexOf(tabPageTarget);
            TabPages[index_dst] = tabPageSource;
            TabPages[index_src] = tabPageTarget;
            Refresh();
        }

        private void OnFormLoad()
        {
            smrStorage.DataInterface.tabPagesSMROpened.ForEach(pathTabPage =>
            {
                ISMRData smrDataFind = smrStorage.SMRActions.FindSMRData(pathTabPage);
                if (smrDataFind is SMRDataSMRFile smrDataSMRFile)
                    OnOpenSMRDataSMRFile(smrDataSMRFile);
                else if (smrDataFind is SMRDataFile smrDataFile)
                    OnOpenSMRDataFile(smrDataFile);
            });
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage tabPage in TabPages)
            {
                if (tabPage is TabPageSMR tabPageSMR && !IsCanTabClosed(tabPageSMR))
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void OnFormClosed()
        {
            DrawItem -= OnDrawItem;
            MouseDown -= OnMouseDownTabControl;
            MouseMove -= OnMouseMoveTabControl;
            MouseUp -= OnMouseUpTabControl;
            DragOver -= OnDragOverTabControl;

            smrStorage.OpenSMRDataSMRFileHandler -= OnOpenSMRDataSMRFile;
            smrStorage.OpenSMRDataFileHandler -= OnOpenSMRDataFile;
            smrStorage.FormLoadHandler -= OnFormLoad;
            smrStorage.FormClosingHandler -= OnFormClosing;
            smrStorage.FormClosedHandler -= OnFormClosed;
        }

        private void OnOpenSMRDataFile(SMRDataFile smrDataFile)
        {
            if (smrDataFile is SMRDataSMRFile smrDataSMRFile)
            {
                OnOpenSMRDataSMRFile(smrDataSMRFile);
                return;
            }

            TabPageFile tabPageFile = smrDataFile.TabPageFile ?? new TabPageFile(smrDataFile, this);

            if (smrDataFile.TabPageFile == null)
                return;

            ShowTabPage(tabPageFile);
        }

        private void OnOpenSMRDataSMRFile(SMRDataSMRFile smrDataSMRFile)
        {
            TabPageSMR tabPageSMR = smrDataSMRFile.TabPageSMR ?? new TabPageSMR(smrStorage, smrDataSMRFile, this);

            if (smrDataSMRFile.TabPageSMR == null)
                return;

            ShowTabPage(tabPageSMR);
        }

        private void OnMouseDownTabControl(object sender, MouseEventArgs e)
        {
            int hoverIndex = GetHoverTabIndex();
            if (hoverIndex >= 0)
            {
                Tag = TabPages[hoverIndex];

                Rectangle r = GetTabRect(hoverIndex);
                Rectangle closeButton = new Rectangle(r.Right - 20, r.Top + 4, 12, 15);

                if (closeButton.Contains(e.Location))
                    CloseTabPage(SelectedTab);
            }
        }

        private void OnMouseMoveTabControl(object sender, MouseEventArgs e)
        {
            if ((e.Button != MouseButtons.Left) || !(Tag is TabPage tabPage) || tabPage.Parent == null) 
                return;

            DoDragDrop(tabPage, DragDropEffects.All);
        }

        private void OnDragOverTabControl(object sender, DragEventArgs e)
        {
            if (!(Tag is TabPage dragTab)) 
                return;

            int dragTab_index = TabPages.IndexOf(dragTab);

            int hoverTab_index = GetHoverTabIndex();

            if (hoverTab_index < 0) { 
                e.Effect = DragDropEffects.None; 
                return; 
            }

            TabPage hoverTab = TabPages[hoverTab_index];
            e.Effect = DragDropEffects.Move;

            if (dragTab == hoverTab) 
                return;
            Rectangle dragTabRect = GetTabRect(dragTab_index);
            Rectangle hoverTabRect = GetTabRect(hoverTab_index);

            if (dragTabRect.Width < hoverTabRect.Width)
            {
                Point tcLocation = PointToScreen(Location);

                if (dragTab_index < hoverTab_index)
                {
                    if ((e.X - tcLocation.X) > ((hoverTabRect.X + hoverTabRect.Width) - dragTabRect.Width))
                        SwapTabPages(dragTab, hoverTab);
                }
                else if (dragTab_index > hoverTab_index)
                {
                    if ((e.X - tcLocation.X) < (hoverTabRect.X + dragTabRect.Width))
                        SwapTabPages(dragTab, hoverTab);
                }
            }
            else SwapTabPages(dragTab, hoverTab);

            SelectedIndex = TabPages.IndexOf(dragTab);
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.DrawString("x", DataDefault.textFont10, Brushes.Black, e.Bounds.Right - 20, e.Bounds.Top + 4);
            e.Graphics.DrawString(TabPages[e.Index].Text, DataDefault.textFont10, Brushes.Black, e.Bounds.Left + 5, e.Bounds.Top + 5);
            e.DrawFocusRectangle();
        }

        private void OnMouseUpTabControl(object sender, MouseEventArgs e) => Tag = null;
    }
}
