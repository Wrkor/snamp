using System;
using System.IO;
using SNAMP.Utils;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Views
{
    internal class ListViewNavigation : ListView
    {
        private readonly SMRStorage smrStorage;
        private readonly ContextMenuStripListView contextMenuStripListView;
        private readonly ContextMenuStripListViewOnFile contextMenuStripListViewOnFile;
        private readonly SplitContainer managerToolsContainer;

        private bool isSelect;

        private List<ListViewItem> smrDataBufferList;
        private SMRDataDirectory currentSMRDataDirectory;

        public ListViewNavigation(SMRStorage smrStorage, SplitContainer managerToolsContainer) : base()
        {
            this.smrStorage = smrStorage;
            this.managerToolsContainer = managerToolsContainer;

            AllowDrop = true;
            BackColor = DataDefault.bg;
            BorderStyle = BorderStyle.FixedSingle;
            Cursor = Cursors.Arrow;
            Dock = DockStyle.Fill;
            Font = DataDefault.textFont12;
            ForeColor = Color.White;
            HideSelection = false;
            LabelEdit = true;
            LargeImageList = IconList.IconsList64;
            UseCompatibleStateImageBehavior = false;
            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
            Scrollable = true;

            contextMenuStripListView = new ContextMenuStripListView();
            contextMenuStripListViewOnFile = new ContextMenuStripListViewOnFile();
            ContextMenuStrip = contextMenuStripListView.ContextMenuStrip;
            InitializeElements();

            isSelect = true;
        }

        private void OnSplitterMoved()
        {
            Scrollable = true;
            Scrollable = false;
        }

        private void InitializeElements()
        {
            smrDataBufferList = new List<ListViewItem>();

            MouseDoubleClick += (sender, e) => OnMouseDoubleClick();
            ItemSelectionChanged += OnItemSelectionChanged;
            ItemDrag += new ItemDragEventHandler(OnItemDragNavListView);
            DragLeave += new EventHandler(OnDragLeaveNavListView);
            DragEnter += new DragEventHandler(OnDragEnterNavListView);
            DragDrop += new DragEventHandler(OnDragDropNavListView);
            AfterLabelEdit += OnAfterListViewItemEdit;
            KeyDown += OnKeyDownNavListView;

            managerToolsContainer.SplitterMoved += (sender, e) => OnSplitterMoved();

            contextMenuStripListView.ToolStripMenuItemImportDirectory.Click += (sender, e) => OnToolStripMenuItemImportDirectory();
            contextMenuStripListView.ToolStripMenuItemImportFile.Click += (sender, e) => OnToolStripMenuItemImportFile();
            contextMenuStripListView.ToolStripMenuItemImportSMRFile.Click += (sender, e) => OnToolStripMenuItemImportSMRFile();
            contextMenuStripListView.ToolStripMenuItemOpenExplorer.Click += (sender, e) => OnOpenExplorer();
            contextMenuStripListView.ToolStripMenuItemPaste.Click += (sender, e) => OnClickPasteBtn();
            contextMenuStripListView.ToolStripMenuItemUpdate.Click += (sender, e) => OnClickUpdateBtn();

            contextMenuStripListViewOnFile.ToolStripMenuItemCut.Click += (sender, e) => OnClickCutBtn();
            contextMenuStripListViewOnFile.ToolStripMenuItemCopy.Click += (sender, e) => OnClickCopyBtn();
            contextMenuStripListViewOnFile.ToolStripMenuItemDelete.Click += (sender, e) => OnClickDeleteBtn();
            contextMenuStripListViewOnFile.ToolStripMenuItemOpen.Click += (sender, e) => OnMouseDoubleClick();
            contextMenuStripListViewOnFile.ToolStripMenuItemOpenExplorer.Click += (sender, e) => OnOpenExplorer();
            contextMenuStripListViewOnFile.ToolStripMenuItemOpenFile.Click += (sender, e) => OnOpenFile();
            contextMenuStripListViewOnFile.ToolStripMenuItemRename.Click += (sender, e) => OnClickRenameBtn();

            smrStorage.OnReadSMRDataDirectoryHandler += OnReadSMRDataDirectory;
            smrStorage.OnRefreshSMRDataDirectoryHandler += OnRefreshSMRDataDirectory;
            smrStorage.OnUpdateSMRDataHandler += OnUpdateSMRData;
            smrStorage.OnCreateSMRDataHandler += OnCreateSMRData;
            smrStorage.ActivedSMRDataToolHandler += OnActivedSMRDataTool;
            smrStorage.OnSelectedFilesHandler += OnSelectedFiles;
            smrStorage.FormClosedHandler += OnFormCLosed;
        }

        private void OnRefreshSMRDataDirectory(SMRDataDirectory directory)
        {
            ListViewItem[] listViewItemSort = Items.Cast<ListViewItem>().OrderBy(listViewItem => listViewItem.ImageIndex).ToArray();
            Items.Clear();
            Items.AddRange(listViewItemSort);
        }

        private void OnCreateSMRData(List<ISMRData> smrDatas)
        {
            smrDatas.ForEach(smrData =>
            {
                smrData.ListViewItem = smrData.ListViewItem ?? new ListViewItem() { Text = smrData.Node.Text, ImageIndex = smrData.IndexImg, Tag = smrData };
                Items.Add(smrData.ListViewItem);
            });
        }

        private void OnUpdateSMRData(List<ISMRData> smrDatas)
        {
            smrDatas.ForEach(smrData =>
            {
                if (smrData.ListViewItem == null)
                    smrData.ListViewItem = new ListViewItem() { Text = smrData.Node.Text, ImageIndex = smrData.IndexImg, Tag = smrData };
                
                else
                {
                    smrData.ListViewItem.Text = smrData.Node.Text;
                    smrData.ListViewItem.ImageIndex = smrData.IndexImg;
                }

                if (smrData.Node?.Parent != currentSMRDataDirectory.Node)
                {
                    if (smrData.ListViewItem.ListView != null)
                        smrData.ListViewItem.Remove();
                }
                else
                {
                    if (smrData.ListViewItem.ListView == null)
                        Items.Add(smrData.ListViewItem);
                }
            });
        }

        private void ClearSMRDataBufferList()
        {
            smrDataBufferList.ForEach(listViewItem => listViewItem.BackColor = DataDefault.Transparent);
            smrDataBufferList.Clear();
        }

        private void UpdateBtns()
        {
            contextMenuStripListView.ToolStripMenuItemPaste.Enabled = smrStorage.SMRActions.SMRDataBuffer.Count > 0;

            if (SelectedItems.Count > 0)
            {
                contextMenuStripListViewOnFile.ToolStripMenuItemOpen.Enabled = SelectedItems[0].Tag is SMRDataDirectory || SelectedItems[0].Tag is SMRDataSMRFile || (SelectedItems[0].Tag is SMRDataFile smrDataFile2 && BuilderDocument.CheckOnOpenFileState(smrDataFile2.FileState));
                contextMenuStripListViewOnFile.ToolStripMenuItemOpenFile.Enabled = SelectedItems[0].Tag is SMRDataFile smrDataFile && !(smrDataFile is SMRDataSMRFile);
            }

            else
            {
                contextMenuStripListViewOnFile.ToolStripMenuItemOpen.Enabled = false;
                contextMenuStripListViewOnFile.ToolStripMenuItemOpenFile.Enabled = false;
            }
        }

        private void OnFormCLosed()
        {
            MouseDoubleClick -= (sender, e) => OnMouseDoubleClick();
            ItemSelectionChanged -= OnItemSelectionChanged;
            ItemDrag -= new ItemDragEventHandler(OnItemDragNavListView);
            DragLeave -= new EventHandler(OnDragLeaveNavListView);
            DragEnter -= new DragEventHandler(OnDragEnterNavListView);
            DragDrop -= new DragEventHandler(OnDragDropNavListView);
            AfterLabelEdit -= OnAfterListViewItemEdit;
            KeyDown -= OnKeyDownNavListView;

            managerToolsContainer.SplitterMoved -= (sender, e) => OnSplitterMoved();

            contextMenuStripListView.ToolStripMenuItemImportDirectory.Click -= (sender, e) => OnToolStripMenuItemImportDirectory();
            contextMenuStripListView.ToolStripMenuItemImportFile.Click -= (sender, e) => OnToolStripMenuItemImportFile();
            contextMenuStripListView.ToolStripMenuItemImportSMRFile.Click -= (sender, e) => OnToolStripMenuItemImportSMRFile();
            contextMenuStripListView.ToolStripMenuItemOpenExplorer.Click -= (sender, e) => OnOpenExplorer();
            contextMenuStripListView.ToolStripMenuItemPaste.Click -= (sender, e) => OnClickPasteBtn();
            contextMenuStripListView.ToolStripMenuItemUpdate.Click -= (sender, e) => OnClickUpdateBtn();

            contextMenuStripListViewOnFile.ToolStripMenuItemCut.Click -= (sender, e) => OnClickCutBtn();
            contextMenuStripListViewOnFile.ToolStripMenuItemCopy.Click -= (sender, e) => OnClickCopyBtn();
            contextMenuStripListViewOnFile.ToolStripMenuItemDelete.Click -= (sender, e) => OnClickDeleteBtn();
            contextMenuStripListViewOnFile.ToolStripMenuItemOpen.Click -= (sender, e) => OnMouseDoubleClick();
            contextMenuStripListViewOnFile.ToolStripMenuItemOpenExplorer.Click -= (sender, e) => OnOpenExplorer();
            contextMenuStripListViewOnFile.ToolStripMenuItemOpenFile.Click -= (sender, e) => OnOpenFile();
            contextMenuStripListViewOnFile.ToolStripMenuItemRename.Click -= (sender, e) => OnClickRenameBtn();

            smrStorage.OnReadSMRDataDirectoryHandler -= OnReadSMRDataDirectory;
            smrStorage.OnRefreshSMRDataDirectoryHandler -= OnRefreshSMRDataDirectory;
            smrStorage.OnUpdateSMRDataHandler -= OnUpdateSMRData;
            smrStorage.OnCreateSMRDataHandler -= OnCreateSMRData;
            smrStorage.FormClosedHandler -= OnFormCLosed;
            smrStorage.ActivedSMRDataToolHandler -= OnActivedSMRDataTool;
            smrStorage.OnSelectedFilesHandler -= OnSelectedFiles;
        }

        private void OnSelectedFiles(List<ISMRData> selectedItems)
        {
            if (isSelect)
            {
                SelectedItems.Clear();
                selectedItems.ForEach(selectedItem =>
                {
                    if (selectedItem.ListViewItem != null)
                        selectedItem.ListViewItem.Selected = true;
                });
            }
            ContextMenuStrip = SelectedItems.Count > 0 ? contextMenuStripListViewOnFile.ContextMenuStrip : contextMenuStripListView.ContextMenuStrip;
            UpdateBtns();
        }

        private void OnMouseDoubleClick()
        {
            bool isOpenDirectory = false;
            foreach (ListViewItem selectedItem in SelectedItems)
            {
                if (selectedItem.Tag != null && selectedItem.Tag is ISMRData smrData)
                {
                    if (smrData is SMRDataDirectory smrDataDirectory)
                    {
                        if (!isOpenDirectory)
                        {
                            smrStorage.SMRActions.CheckInitializeNodesInTreeNode(smrDataDirectory);
                            smrStorage.SMRActions.SMRToolOpen(smrData);
                            isOpenDirectory = true;
                        }
                    }
                        
                    else
                    {
                        smrStorage.SMRActions.SMRToolOpen(smrData);
                    }
                }
            }   
        }

        private void OnReadSMRDataDirectory(SMRDataDirectory smrData)
        {
            currentSMRDataDirectory = smrData;
            SelectedItems.Clear();
            Items.Clear();

            foreach (TreeNode node in smrData.Node.Nodes)
            {
                if (!(node?.Tag is ISMRData data))
                    continue;

                data.ListViewItem = data.ListViewItem ?? new ListViewItem() { Text = data.Node.Text, ImageIndex = data.IndexImg, Tag = data };
                Items.Add(data.ListViewItem);
            }

            UpdateBtns();
        }

        private void OnActivedSMRDataTool(IEnumerable<ISMRData> smrDatas, DataDefault.SMRTool smrTool)
        {
            if (smrTool == DataDefault.SMRTool.Copy || smrTool == DataDefault.SMRTool.Cut)
            {
                ClearSMRDataBufferList();
                smrDatas.Where(smrData => smrData.ListViewItem != null)
                    .ToList()
                    .ForEach(smrData =>
                    {
                        smrData.ListViewItem.BackColor = smrTool == DataDefault.SMRTool.Copy ? DataDefault.smrDataCopy : DataDefault.smrDataCut;
                        smrDataBufferList.Add(smrData.ListViewItem);
                    });
            }

            else if (smrTool == DataDefault.SMRTool.Paste || smrTool == DataDefault.SMRTool.None)
                ClearSMRDataBufferList();

            UpdateBtns();
        }

        private void OnAfterListViewItemEdit(object sender, LabelEditEventArgs e)
        {
            if (!(Items[e.Item]?.Tag is ISMRData smrData) || SelectedItems[0].Text.Length == 0 || SelectedItems[0].Text.Length > 245)
            {
                e.CancelEdit = true;
                return;
            }

            if (!smrStorage.SMRActions.SMRToolRename(smrData, e.Label))
                e.CancelEdit = true;
        }

        private void OnDragDropNavListView(object sender, DragEventArgs e)
        {
            Point targetPoint = PointToClient(new Point(e.X, e.Y));
            int targetIndex = InsertionMark.NearestIndex(targetPoint);

            if (targetIndex == -1 || Items.Count < targetIndex + 1 || !(Items[targetIndex]?.Tag is SMRDataDirectory smrDataDirecory) || smrDataDirecory is SMRDataDirectoryRoot || SelectedItems.Count == 0)
                return;

            List<ISMRData> smrDatas = new List<ISMRData>();

            foreach (ListViewItem selectedListViewItem in SelectedItems)
                if (selectedListViewItem?.Tag is ISMRData smrData)
                    smrDatas.Add(smrData);

            smrStorage.SMRActions.SMRToolCut(smrDatas);
            smrStorage.SMRActions.SMRToolPaste(smrDataDirecory);
        }

        private void OnKeyDownNavListView(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                smrStorage.SMRActions.SMRToolDelete();

            else if (e.Control && e.KeyCode == Keys.C)
                smrStorage.SMRActions.SMRToolCopy();

            else if (e.Control && e.KeyCode == Keys.V)
                smrStorage.SMRActions.SMRToolPaste(currentSMRDataDirectory);

            else if (e.Control && e.KeyCode == Keys.X)
                smrStorage.SMRActions.SMRToolCut();

            else if (e.KeyCode == Keys.F2)
                OnClickRenameBtn();

            else if (e.KeyCode == Keys.Enter)
                OnMouseDoubleClick();
        }

        private void OnOpenFile()
        {
            if (SelectedItems.Count == 0)
                return;

            try
            {
                foreach (ListViewItem selectedItem in SelectedItems)
                {
                    if (selectedItem.Tag is ISMRData smrData && !(smrData is SMRDataSMRFile) && File.Exists(smrData.FullPathToSMRData))
                        Process.Start(smrData.FullPathToSMRData);
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
            if (SelectedItems.Count > 0 && SelectedItems[0]?.Tag is ISMRData smrData)
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
                path = currentSMRDataDirectory.FullPathToSMRData;

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

        private void OnClickRenameBtn()
        {
            if (SelectedItems.Count > 0)
                SelectedItems[0].BeginEdit();
        }

        private void OnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            isSelect = false;
            smrStorage.UpdateSelectedFiles(SelectedItems.Cast<ListViewItem>().Where(selectedItem => selectedItem.Tag is ISMRData).Select(selectedItem => selectedItem.Tag as ISMRData).ToList());
            isSelect = true;
        }

        private void OnClickDeleteBtn() => smrStorage.SMRActions.SMRToolDelete();

        private void OnClickPasteBtn() => smrStorage.SMRActions.SMRToolPaste(currentSMRDataDirectory);

        private void OnClickUpdateBtn() => smrStorage.SMRActions.SMRToolRefresh(currentSMRDataDirectory);

        private void OnClickCutBtn() => smrStorage.SMRActions.SMRToolCut();

        private void OnClickCopyBtn() => smrStorage.SMRActions.SMRToolCopy();

        private void OnToolStripMenuItemImportSMRFile() => smrStorage.SMRActions.CreateSMRFile(currentSMRDataDirectory);

        private void OnToolStripMenuItemImportFile() => smrStorage.SMRActions.ImportFiles(currentSMRDataDirectory);

        private void OnToolStripMenuItemImportDirectory() => smrStorage.SMRActions.CreateDirectory(currentSMRDataDirectory);

        private void OnItemDragNavListView(object sender, ItemDragEventArgs e) => DoDragDrop(e.Item, DragDropEffects.Move);

        private void OnDragEnterNavListView(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;

        private void OnDragLeaveNavListView(object sender, EventArgs e) => InsertionMark.Index = -1;
    }
}
