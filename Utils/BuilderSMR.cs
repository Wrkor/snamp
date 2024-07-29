using System.IO;
using System.Linq;
using SNAMP.Models;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Utils
{
    public static class BuilderSMR
    {
        public static bool CheckCloseSMRData(List<ISMRData> smrDatas)
        {
            foreach (ISMRData smrData in smrDatas)
            {
                if (smrData is SMRDataDirectory smrDataDirectory)
                {
                    if (!CheckCloseSMRData(smrDataDirectory.Node.Nodes.Cast<TreeNode>().Where(treeNode => treeNode.Tag is ISMRData).Select(treeNode => treeNode.Tag as ISMRData).ToList()))
                        return false;
                }
                else if (smrData is SMRDataSMRFile smrDataSMRFile)
                {
                    if (smrDataSMRFile.TabPageSMR != null && !smrDataSMRFile.TabPageSMR.MTabControl.CloseTabPage(smrDataSMRFile.TabPageSMR))
                        return false;
                }
                else if (smrData is SMRDataFile smrDataFile)
                {
                    if (smrDataFile.TabPageFile != null && !smrDataFile.TabPageFile.MTabControl.CloseTabPage(smrDataFile.TabPageFile))
                        return false;
                }
            }

            return true;
        }

        public static void BuildSMRProject(SMRProject project, IEnumerable<string> directories)
        {
            try
            {
                File.Create(Path.Combine(project.Path, project.Name + DataDefault.SMR_PROJECT_EXT)).Close();

                foreach (string directory in directories)
                {
                    Directory.CreateDirectory(Path.Combine(project.Path, directory));
                }
            }

            catch
            {
                DialogWindow.MessageError("Ошибка создания проекта: " + project.GetFullPath());
            }
        }

        public static void DeleteSMRDataSMRMeta(SMRDataFile smrDataFile)
        {
            if (CheckOnSMRMetaFile(new FileInfo(smrDataFile.FullPathToSMRDataMeta)) && File.Exists(smrDataFile.FullPathToSMRDataMeta))
                File.Delete(smrDataFile.FullPathToSMRDataMeta);
        }

        public static string GetPathToSMRMetaBySMRDataFile(FileInfo smrDataFile) => smrDataFile.FullName + DataDefault.SMR_META_EXT;
        
        public static string GetPathToSMRDataFileBySMRMeta(FileInfo smrMeta) => smrMeta.Name.EndsWith(DataDefault.SMR_META_EXT) ? smrMeta.FullName.Substring(0, smrMeta.FullName.Length - DataDefault.SMR_META_EXT.Length) : smrMeta.FullName;
        
        public static bool CheckOnSMRFile(FileInfo file) => file.Exists && !string.IsNullOrWhiteSpace(file.Extension) && file.Extension == DataDefault.SMR_EXT;

        public static bool CheckOnSMRMetaFile(FileInfo file) => file.Exists && !string.IsNullOrWhiteSpace(file.Extension) && file.Extension == DataDefault.SMR_META_EXT;

        public static bool CheckOnSMRProjectFile(FileInfo file) => file.Exists && !string.IsNullOrWhiteSpace(file.Extension) && file.Extension == DataDefault.SMR_PROJECT_EXT;
    }
}
