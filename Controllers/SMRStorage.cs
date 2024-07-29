using System;
using System.IO;
using SNAMP.Models;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP
{
    public class SMRStorage
    {
        public event Action<List<ISMRData>, DataDefault.SMRTool> ActivedSMRDataToolHandler;
        public event Action<SMRDataDirectory> OnReadSMRDataDirectoryHandler;
        public event Action<SMRDataDirectory> OnRefreshSMRDataDirectoryHandler;
        public event Action<List<ISMRData>> OnCreateSMRDataHandler;
        public event Action<List<ISMRData>> OnUpdateSMRDataHandler;
        public event Action<List<ISMRData>> OnDeleteSMRDataHandler;
        public event Action<List<ISMRData>> OnSelectSMRDataHandler;

        public event Action<SMRDataFile> OpenSMRDataFileHandler;
        public event Action<SMRDataSMRFile> OpenSMRDataSMRFileHandler;
        public event Action<List<ISMRData>> OnSelectedFilesHandler;

        public event Action<object, FormClosingEventArgs> FormClosingHandler;
        public event Action FormLoadHandler;
        public event Action FormClosedHandler;

        public SMRProject SMRProject { get; private set; }
        public SMRActions SMRActions { get; private set; }
        public SMRDataDirectoryRoot SMRDataRoot { get; private set; }
        public SMRDataDirectory SMRDataDirectoryCurrent { get; private set; }
        public DataInterface DataInterface { get; private set; }

        public bool IsLoad { get; private set; }

        public SMRStorage(SMRProject smrProject, DataInterface dataInterface)
        {
            SMRProject = smrProject;
            DataInterface = dataInterface;
            smrProject.isActive = true;
            TreeNode treeNodeRoot = new TreeNode(smrProject.Name);
            SMRDataRoot = new SMRDataDirectoryRoot(treeNodeRoot, new DirectoryInfo(smrProject.Path));
            treeNodeRoot.Tag = SMRDataRoot;
            SMRActions = new SMRActions(this);
        }

        public void FormClosed()
        {
            FormClosedHandler?.Invoke();
            SMRProject.UpdateTime();
            SMRProject.isActive = false;
        }

        public void FormLoad()
        {
            IsLoad = true;
            FormLoadHandler?.Invoke();
            SMRDataDirectory smrDataDirectoryStart = SMRDataRoot;

            if (!string.IsNullOrWhiteSpace(DataInterface.pathActived)) 
            {
                ISMRData smrDataFind = SMRActions.FindSMRData(DataInterface.pathActived);

                if (smrDataFind is SMRDataDirectory smrDataDirectory)
                    smrDataDirectoryStart = smrDataDirectory;
            }

            ReadSMRDataDirectory(smrDataDirectoryStart);
        }

        public void ReadSMRDataDirectory(SMRDataDirectory smrDataDirectory)
        {
            SMRDataDirectoryCurrent = smrDataDirectory?.Node?.TreeView != null ? smrDataDirectory : SMRDataRoot;

            OnReadSMRDataDirectoryHandler?.Invoke(SMRDataDirectoryCurrent);
        }

        public void RefreshSMRData(SMRDataDirectory smrDataDirectory) => OnRefreshSMRDataDirectoryHandler?.Invoke(smrDataDirectory);

        public void CreateSMRData(List<ISMRData> smrDatas) => OnCreateSMRDataHandler?.Invoke(smrDatas);

        public void DeleteSMRData(List<ISMRData> smrDatas) => OnDeleteSMRDataHandler?.Invoke(smrDatas);

        public void UpdateSMRData(List<ISMRData> smrDatas) => OnUpdateSMRDataHandler?.Invoke(smrDatas);

        public void FormClosing(object sender, FormClosingEventArgs e) => FormClosingHandler?.Invoke(sender, e);

        public void UpdateSelectedFiles(List<ISMRData> selectedItems) => OnSelectedFilesHandler?.Invoke(selectedItems);

        public void OpenFile(SMRDataFile smrDataFile) => OpenSMRDataFileHandler?.Invoke(smrDataFile);

        public void OpenSMRFile(SMRDataSMRFile smrDataSMRFile) => OpenSMRDataSMRFileHandler?.Invoke(smrDataSMRFile);

        public void ActiveSMRDataTool(List<ISMRData> smrDatas, DataDefault.SMRTool smrTool) => ActivedSMRDataToolHandler?.Invoke(smrDatas, smrTool);
    }
}
