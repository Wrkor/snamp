using SNAMP.Utils;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class TreeViewNavigation : TreeView
    {
        private readonly SMRStorage smrStorage;
        
        public TreeViewNavigation(SMRStorage smrStorage) : base()
        {
            this.smrStorage = smrStorage;
            
            HideSelection = false;
            BackColor = DataDefault.bg;
            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill;
            Font = DataDefault.textFont12;
            ForeColor = Color.White;
            ImageList = IconList.IconsList16;
            LineColor = Color.White;
            SelectedImageIndex = 0;

            Nodes.Add(smrStorage.SMRDataRoot.Node);
            smrStorage.SMRDataRoot.TreeViewNavigation = this;
            
            InitializeElements();
        }

        private void InitializeElements()
        {
            smrStorage.OnReadSMRDataDirectoryHandler += OnReadSMRDataDirectory;
            smrStorage.OnRefreshSMRDataDirectoryHandler += OnRefreshSMRDataDirectory;
            smrStorage.FormLoadHandler += OnFormLoad;
            smrStorage.FormClosedHandler += OnFormClose;
            BeforeExpand += OnBeforeExpand;
            BeforeSelect += OnBeforeSelect;
            NodeMouseClick += OnNodeMouseClick;
        }

        private void OnFormClose()
        {
            smrStorage.OnReadSMRDataDirectoryHandler -= OnReadSMRDataDirectory;
            smrStorage.OnRefreshSMRDataDirectoryHandler -= OnRefreshSMRDataDirectory;
            smrStorage.FormLoadHandler -= OnFormLoad;
            smrStorage.FormClosedHandler -= OnFormClose;
            BeforeExpand -= OnBeforeExpand;
            BeforeSelect -= OnBeforeSelect;
            NodeMouseClick -= OnNodeMouseClick;
        }

        private void OnFormLoad()
        {
            BuilderTreeView.CreateOpenTreeNodes(smrStorage.DataInterface.treeNodesExpanded, smrStorage.SMRDataRoot.Node, smrStorage.SMRDataRoot.PathToSMRDataDirectory);
            smrStorage.SMRDataRoot.Node.Expand();
        }

        private void OnRefreshSMRDataDirectory(SMRDataDirectory smrDataTarget)
        {
            TreeNode[] listViewItemSort = smrDataTarget.Node.Nodes.Cast<TreeNode>().OrderBy(listViewItem => listViewItem.ImageIndex).ToArray();
            smrDataTarget.Node.Nodes.Clear();
            smrDataTarget.Node.Nodes.AddRange(listViewItemSort);
        }

        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.IsSelected && e?.Node?.Tag is ISMRData smrData)
                smrStorage.SMRActions.CheckUpdateActivePath(smrData);
        }

        private void OnBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e?.Node?.Tag is ISMRData smrData && e?.Action != TreeViewAction.Unknown)
                smrStorage.SMRActions.CheckUpdateActivePath(smrData);
        }

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e?.Node?.Tag is ISMRData smrData && e?.Action != TreeViewAction.Unknown)
                smrStorage.SMRActions.CheckInitializeNodesInTreeNode(smrData);
        }

        private void OnReadSMRDataDirectory(ISMRData smrData)
        {
            if (smrData.Node.IsVisible && (SelectedNode?.Parent != smrData.Node || smrData is SMRDataDirectory))
                SelectedNode = smrData.Node;
        }
    }
}
