using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Views
{
    internal class FlowLayoutPanelBreadCrumbs : FlowLayoutPanel
    {
        private Stack<SMRDataDirectory> treeNodesBreadCrumbs;

        private readonly SMRStorage smrStorage;

        public FlowLayoutPanelBreadCrumbs(SMRStorage smrStorage) : base()
        {
            this.smrStorage = smrStorage;

            BorderStyle = BorderStyle.FixedSingle;
            Dock = DockStyle.Fill;
            Font = DataDefault.textFont10;
            ForeColor = Color.White; 
            WrapContents = false;
            Padding = new Padding(5, 8, 5, 8);

            treeNodesBreadCrumbs = new Stack<SMRDataDirectory>();

            this.smrStorage.OnReadSMRDataDirectoryHandler += OnReadSMRDataDirectory;
            this.smrStorage.FormClosedHandler += OnFormClosed;
        }

        private void OnFormClosed()
        {
            foreach (Control control in Controls)
            {
                if (control is LinkLabel linkLabel)
                    linkLabel.LinkClicked -= (sender, e) => OnLinkCkicked(linkLabel.Tag as SMRDataDirectory);
            }
            smrStorage.OnReadSMRDataDirectoryHandler -= OnReadSMRDataDirectory;
            smrStorage.FormClosedHandler -= OnFormClosed;
        }

        private void OnReadSMRDataDirectory(ISMRData smrData)
        {
            if (smrData?.Node?.TreeView == null)
                return;

            Visible = false;
            TreeNode treeNodeParent = smrData.Node;
            do
            {
                if (treeNodeParent.Tag is SMRDataDirectory smrDataDirectory)
                {
                    treeNodeParent = treeNodeParent.Parent;
                    treeNodesBreadCrumbs.Push(smrDataDirectory);
                    treeNodesBreadCrumbs.Push(smrDataDirectory);
                }

                else
                {
                    return;
                }

            } while (treeNodeParent != null);
            
            if (treeNodesBreadCrumbs.Count == 0)
                return;

            if (Controls.Count > treeNodesBreadCrumbs.Count)
            {
                for (int i = Controls.Count - 1; i >= treeNodesBreadCrumbs.Count - 1; i--)
                {
                    if (Controls[i] is LinkLabel linkLabel)
                            linkLabel.LinkClicked -= (sender, e) => OnLinkCkicked(linkLabel.Tag as SMRDataDirectory);

                    Controls.RemoveAt(i);
                }
            }

            else if (Controls.Count <= treeNodesBreadCrumbs.Count)
            {
                for (int i = Controls.Count; i < treeNodesBreadCrumbs.Count - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        LinkLabel link = new LinkLabel() { ActiveLinkColor = DataDefault.textWhite, Font = DataDefault.textFont12, AutoSize = true };
                        link.LinkClicked += (sender, e) => OnLinkCkicked(link.Tag as SMRDataDirectory);
                        Controls.Add(link);
                    }
                    else
                    {
                        Controls.Add(new MLabel(DataDefault.SEPARATOR) { AutoSize = true });
                    }
                }

            }

            for (int i = 0; i < Controls.Count; i += 2)
            {
                if (Controls[i] is LinkLabel linkLabel)
                {
                    SMRDataDirectory smrDataDirectory = treeNodesBreadCrumbs.Pop();
                    linkLabel.LinkColor = DataDefault.blue;
                    linkLabel.Text = smrDataDirectory.Name;
                    linkLabel.Tag = smrDataDirectory;

                    if (treeNodesBreadCrumbs.Count != 0)
                        treeNodesBreadCrumbs.Pop();
                }
            }

            if (Controls[0] is LinkLabel linkLabelFirst)
                linkLabelFirst.LinkColor = DataDefault.blue;

            if (Controls[Controls.Count - 1] is LinkLabel linkLabelLast)
                linkLabelLast.LinkColor = DataDefault.textWhite;
            
            treeNodesBreadCrumbs.Clear();
            Visible = true;
        }

        private void OnLinkCkicked(SMRDataDirectory smrDataDirectory) => smrStorage.SMRActions.CheckUpdateActivePath(smrDataDirectory);
    }
}
