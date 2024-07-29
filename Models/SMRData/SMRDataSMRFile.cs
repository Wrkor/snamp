using System.IO;
using SNAMP.Utils;
using SNAMP.Views;
using System.Windows.Forms;

namespace SNAMP
{
    public class SMRDataSMRFile : SMRDataFile, ISMRData
    {
        public TabPageSMR TabPageSMR { get; set; }
        public DataSMR DataSMR
        {
            get
            {
                if (_dataSMR == null)
                {
                    _dataSMR = DataSerialize.ReadData<DataSMR>(FullPathToSMRData);

                    if (_dataSMR == null)
                    {
                        _dataSMR = new DataSMR();
                        _dataSMR.SetDataByDefalut();
                        DataSerialize.WriteData(_dataSMR, FullPathToSMRData);
                    }
                }

                return _dataSMR;
            }
            set => _dataSMR = null;
        }

        private DataSMR _dataSMR;

        public SMRDataSMRFile(TreeNode node, FileInfo pathToSMRDataFile) : base(node, pathToSMRDataFile)
        {
            FileState = DataDefault.FileState.None;
            SMRState = DataDefault.SMRState.SMRFile;
            IndexImg = 1;
            node.Tag = this;
            node.ImageIndex = IndexImg;
            node.SelectedImageIndex = IndexImg;
        }

        public override void UpdateFile()
        {
            FullPathToSMRData = PathToSMRDataFile.FullName;
            PathToSMRDataParent = PathToSMRDataFile.Directory;
            Name = PathToSMRDataFile.Name;
            FullPathToSMRDataMeta = BuilderSMR.GetPathToSMRMetaBySMRDataFile(PathToSMRDataFile);
            Node.Text = PathToSMRDataFile.Name;
            Node.Name = PathToSMRDataFile.Name;
            Node.ImageIndex = IndexImg;
            Node.SelectedImageIndex = IndexImg;
        }
    }
}
