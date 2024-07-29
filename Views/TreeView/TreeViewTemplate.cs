using SNAMP.Utils;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SNAMP.Views
{
    public class TreeViewTemplate : TreeView
    {
        public bool IsSave { get; set; }

        private const string NAME_TEMPLATE = "Шаблон проекта";

        private TreeNode treeNodeRoot;

        public TreeViewTemplate(ImageList iconList) :base()
        {
            AllowDrop = true;
            BackColor = DataDefault.bg;
            Dock = DockStyle.Fill;
            Font = DataDefault.textFont12;
            ForeColor = Color.White;
            ImageIndex = 0;
            ImageList = iconList;
            LineColor = Color.White;
            SelectedImageIndex = 0;
            IsSave = true;

            InitializeElements();
        }

        public List<string> GetData()
        {
            List<string> dirs = new List<string>();

            foreach (TreeNode child in treeNodeRoot.Nodes)
                GetAllDirectoriesByNode(child, ref dirs);

            return dirs;
        }

        public void RenameNode()
        {
            if (SelectedNode != null && SelectedNode != treeNodeRoot)
                SelectedNode.BeginEdit();
        }

        public void DeleteNode()
        {
            if (SelectedNode != null && SelectedNode != treeNodeRoot)
            {
                Nodes.Remove(SelectedNode);
                IsSave = false;
            }
        }

        public void CreateNode()
        {
            if (SelectedNode == null)
                return;

            string name = BuilderTreeView.GetCheckTreeNodeName(DataDefault.NAME_NEW_DIRECTORY, SelectedNode);

            TreeNode newTreeNode = new TreeNode() { Text = name, Name = name };
            SelectedNode.Nodes.Add(newTreeNode);
            SelectedNode.Expand();
            IsSave = false;
        }

        public void InitializeData(List<string> directories)
        {
            Nodes.Clear();
            treeNodeRoot = new TreeNode(NAME_TEMPLATE) { Text = NAME_TEMPLATE };
            Nodes.Add(treeNodeRoot);
            LabelEdit = true;
            PathSeparator = DataDefault.SEPARATOR;

            TreeNode lastNode = null;
            string subPathAgg;
            foreach (string path in directories)
            {
                subPathAgg = string.Empty;
                foreach (string subPath in path.Split(DataDefault.SEPARATOR[0]))
                {
                    subPathAgg += subPath + DataDefault.SEPARATOR;
                    TreeNode[] nodes = treeNodeRoot.Nodes.Find(subPathAgg, true);
                    if (nodes.Length == 0)
                        lastNode = lastNode == null ? treeNodeRoot.Nodes.Add(subPathAgg, subPath) : lastNode.Nodes.Add(subPathAgg, subPath);
                    else
                        lastNode = nodes[0];
                    lastNode.Expand();
                }
                lastNode = null;
            }

            treeNodeRoot.Expand();
            SelectedNode = treeNodeRoot;
        }

        public void FormClose()
        {
            BeforeLabelEdit -= OnBeforeLabelEditTreeViewTemplate;
            AfterLabelEdit -= OnAfterLabelEditTreeViewTemplate;
            DragDrop -= OnTreeViewTemplateDragDrop;
            DragEnter -= OnTreeViewTemplateDragEnter;
            ItemDrag -= OnTreeViewTemplateItemDrag;
            KeyDown -= OnKeyDownTreeViewTemplate;
        }

        private List<string> GetAllDirectoriesByNode(TreeNode _self, ref List<string> dirs)
        {
            if (_self.FullPath.Length >= NAME_TEMPLATE.Length + 1)
                dirs.Add(_self.FullPath.Substring(NAME_TEMPLATE.Length + 1));

            foreach (TreeNode child in _self.Nodes)
                GetAllDirectoriesByNode(child, ref dirs);

            return dirs;
        }

        private void InitializeElements()
        {
            BeforeLabelEdit += OnBeforeLabelEditTreeViewTemplate;
            AfterLabelEdit += OnAfterLabelEditTreeViewTemplate;
            DragDrop += OnTreeViewTemplateDragDrop;
            DragEnter += OnTreeViewTemplateDragEnter;
            ItemDrag += OnTreeViewTemplateItemDrag;
            KeyDown += OnKeyDownTreeViewTemplate;
        }

        private void OnKeyDownTreeViewTemplate(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                RenameNode();

            else if (e.KeyCode == Keys.OemMinus)
                DeleteNode();

            else if (e.KeyCode == Keys.Oemplus)
                CreateNode();
        }

        private void OnTreeViewTemplateDragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = GetNodeAt(targetPoint);

            if (targetNode == null)
                return;

            TreeNode draggedNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (draggedNode == null || draggedNode == treeNodeRoot)
                return;

            if (draggedNode.Equals(targetNode))
                return;

            if (draggedNode.Nodes.Contains(targetNode))
            {
                DialogWindow.MessageError("Невозможно переместить папку внутрь этой же папки");
                return;
            }

            draggedNode.Remove();
            draggedNode.Text = BuilderTreeView.GetCheckTreeNodeName(draggedNode.Text, targetNode);
            targetNode.Nodes.Add(draggedNode);
            IsSave = false;
            targetNode.Expand();
        }

        private void OnAfterLabelEditTreeViewTemplate(object sender, NodeLabelEditEventArgs e)
        {
            if (e?.Label == null || e?.Label == e.Node.Text)
            {
                e.CancelEdit = true;
                return;
            }
            else if (e?.Node?.Parent == null)
            {
                DialogWindow.MessageError("Непредвиденная ошибка");
                e.CancelEdit = true;
                return;
            }

            else if (string.IsNullOrEmpty(e.Label) || e.Label.Length > 255)
            {
                DialogWindow.MessageError("Имя папки должно иметь длину от 1 до 255 сиволов");
                e.CancelEdit = true;
                return;
            }

            else if (BuilderTreeView.IsNodeNameInNodes(e.Label, e.Node.Parent.Nodes))
            {
                DialogWindow.MessageError("Папка с таким именем по этому пути уже существует: " + e.Label);
                e.CancelEdit = true;
                return;
            }

            else if (Regex.Match(e.Label, DataDefault.REG_NAME_DIRECTORY).Success)
            {
                DialogWindow.MessageError("Нельзя использовать знаки: " + DataDefault.REG_NAME_DIRECTORY.Replace("[", "").Replace("]", ""));
                e.CancelEdit = true;
                return;
            }

            IsSave = false;
        }

        private void OnBeforeLabelEditTreeViewTemplate(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == treeNodeRoot)
                e.CancelEdit = true;
        }

        private void OnTreeViewTemplateItemDrag(object sender, ItemDragEventArgs e) => DoDragDrop(e.Item, DragDropEffects.Move);

        private void OnTreeViewTemplateDragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;
    }
}
