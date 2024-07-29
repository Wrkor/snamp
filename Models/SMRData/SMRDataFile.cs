using System.IO;
using SNAMP.Utils;
using System.Windows.Forms;
using SNAMP.Views;
using System.Collections.Generic;

namespace SNAMP
{
    public class SMRDataFile : ISMRData
    {
        public DataDefault.SMRState SMRState { get; set; }
        public DataDefault.FileState FileState { get; set; }

        public List<ListViewItem> ListViewItemsSMR { get; set; }

        public DirectoryInfo PathToSMRDataParent { get; set; }
        public FileInfo PathToSMRDataFile { get; set; }
        public TreeNode Node { get; set; }
        public ListViewItem ListViewItem { get; set; }
        public TabPageFile TabPageFile { get; set; }
        public DataMeta DataMeta
        {
            get
            {
                if (_dataMeta == null)
                {
                    _dataMeta = DataSerialize.ReadData<DataMeta>(FullPathToSMRDataMeta, false);

                    if (_dataMeta == null)
                    {
                        _dataMeta = new DataMeta();
                        _dataMeta.SetDataByDefalut();
                        DataSerialize.WriteData(_dataMeta, FullPathToSMRDataMeta);
                    }
                }

                return _dataMeta;
            }
            set => _dataMeta = DataSerialize.ReadData<DataMeta>(FullPathToSMRDataMeta);
        }

        public string FullPathToSMRData { get; set; }
        public string FullPathToSMRDataMeta { get; set; }
        public string Name { get; set; }

        public int IndexImg { get; set; }


        private DataMeta _dataMeta;

        public SMRDataFile(TreeNode node, FileInfo pathToSMRDataFile)
        {
            Node = node;
            PathToSMRDataFile = pathToSMRDataFile;
            FullPathToSMRData = pathToSMRDataFile.FullName;
            Name = pathToSMRDataFile.Name;
            PathToSMRDataParent = pathToSMRDataFile.Directory;
            FullPathToSMRDataMeta = BuilderSMR.GetPathToSMRMetaBySMRDataFile(pathToSMRDataFile);
            SMRState = DataDefault.SMRState.File;
            FileState = DataDefault.GetFileState(pathToSMRDataFile.Extension);
            IndexImg = DataDefault.GetIndexImageByFileState(FileState);
            Node.Tag = this;
            Node.ImageIndex = IndexImg;
            Node.SelectedImageIndex = IndexImg;
            ListViewItemsSMR = new List<ListViewItem>();
        }

        public virtual void UpdateFile()
        {
            FullPathToSMRData = PathToSMRDataFile.FullName;
            PathToSMRDataParent = PathToSMRDataFile.Directory;
            Name = PathToSMRDataFile.Name;
            FullPathToSMRDataMeta = BuilderSMR.GetPathToSMRMetaBySMRDataFile(PathToSMRDataFile);
            FileState = DataDefault.GetFileState(PathToSMRDataFile.Extension);
            IndexImg = DataDefault.GetIndexImageByFileState(FileState);
            Node.Text = PathToSMRDataFile.Name;
            Node.Name = PathToSMRDataFile.Name;
            Node.ImageIndex = IndexImg;
            Node.SelectedImageIndex = IndexImg;
        }
    }
}
