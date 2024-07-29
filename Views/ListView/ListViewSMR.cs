using System.IO;
using SNAMP.Utils;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class ListViewSMR : ListView
    {
        private ContextMenuStripListViewSMR contextMenuStripListViewSMR;

        private readonly SMRDataSMRFile smrDataSMRFile;
        private readonly SMRStorage smrStorage;

        public ListViewSMR(SMRStorage smrStorage, SMRDataSMRFile smrDataSMRFile) : base()
        {
            this.smrDataSMRFile = smrDataSMRFile;
            this.smrStorage = smrStorage;

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.FixedSingle;
            ForeColor = Color.White;
            BackColor = DataDefault.bg;
            LargeImageList = IconList.IconsList64;
            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
            Height = 280;

            InitializeElements();
            InitializeData();
        }

        public void InitializeData()
        {
            Items.Clear();
            smrDataSMRFile.DataMeta = null;
            smrDataSMRFile.DataMeta.links.ForEach(linkToFile =>
            {
                ISMRData smrDataFind = smrStorage.SMRActions.FindSMRData(linkToFile.LinkTreeNode);
                if (smrDataFind is SMRDataFile smrDataFileFind)
                {
                    ListViewItem listViewItemSMR = smrDataFileFind.ListViewItemsSMR.Find(ListViewItemSMR => ListViewItemSMR.ListView == this);

                    if (listViewItemSMR == null)
                    {
                        listViewItemSMR = new ListViewItem() { Text = smrDataFileFind.Node.Text, ImageIndex = smrDataFileFind.IndexImg, Tag = smrDataFileFind };
                        smrDataFileFind.ListViewItemsSMR.Add(listViewItemSMR);
                    }

                    listViewItemSMR.Remove();
                    Items.Add(listViewItemSMR);
                }
            });
        }

        private void InitializeElements()
        {
            MouseDoubleClick += (sender, e) => OnMouseDoubleClick();
            ItemSelectionChanged += (sender, e) => OnItemSelectionChanged();

            contextMenuStripListViewSMR = new ContextMenuStripListViewSMR();

            contextMenuStripListViewSMR.ToolStripMenuItemOpen.Click += (sender, e) => OnMouseDoubleClick();
            contextMenuStripListViewSMR.ToolStripMenuItemWatch.Click += (sender, e) => OnWatch();
            contextMenuStripListViewSMR.ToolStripMenuItemOpenFile.Click += (sender, e) => OnOpenFile();
            contextMenuStripListViewSMR.ToolStripMenuItemOpenExplorer.Click += (sender, e) => OnOpenExplorer();
        }

        public void FormClosed()
        {
            MouseDoubleClick -= (sender, e) => OnMouseDoubleClick();
            ItemSelectionChanged -= (sender, e) => OnItemSelectionChanged();

            contextMenuStripListViewSMR.ToolStripMenuItemOpen.Click -= (sender, e) => OnMouseDoubleClick();
            contextMenuStripListViewSMR.ToolStripMenuItemWatch.Click -= (sender, e) => OnWatch();
            contextMenuStripListViewSMR.ToolStripMenuItemOpenFile.Click -= (sender, e) => OnOpenFile();
            contextMenuStripListViewSMR.ToolStripMenuItemOpenExplorer.Click -= (sender, e) => OnOpenExplorer();
        }

        private void OnWatch()
        {
            if (SelectedItems.Count > 0 && SelectedItems[0]?.Tag is ISMRData smrData && !(smrData is SMRDataDirectory))
            {
                smrStorage.SMRActions.CheckUpdateActivePath(smrData);
            }
        }

        private void OnMouseDoubleClick()
        {
            foreach (ListViewItem selectedItem in SelectedItems)
            {
                if (selectedItem?.Tag is ISMRData smrData && !(smrData is SMRDataDirectory))
                    smrStorage.SMRActions.SMRToolOpen(smrData);
            }   
        }

        private void OnOpenFile()
        {
            try
            {
                foreach (ListViewItem selectedItem in SelectedItems)
                {
                    if (selectedItem?.Tag is SMRDataFile smrDataFile && !(smrDataFile is SMRDataSMRFile) && File.Exists(smrDataFile.FullPathToSMRData))
                        Process.Start(smrDataFile.FullPathToSMRData);
                }
            }
            catch
            {
                DialogWindow.MessageError("Не предвиденная ошибка открытия файла");
            }
        }

        private void OnOpenExplorer()
        {
            string path = string.Empty;
            ProcessStartInfo info;
            if (SelectedItems.Count > 0 && SelectedItems[0].Tag is ISMRData smrData)
            {

                if (!File.Exists(smrData.FullPathToSMRData) && !Directory.Exists(smrData.FullPathToSMRData))
                    return;

                info = new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = $"/e, /select, \"{smrData.FullPathToSMRData}\"",
                };
            }

            else
            {
                path = smrDataSMRFile.PathToSMRDataParent.FullName;

                if (!Directory.Exists(path))
                    return;

                info = new ProcessStartInfo(path);
            }

            try
            {
                Process.Start(info);
            }
            catch
            {
                DialogWindow.MessageError("Не предвиденная ошибка открытия проводника по пути: " + path);
            }

        }

        private void OnItemSelectionChanged()
        {
            if (SelectedItems.Count > 0)
            {
                ContextMenuStrip = contextMenuStripListViewSMR.ContextMenuStrip;
                contextMenuStripListViewSMR.ToolStripMenuItemOpen.Enabled = SelectedItems[0].Tag is SMRDataDirectory || SelectedItems[0].Tag is SMRDataSMRFile || (SelectedItems[0].Tag is SMRDataFile smrDataFile2 && BuilderDocument.CheckOnOpenFileState(smrDataFile2.FileState));
                contextMenuStripListViewSMR.ToolStripMenuItemOpenFile.Enabled = SelectedItems[0].Tag is SMRDataFile smrDataFile && !(smrDataFile is SMRDataSMRFile);
            }

            else
            {
                ContextMenuStrip = null;
            }
        }
    }
}
