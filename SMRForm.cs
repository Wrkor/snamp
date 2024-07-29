using SNAMP.Utils;
using System.Linq;
using SNAMP.Views;
using SNAMP.Models;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP
{
    public partial class SMRForm : Form
    {
        public DataInterface DataInterface
        {
            get
            {
                if (_dataInterface == null)
                    _dataInterface = DataSerialize.ReadData<DataInterface>(smrProject.GetFullPath());

                return _dataInterface;
            }
            private set => _dataInterface = value;
        }

        public readonly SMRStorage smrStorage;

        private DataInterface _dataInterface;

        private readonly Storage storage;
        private readonly SMRProject smrProject;
        private readonly TreeViewNavigation treeViewNavigation;
        private readonly ListViewNavigation listViewNavigation;
        private readonly FlowLayoutPanelBreadCrumbs flowLayoutPanelBreadCrumbs;
        private readonly ToolStripNavigation toolStripNavigation;
        private readonly MTabControl mTabControl;
        private readonly PanelProperties panelProperty;

        public SMRForm(Storage storage, SMRProject smrProject)
        {
            InitializeComponent();
            Text = $"SNAMP - {smrProject.Name}";

            this.storage = storage;
            this.smrProject = smrProject;

            smrStorage = new SMRStorage(smrProject, DataInterface);
            treeViewNavigation = new TreeViewNavigation(smrStorage);
            listViewNavigation = new ListViewNavigation(smrStorage, managerToolsContainer);
            flowLayoutPanelBreadCrumbs = new FlowLayoutPanelBreadCrumbs(smrStorage);
            toolStripNavigation = new ToolStripNavigation(smrStorage);
            panelProperty = new PanelProperties(smrStorage);
            mTabControl = new MTabControl(smrStorage, viewManagerContainer);

            Load += (sender, e) => OnSMRFormLoad();
            FormClosed += (sender, e) => OnSMRFormFormClosed();
            FormClosing += OnSMRFormFormClosing;

            toolStripMenuItemSMRHub.Click += (sender, e) => OnToolStripMenuItemSMRHubClick();
            toolStripMenuItemAbout.Click += (sender, e) => OnToolStripMenuItemAboutClick();
            toolStripMenuItemHelp.Click += (sender, e) => OnToolStripMenuItemHelpClick();

            splitContainerTreeView.Panel2.Controls.Add(treeViewNavigation);
            managerToolsContainer.Panel2.Controls.Add(listViewNavigation);
            managerLinksContainer.Panel1.Controls.Add(flowLayoutPanelBreadCrumbs);
            managerToolsContainer.Panel1.Controls.Add(toolStripNavigation);
            splitContainerProperties.Panel2.Controls.Add(panelProperty);
            viewManagerContainer.Panel1.Controls.Add(mTabControl);
        }

        private void OnSMRFormLoad()
        {
            arhViewContainer.SplitterDistance = DataInterface.arhViewPanelsSD;
            viewPropertyContainer.SplitterDistance = DataInterface.viewPropertPanelsSD;
            viewManagerContainer.SplitterDistance = DataInterface.viewManagerPanelsSD;
            WindowState = DataInterface.windowState;
            Size = DataInterface.windowSize;
            Location = DataInterface.windowLocation;
            Opacity = 1;

            smrStorage.FormLoad();
        }

        private void OnSMRFormFormClosed()
        {
            DataInterface.arhViewPanelsSD = arhViewContainer.SplitterDistance;
            DataInterface.viewPropertPanelsSD = viewPropertyContainer.SplitterDistance;
            DataInterface.viewManagerPanelsSD = viewManagerContainer.SplitterDistance;
            DataInterface.windowLocation = Location;
            DataInterface.windowSize = Size;
            DataInterface.windowState = WindowState;
            DataInterface.treeNodesExpanded = BuilderTreeView.GetListNodesExpanded(new List<string>(), smrStorage.SMRDataRoot.Node).ToList();
            DataInterface.tabPagesSMROpened = new List<string>(mTabControl.TabPages.Count);
            DataInterface.pathActived = smrStorage?.SMRDataDirectoryCurrent?.Node?.FullPath ?? smrProject.Name;

            for (int i = 0; i < mTabControl.TabPages.Count; i++)
            {
                if (mTabControl.TabPages[i] is TabPageSMR tabPageSMR && tabPageSMR.SMRDataSMRFile.Node.TreeView != null)
                    DataInterface.tabPagesSMROpened.Add(tabPageSMR.SMRDataSMRFile.Node.FullPath);

                else if (mTabControl.TabPages[i] is TabPageFile tabPageFile && tabPageFile.SMRDataFile.Node.TreeView != null)
                    DataInterface.tabPagesSMROpened.Add(tabPageFile.SMRDataFile.Node.FullPath);
            }

            DataSerialize.WriteData(DataInterface, smrStorage.SMRProject.GetFullPath());

            smrStorage.FormClosed();
            storage.CloseSMRProject(this);

            Load -= (sender, e) => OnSMRFormLoad();
            FormClosed -= (sender, e) => OnSMRFormFormClosed();
            FormClosing -= OnSMRFormFormClosing;

            toolStripMenuItemSMRHub.Click -= (sender, e) => OnToolStripMenuItemSMRHubClick();
            toolStripMenuItemAbout.Click -= (sender, e) => OnToolStripMenuItemAboutClick();
            toolStripMenuItemHelp.Click -= (sender, e) => OnToolStripMenuItemHelpClick();
        }

        private void OnToolStripMenuItemSMRHubClick()
        {
            storage.smrHubForm.TopMost = true;
            storage.smrHubForm.Show();
            storage.smrHubForm.TopMost = false;
        }

        private void OnSMRFormFormClosing(object sender, FormClosingEventArgs e) => smrStorage.FormClosing(sender, e);

        private void OnToolStripMenuItemHelpClick() => Process.Start(new ProcessStartInfo(DataDefault.URL_HELP));

        private void OnToolStripMenuItemAboutClick() => new AboutForm().Show();
    }
}
