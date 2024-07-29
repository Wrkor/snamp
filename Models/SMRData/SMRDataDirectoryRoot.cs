using System.IO;
using SNAMP.Views;
using System.Windows.Forms;

namespace SNAMP
{
    public class SMRDataDirectoryRoot : SMRDataDirectory
    {
        public TreeViewNavigation TreeViewNavigation { get; set; }

        public string TreeViewPath { get; set; }

        public SMRDataDirectoryRoot(TreeNode node, DirectoryInfo pathToSMRDataDirectory) : base(node, pathToSMRDataDirectory)
        {
            SMRState = DataDefault.SMRState.Root;
            TreeViewPath = pathToSMRDataDirectory.Parent.FullName;
            IndexImg = 1;
        }

        public override void UpdateDirectory() { }
    }
}
