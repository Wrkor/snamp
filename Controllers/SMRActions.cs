using System.IO;
using System.Linq;
using SNAMP.Utils;
using SNAMP.Models;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP
{
    public class SMRActions
    {
        public List<ISMRData> SMRDataBuffer { get; private set; }

        private List<ISMRData> selectedListViewItems;

        private DataDefault.SMRTool currentSMRTool;

        private readonly SMRStorage smrStorage;

        public SMRActions(SMRStorage smrStorage)
        {
            this.smrStorage = smrStorage;

            SMRDataBuffer = new List<ISMRData>();

            smrStorage.ActivedSMRDataToolHandler += OnActivedSMRDataTool;
            smrStorage.OnSelectedFilesHandler += OnSelectedFiles;
            smrStorage.FormClosedHandler += OnFormClosed;
        }

        public ISMRData FindSMRData(string pathToFile)
        {
            if (string.IsNullOrWhiteSpace(pathToFile))
                return null;

            List<string> nodePathParts = new List<string>(pathToFile.Split(DataDefault.SEPARATOR[0]));
            int index = nodePathParts.FindIndex(nodePathPart => nodePathPart == smrStorage.SMRProject.Name);

            if (index == -1)
            {
                DialogWindow.MessageError($"Путь {pathToFile} не ведет в проект {smrStorage.SMRProject.Name}!");
                return null;
            }

            TreeNode nodeForFind = smrStorage.SMRDataRoot.Node;
            for (int i = index + 1; i < nodePathParts.Count; i++)
            {
                if (!nodeForFind.Checked)
                {
                    if (!(nodeForFind.Tag is ISMRData smrDataForChecked))
                        return null;

                    CheckInitializeNodesInTreeNode(smrDataForChecked);
                }

                TreeNode[] treeNodesFind = nodeForFind.Nodes.Find(nodePathParts[i], false);

                if (treeNodesFind.Length == 0)
                {
                    DialogWindow.MessageError($"Путь {pathToFile} не был найден");
                    return null;
                }
                else if (treeNodesFind.Length == 1)
                {
                    nodeForFind = treeNodesFind[0];
                }
                else
                {
                    DialogWindow.MessageError($"Путь {pathToFile} не был найден");
                    return null;
                }
            }
            if (!(nodeForFind.Tag is ISMRData smrData))
                return null;

            if (!nodeForFind.Checked)
                CheckInitializeNodesInTreeNode(smrData);

            return smrData;
        }

        public bool SMRToolRename(ISMRData smrData, string newName)
        {
            if (smrData is SMRDataDirectoryRoot || string.IsNullOrEmpty(newName) || smrData.Name == newName)
                return false;

            else if (smrData is SMRDataDirectory smrDataDirectory)
            {
                List<ISMRData> smrDatasUpdate = new List<ISMRData>();
                BuilderTreeView.GetListSMRDatasInside(smrDatasUpdate, smrDataDirectory.Node);

                if (!BuilderSMR.CheckCloseSMRData(smrDatasUpdate))
                    return false;

                if (!BuilderFileOrDirectory.Rename(smrDataDirectory.PathToSMRDataDirectory, newName))
                    return false;

                UpdateSMRData(smrDataDirectory, smrDataDirectory.Node.FullPath, smrDatasUpdate, smrDatasUpdate);
                smrDatasUpdate.Add(smrDataDirectory);
                smrStorage.UpdateSMRData(smrDatasUpdate);

                return true;
            }

            else if (smrData is SMRDataFile smrDataFile)
            {
                if (!BuilderSMR.CheckCloseSMRData(new List<ISMRData>() { smrData }))
                    return false;

                if (smrDataFile is SMRDataSMRFile && !newName.EndsWith(DataDefault.SMR_EXT))
                {
                    DialogWindow.MessageError("Нельзя переименовать расширение (.smr) SMR объекта");
                    return false;
                }

                if (!BuilderFileOrDirectory.Rename(smrDataFile.PathToSMRDataFile, newName))
                    return false;

                List<ISMRData> smrDatasUpdate = new List<ISMRData>();

                UpdateSMRData(smrDataFile, smrDataFile.Node.FullPath, smrDatasUpdate);
                smrDatasUpdate.Add(smrDataFile);
                smrStorage.UpdateSMRData(smrDatasUpdate);

                return true;
            }

            return false;
        }

        public void CreateDirectory(SMRDataDirectory smrDataTarget)
        {
            DirectoryInfo directoryInfo = BuilderFileOrDirectory.CreateDirectory(smrDataTarget.PathToSMRDataDirectory, DataDefault.NAME_NEW_DIRECTORY);
            TreeNode treeNode = BuilderTreeView.CreateTreeNode(smrDataTarget.Node, directoryInfo);
            SMRDataDirectory smrDataDirectoryNew = new SMRDataDirectory(treeNode, directoryInfo);

            smrStorage.CreateSMRData(new List<ISMRData> { smrDataDirectoryNew });
        }

        public void CreateSMRFile(SMRDataDirectory smrDataTarget)
        {
            FileInfo fileInfo = BuilderFileOrDirectory.CreateSMRFile(smrDataTarget.PathToSMRDataDirectory, DataDefault.NAME_NEW_SMR_FILE);
            TreeNode treeNode = BuilderTreeView.CreateTreeNode(smrDataTarget.Node, fileInfo);
            SMRDataSMRFile smrDataFileNew = new SMRDataSMRFile(treeNode, fileInfo);

            smrStorage.CreateSMRData(new List<ISMRData> { smrDataFileNew });
        }

        public void ImportFiles(SMRDataDirectory smrDataTarget)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { 
                Filter = DataDefault.ALL_FILTER, 
                Multiselect = true,
                InitialDirectory = smrStorage.SMRProject.Path, 
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileNames.Length != 0)
            {
                List<FileInfo> files = BuilderFileOrDirectory.ImportFiles(smrDataTarget.PathToSMRDataDirectory, openFileDialog.FileNames).Where(file => !BuilderSMR.CheckOnSMRMetaFile(file)).ToList();
                List<TreeNode> treeNodes = BuilderTreeView.CreateTreeNode(smrDataTarget.Node, files);
                List<ISMRData> smrDataFiles = new List<ISMRData>();

                for (int i = 0; i < treeNodes.Count; i++)
                    smrDataFiles.Add(BuilderSMR.CheckOnSMRFile(files[i]) ? new SMRDataSMRFile(treeNodes[i], files[i]) : new SMRDataFile(treeNodes[i], files[i]));

                smrStorage.CreateSMRData(smrDataFiles);
            }
        }

        public void SMRToolRefresh(SMRDataDirectory smrDataTarget = null)
        {
            smrDataTarget = smrDataTarget ?? smrStorage.SMRDataRoot;

            List<ISMRData> smrDatasDelete = BuilderTreeView.CheckTreeNodesOld(smrDataTarget.Node);

            if (smrDatasDelete.Count > 0)
            {
                smrDatasDelete.ForEach(smrDataDelete =>
                {
                    smrDataDelete.Node.Remove();

                    if (smrDataDelete is SMRDataFile smrDataFile)
                        BuilderSMR.DeleteSMRDataSMRMeta(smrDataFile);

                    if (smrDataDelete.ListViewItem != null)
                        smrDataDelete.ListViewItem.Remove();
                });
                smrStorage.DeleteSMRData(smrDatasDelete);
            }

            List<ISMRData> smrDatasAdd = BuilderTreeView.CheckTreeNodesNew(smrDataTarget.Node, new DirectoryInfo(smrDataTarget.FullPathToSMRData));

            if (smrDatasAdd.Count > 0)
                smrStorage.CreateSMRData(smrDatasAdd);

            smrStorage.RefreshSMRData(smrDataTarget);
        }

        public void SMRToolPaste(SMRDataDirectory smrDataTarget)
        {
            if (SMRDataBuffer.Count == 0)
                return;
            SMRDataBuffer = SMRDataBuffer.Where(smrData => !(smrData is SMRDataDirectoryRoot)).ToList();

            BuilderTreeView.CheckInitializeNodesInTreeNode(smrDataTarget.Node, smrDataTarget.PathToSMRDataDirectory);

            if (currentSMRTool == DataDefault.SMRTool.Cut)
            {
                if (!BuilderSMR.CheckCloseSMRData(SMRDataBuffer))
                    return;

                List<ISMRData> smrDatasMove = BuilderFileOrDirectory.MoveSMRDatas(smrDataTarget.PathToSMRDataDirectory, SMRDataBuffer);
                List<ISMRData> smrDatasUpdate = new List<ISMRData>(smrDatasMove);

                for (int i = 0; i < smrDatasMove.Count; i++)
                {
                    string oldNodePath = smrDatasUpdate[i].Node.FullPath;

                    smrDatasMove[i].Node.Parent.Nodes.Remove(smrDatasMove[i].Node);
                    smrDataTarget.Node.Nodes.Add(smrDatasMove[i].Node);

                    if (smrDatasMove[i] is SMRDataDirectory smrDataDirectory)
                    {
                        List<ISMRData> smrDatasFileInDirectory = new List<ISMRData>();
                        BuilderTreeView.GetListSMRDatasInside(smrDatasFileInDirectory, smrDataDirectory.Node);
                        UpdateSMRData(smrDataDirectory, oldNodePath, smrDatasFileInDirectory, smrDatasUpdate);

                        smrDatasUpdate.AddRange(smrDatasFileInDirectory);
                    }
                    else if (smrDatasMove[i] is SMRDataFile smrDataFile)
                    {
                        UpdateSMRData(smrDataFile, oldNodePath, smrDatasUpdate);
                    }
                }

                smrStorage.UpdateSMRData(smrDatasUpdate);

                if (smrDatasMove != null)
                    smrStorage.ActiveSMRDataTool(smrDatasMove, DataDefault.SMRTool.Paste);
            }

            else if (currentSMRTool == DataDefault.SMRTool.Copy)
            {
                List<FileSystemInfo> fileSystemInfos = BuilderFileOrDirectory.CopySMRDatas(smrDataTarget.PathToSMRDataDirectory, SMRDataBuffer);

                List<DirectoryInfo> dirs = fileSystemInfos.Where(fileSystemInfo => fileSystemInfo is DirectoryInfo).Cast<DirectoryInfo>().ToList();
                List<FileInfo> files = fileSystemInfos.Where(fileSystemInfo => fileSystemInfo is FileInfo fileInfo && fileInfo.Directory.FullName == smrDataTarget.PathToSMRDataDirectory.FullName).Cast<FileInfo>().ToList();

                List<TreeNode> treeNodesDirs = BuilderTreeView.CreateTreeNode(smrDataTarget.Node, dirs);
                List<TreeNode> treeNodesFiles = BuilderTreeView.CreateTreeNode(smrDataTarget.Node, files);

                List<SMRDataDirectory> smrDataDirs = new List<SMRDataDirectory>(treeNodesDirs.Count);
                List<SMRDataFile> smrDataFiles = new List<SMRDataFile>(treeNodesFiles.Count);

                for (int i = 0; i < treeNodesDirs.Count; i++)
                {
                    SMRDataDirectory smrDataDirectoryNew = new SMRDataDirectory(treeNodesDirs[i], dirs[i]);
                    BuilderTreeView.CheckInitializeNodesInTreeNode(treeNodesDirs[i], dirs[i]);
                    smrDataDirs.Add(smrDataDirectoryNew);
                }

                for (int i = 0; i < treeNodesFiles.Count; i++)
                {
                    if (!BuilderSMR.CheckOnSMRMetaFile(files[i]))
                        smrDataFiles.Add(BuilderSMR.CheckOnSMRFile(files[i]) ? new SMRDataSMRFile(treeNodesFiles[i], files[i]) : new SMRDataFile(treeNodesFiles[i], files[i]));
                }

                List<ISMRData> smrDataCreate = new List<ISMRData>();
                smrDataCreate.AddRange(smrDataDirs);
                smrDataCreate.AddRange(smrDataFiles);

                smrStorage.CreateSMRData(smrDataCreate);

                smrStorage.ActiveSMRDataTool(SMRDataBuffer, DataDefault.SMRTool.Paste);
                smrStorage.ActiveSMRDataTool(SMRDataBuffer, DataDefault.SMRTool.Copy);
            }
        }

        public void SMRToolOpen(ISMRData smrData)
        {
            if (smrData is SMRDataDirectory smrDataDirectory)
                smrStorage.ReadSMRDataDirectory(smrDataDirectory);

            else if (smrData is SMRDataSMRFile smrDataSMRFile)
                smrStorage.OpenSMRFile(smrDataSMRFile);

            else if (smrData is SMRDataFile smrDataFile)
                smrStorage.OpenFile(smrDataFile);
        }

        public void SMRToolCut(List<ISMRData> smrDatas = null)
        {
            smrDatas = smrDatas ?? selectedListViewItems;
            SMRToolCancel();

            if (smrDatas.Count > 0)
            {
                SMRDataBuffer.Clear();
                SMRDataBuffer.AddRange(smrDatas);
                smrStorage.ActiveSMRDataTool(SMRDataBuffer, DataDefault.SMRTool.Cut);
            }
        }

        public void SMRToolCopy()
        {
            SMRToolCancel();

            if (selectedListViewItems.Count > 0)
            {
                SMRDataBuffer.Clear();
                SMRDataBuffer.AddRange(selectedListViewItems);
                smrStorage.ActiveSMRDataTool(SMRDataBuffer, DataDefault.SMRTool.Copy);
            }
        }

        public void SMRToolCancel()
        {
            SMRDataBuffer.Clear();
            smrStorage.ActiveSMRDataTool(SMRDataBuffer, DataDefault.SMRTool.None);
        }

        public void SMRToolDelete()
        {
            SMRDataBuffer.Clear();
            SMRDataBuffer.AddRange(selectedListViewItems);

            if (SMRDataBuffer.Count == 0)
                return;

            if (DialogWindow.MessageWarning("Перместить выбранные файлы в корзину?") == DialogResult.Yes)
            {
                if (!BuilderSMR.CheckCloseSMRData(SMRDataBuffer))
                    return;

                List<ISMRData> smrDatasDelete = BuilderFileOrDirectory.DeleteSMRDatas(SMRDataBuffer);

                smrDatasDelete.ForEach(smrDataDelete =>
                {
                    if (smrDataDelete is SMRDataFile smrDataFile)
                    {
                        smrDataFile.DataMeta.links.ForEach(link =>
                        {
                            ISMRData smrDataFind = FindSMRData(link.LinkTreeNode);
                            if (smrDataFind is SMRDataFile smrDataFindFile)
                            {
                                LinkToFile linkTofile = smrDataFindFile.DataMeta.links.Find(linkTofileFind => linkTofileFind.LinkTreeNode == smrDataDelete.Node.FullPath);
                                
                                if (linkTofile != null)
                                    smrDataFindFile.DataMeta.links.Remove(linkTofile);
                            }
                        });
                        BuilderSMR.DeleteSMRDataSMRMeta(smrDataFile);
                    }

                    smrDataDelete.Node.Remove();

                    if (smrDataDelete.ListViewItem != null)
                        smrDataDelete.ListViewItem.Remove();
                });
                smrStorage.DeleteSMRData(smrDatasDelete);
                smrStorage.ActiveSMRDataTool(smrDatasDelete, DataDefault.SMRTool.Delete);
                SMRToolCancel();
            }
        }

        public void UpdateSMRData(SMRDataFile smrDataFile, string oldNodePathToFile, List<ISMRData> smrDatasUpdate)
        {
            BuilderSMR.DeleteSMRDataSMRMeta(smrDataFile);
            smrDataFile.UpdateFile();

            for (int i = 0; i < smrDataFile.DataMeta.links.Count; i++)
            {
                ISMRData smrDataLink = FindSMRData(smrDataFile.DataMeta.links[i].LinkTreeNode);

                if (smrDataLink == null)
                {
                    smrDataLink = smrDatasUpdate.FirstOrDefault(smrDataUpdate => smrDataUpdate.Node.FullPath == smrDataFile.DataMeta.links[i].LinkTreeNode);

                    if (smrDataLink == null)
                        continue;
                }

                if (smrDataLink is SMRDataFile smrDataFileLink)
                {
                    int indexLinkToReplace = smrDataFileLink.DataMeta.links.FindIndex(linkToFile => linkToFile.LinkTreeNode == oldNodePathToFile);

                    if (indexLinkToReplace != -1)
                    {
                        smrDataFileLink.DataMeta.links[indexLinkToReplace].LinkTreeNode = smrDataFile.Node.FullPath;
                        smrDataFileLink.DataMeta.links[indexLinkToReplace].Link = smrDataFile.FullPathToSMRData;
                        DataSerialize.WriteData(smrDataFileLink.DataMeta, smrDataFileLink.FullPathToSMRDataMeta);
                        smrDatasUpdate.Add(smrDataLink);
                    }
                }
            }
            DataSerialize.WriteData(smrDataFile.DataMeta, smrDataFile.FullPathToSMRDataMeta);
        }

        public void UpdateSMRData(SMRDataDirectory smrDataDirectory, string oldNodePathToDirectory, List<ISMRData> smrDatasFileInDirectory, List<ISMRData> smrDatasUpdate)
        {
            smrDataDirectory.UpdateDirectory();
            int count = smrDatasFileInDirectory.Count;

            for (int i = 0; i < count; i++)
            {
                if (smrDatasFileInDirectory[i] is SMRDataDirectory smrDataDirectoryFind)
                {
                    string path = smrDataDirectoryFind.PathToSMRDataDirectory.FullName.Replace(oldNodePathToDirectory, smrDataDirectory.Node.FullPath);
                    smrDataDirectoryFind.PathToSMRDataDirectory = new DirectoryInfo(path);
                    smrDataDirectoryFind.UpdateDirectory();
                }

                else if (smrDatasFileInDirectory[i] is SMRDataFile smrDataFileFind)
                {
                    smrDataFileFind.PathToSMRDataFile = new FileInfo(smrDataFileFind.PathToSMRDataFile.FullName.Replace(oldNodePathToDirectory, smrDataDirectory.Node.FullPath));
                    smrDataFileFind.UpdateFile();

                    for (int j = 0; j < smrDataFileFind.DataMeta.links.Count; j++)
                    {
                        if (smrDataFileFind.DataMeta.links[j].LinkTreeNode.StartsWith(oldNodePathToDirectory))
                        {
                            smrDataFileFind.DataMeta.links[j].LinkTreeNode = smrDataDirectory.Node.FullPath + smrDataFileFind.DataMeta.links[j].LinkTreeNode.Remove(0, oldNodePathToDirectory.Length);
                            smrDataFileFind.DataMeta.links[j].Link = Path.Combine(smrStorage.SMRDataRoot.PathToSMRDataDirectory.Parent.FullName, smrDataFileFind.DataMeta.links[j].LinkTreeNode);
                        }

                        else
                        {
                            ISMRData smrDataLink = FindSMRData(smrDataFileFind.DataMeta.links[j].LinkTreeNode);

                            if (smrDataLink == null)
                            {
                                smrDataLink = smrDatasUpdate.FirstOrDefault(smrDataUpdate => smrDataUpdate.Node.FullPath == smrDataFileFind.DataMeta.links[j].LinkTreeNode) ?? smrDatasFileInDirectory.FirstOrDefault(smrDataUpdate => smrDataUpdate.Node.FullPath == smrDataFileFind.DataMeta.links[j].LinkTreeNode);

                                if (smrDataLink == null)
                                    continue;
                            }

                            if (smrDataLink is SMRDataFile smrDataFileLink)
                            {
                                string oldNodePathToFile = oldNodePathToDirectory + smrDataFileFind.Node.FullPath.Remove(0, smrDataDirectory.Node.FullPath.Length);
                                int indexLinkToReplace = smrDataFileLink.DataMeta.links.FindIndex(linkToFile => linkToFile.LinkTreeNode == oldNodePathToFile);

                                if (indexLinkToReplace != -1)
                                {
                                    smrDataFileLink.DataMeta.links[indexLinkToReplace].LinkTreeNode = smrDataFileFind.Node.FullPath;
                                    smrDataFileLink.DataMeta.links[indexLinkToReplace].Link = smrDataFileFind.FullPathToSMRData;
                                    DataSerialize.WriteData(smrDataFileLink.DataMeta, smrDataFileLink.FullPathToSMRDataMeta);
                                    smrDatasFileInDirectory.Add(smrDataLink);
                                }
                            }
                        }
                    }
                    DataSerialize.WriteData(smrDataFileFind.DataMeta, smrDataFileFind.FullPathToSMRDataMeta);
                }
            }
            smrDatasFileInDirectory.Add(smrDataDirectory);
        }

        public void CheckUpdateActivePath(ISMRData smrData)
        {
            if (smrData is SMRDataDirectory smrDataDirectory)
            {
                if (smrStorage.SMRDataDirectoryCurrent == smrDataDirectory)
                    return;

                if (!smrDataDirectory.Node.Checked)
                    CheckInitializeNodesInTreeNode(smrDataDirectory);

                smrStorage.ReadSMRDataDirectory(smrDataDirectory);
            }

            else if (smrData is SMRDataFile smrDataSMRFile)
            {
                if (!(smrDataSMRFile.Node?.Parent?.Tag is SMRDataDirectory smrDataFileParent))
                {
                    smrStorage.ReadSMRDataDirectory(smrStorage.SMRDataRoot);
                    return;
                }

                CheckUpdateActivePath(smrDataFileParent);
                smrStorage.UpdateSelectedFiles(new List<ISMRData>() { smrDataSMRFile });
            }
        }

        public void CheckInitializeNodesInTreeNode(ISMRData smrData)
        {
            if (smrData is SMRDataDirectory smrDataDirectory)
                BuilderTreeView.CheckInitializeNodesInTreeNode(smrDataDirectory.Node, smrDataDirectory.PathToSMRDataDirectory);

            else if (smrData is SMRDataFile smrDataFile && smrDataFile.Node?.Parent?.Tag is SMRDataDirectory smrDataDirectoryParent)
                BuilderTreeView.CheckInitializeNodesInTreeNode(smrDataDirectoryParent.Node, smrDataDirectoryParent.PathToSMRDataDirectory);
        }

        private void OnActivedSMRDataTool(List<ISMRData> smrDatas, DataDefault.SMRTool smrTool)
        {
            if (smrTool == DataDefault.SMRTool.Paste)
            {
                if (currentSMRTool == DataDefault.SMRTool.Cut)
                    SMRToolCancel();
                else
                    smrStorage.ActiveSMRDataTool(smrDatas, DataDefault.SMRTool.Copy);
            }

            else if (smrTool == DataDefault.SMRTool.Update)
                smrStorage.ActiveSMRDataTool(smrDatas, DataDefault.SMRTool.None);

            else
                currentSMRTool = smrTool;
        }

        private void OnFormClosed()
        {
            smrStorage.ActivedSMRDataToolHandler -= OnActivedSMRDataTool;
            smrStorage.OnSelectedFilesHandler -= OnSelectedFiles;
            smrStorage.FormClosedHandler -= OnFormClosed;
        }

        private void OnSelectedFiles(List<ISMRData> selectedListViewItems) => this.selectedListViewItems = selectedListViewItems;
    }
}
