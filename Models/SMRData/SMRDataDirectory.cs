using System;
using System.IO;
using System.Windows.Forms;

namespace SNAMP
{
    public class SMRDataDirectory : ISMRData, IComparable<SMRDataDirectory>
    {
        public DataDefault.SMRState SMRState { get; set; }

        public DirectoryInfo PathToSMRDataDirectory { get; set; }
        public TreeNode Node { get; set; }
        public ListViewItem ListViewItem { get; set; }

        public string FullPathToSMRData { get; set; }
        public string Name { get; set; }

        public int IndexImg { get; set; }

        public SMRDataDirectory(TreeNode node, DirectoryInfo pathToSMRDataDirectory)
        {
            Node = node;
            PathToSMRDataDirectory = pathToSMRDataDirectory;
            FullPathToSMRData = pathToSMRDataDirectory.FullName;
            SMRState = DataDefault.SMRState.Directory;
            Name = pathToSMRDataDirectory.Name;
            IndexImg = 0;
            Node.Tag = this;
            Node.ImageIndex = IndexImg;
        }

        public int CompareTo(SMRDataDirectory other) => other.FullPathToSMRData == FullPathToSMRData ? 0 : 1;

        public virtual void UpdateDirectory()
        {
            PathToSMRDataDirectory = new DirectoryInfo(PathToSMRDataDirectory.FullName.TrimEnd(Path.DirectorySeparatorChar));
            Name = PathToSMRDataDirectory.Name;
            FullPathToSMRData = PathToSMRDataDirectory.FullName;
            Node.Text = PathToSMRDataDirectory.Name;
            Node.Name = PathToSMRDataDirectory.Name;
        }
    }
}
