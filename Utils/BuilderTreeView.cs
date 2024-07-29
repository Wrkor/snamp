using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SNAMP.Utils
{
    public static class BuilderTreeView
    {
        public static List<ISMRData> CheckTreeNodesOld(TreeNode treeNodeTarget)
        {
            List<ISMRData> smrDatas = new List<ISMRData>();
            List<ISMRData> smrDatasDelete = new List<ISMRData>();
            GetListSMRDatasInside(smrDatas, treeNodeTarget);

            for (int i = 0; i < smrDatas.Count; i++)
            {
                if (!File.Exists(smrDatas[i].FullPathToSMRData) && !Directory.Exists(smrDatas[i].FullPathToSMRData))
                    smrDatasDelete.Add(smrDatas[i]);
            }
            return smrDatasDelete;
        }

        public static List<ISMRData> CheckTreeNodesNew(TreeNode treeNodeTarget, DirectoryInfo directoryInfoTarget)
        {
            directoryInfoTarget.Refresh();
            List<ISMRData> smrDataNew = new List<ISMRData>();
            if (treeNodeTarget.Checked)
            {
                foreach (DirectoryInfo dir in directoryInfoTarget.GetDirectories())
                {

                    if (treeNodeTarget.Nodes.Find(dir.Name, false).Length == 0)
                    {
                        TreeNode nodeDir = CreateTreeNode(treeNodeTarget, dir);
                        smrDataNew.Add(new SMRDataDirectory(nodeDir, dir));

                        DirectoryInfo[] dirs = dir.GetDirectories();
                        FileInfo files = dir.GetFiles().FirstOrDefault(file => !BuilderSMR.CheckOnSMRProjectFile(file) && !BuilderSMR.CheckOnSMRMetaFile(file));

                        if (dirs.Length != 0 || files != null)
                            nodeDir.Nodes.Add(new TreeNode());
                    }
                }
                
                foreach (FileInfo file in directoryInfoTarget.GetFiles())
                {
                    if (BuilderSMR.CheckOnSMRProjectFile(file))
                        continue;

                    if (BuilderSMR.CheckOnSMRMetaFile(file))
                    {
                        if (file.Exists)
                        {
                            if (!File.Exists(BuilderSMR.GetPathToSMRDataFileBySMRMeta(file)))
                                file.Delete();
                        }

                        continue;
                    }

                    if (treeNodeTarget.Nodes.Find(file.Name, false).Length == 0)
                    {
                        TreeNode nodeFile = CreateTreeNode(treeNodeTarget, file);
                        SMRDataFile smrData = BuilderSMR.CheckOnSMRFile(file) ? new SMRDataSMRFile(nodeFile, file) : new SMRDataFile(nodeFile, file);
                        smrDataNew.Add(smrData);
                    }
                }
            }
            return smrDataNew;
        }

        public static List<ISMRData> GetListSMRDatasInside(List<ISMRData> smrDatas, TreeNode treeNodeParent)
        {
            foreach (TreeNode treeNode in treeNodeParent.Nodes)
            {
                if (treeNode?.Tag is ISMRData smrData)
                {
                    if (treeNode.Checked && treeNode.GetNodeCount(false) > 0)
                        GetListSMRDatasInside(smrDatas, treeNode);

                    smrDatas.Add(smrData);
                }
            }

            return smrDatas;
        }

        public static List<TreeNode> CreateTreeNode(TreeNode treeNode, List<FileInfo> files)
        {
            List<TreeNode> treeNodes = files.Select(file => new TreeNode() { Text = file.Name, Name = file.Name }).ToList();
            treeNode.Nodes.AddRange(treeNodes.ToArray());

            return treeNodes;
        }

        public static List<TreeNode> CreateTreeNode(TreeNode treeNode, List<DirectoryInfo> files)
        {
            List<TreeNode> treeNodes = files.Select(file => new TreeNode() { Text = file.Name, Name = file.Name }).ToList();
            treeNode.Nodes.AddRange(treeNodes.ToArray());

            return treeNodes;
        }
        
        public static List<string> GetListNodesExpanded(List<string> nodeExpanded, TreeNode treeNodeParent)
        {
            foreach (TreeNode treeNode in treeNodeParent.Nodes)
            {
                if (treeNode.Checked)
                {
                    if (treeNode.IsExpanded && treeNode?.Tag is SMRDataDirectory smrDataDirectory)
                        nodeExpanded.Add(smrDataDirectory.Node.FullPath);

                    GetListNodesExpanded(nodeExpanded, treeNode);
                }
            }

            return nodeExpanded;
        }

        public static TreeNode CreateTreeNode(TreeNode treeNode, DirectoryInfo directoryInfo)
        {
            TreeNode newTreeNode = new TreeNode() { Text = directoryInfo.Name, Name = directoryInfo.Name };
            treeNode.Nodes.Add(newTreeNode);

            return newTreeNode;
        }

        public static TreeNode CreateTreeNode(TreeNode treeNode, FileInfo fileInfo)
        {
            TreeNode newTreeNode = new TreeNode() { Text = fileInfo.Name, Name = fileInfo.Name };
            treeNode.Nodes.Add(newTreeNode);

            return newTreeNode;
        }

        public static string GetCheckTreeNodeName(string name, TreeNode treeNodeTarget)
        {
            if (IsNodeNameInNodes(name, treeNodeTarget.Nodes))
            {
                Match matchSuffix = Regex.Match(name, DataDefault.REG_SUFIX);

                if (!matchSuffix.Success)
                {
                    name = name.TrimEnd() + " (1)";
                    matchSuffix = Regex.Match(name, DataDefault.REG_SUFIX);
                }

                Match matchNumber = Regex.Match(matchSuffix.Groups[0].Value, DataDefault.REG_NUMBER);
                int suffix = Convert.ToInt32(matchNumber.Groups[0].Value);

                string clearName = name.Replace(matchSuffix.Groups[0].Value, "").TrimEnd();

                while (IsNodeNameInNodes(name, treeNodeTarget.Nodes))
                    name = $"{clearName} ({++suffix})";
            }
            return name;
        }

        public static bool IsNodeNameInNodes(string name, TreeNodeCollection treeNodes)
        {
            foreach (TreeNode treeNode in treeNodes)
                if (treeNode.Text.Trim() == name.Trim())
                    return true;

            return false;
        }

        public static void CheckInitializeNodesInTreeNode(TreeNode treeNode, DirectoryInfo targetDir)
        {
            if (treeNode.Checked)
                return;

            treeNode.Nodes.Clear();
            CreateOpenTreeNodes(new List<string>(), treeNode, targetDir);
        }

        public static void CreateOpenTreeNodes(List<string> nodeExpanded, TreeNode treeNode, DirectoryInfo targetDir)
        {
            treeNode.Checked = true;
            targetDir.Refresh();
            foreach (DirectoryInfo dir in targetDir.GetDirectories())
            {
                TreeNode nodeDir = CreateTreeNode(treeNode, dir);
                new SMRDataDirectory(nodeDir, dir);

                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo files = dir.GetFiles().FirstOrDefault(file => !BuilderSMR.CheckOnSMRProjectFile(file) && !BuilderSMR.CheckOnSMRMetaFile(file));

                if (dirs.Length != 0 || files != null)
                    nodeDir.Nodes.Add(new TreeNode());

                if (nodeExpanded.Contains(nodeDir.FullPath))
                {
                    nodeDir.Nodes.Clear();
                    CreateOpenTreeNodes(nodeExpanded, nodeDir, dir);
                    nodeDir.Checked = true;
                    nodeDir.Expand();
                }
            }

            foreach (FileInfo file in targetDir.GetFiles())
            {
                if (BuilderSMR.CheckOnSMRProjectFile(file))
                    continue;

                if (BuilderSMR.CheckOnSMRMetaFile(file))
                {
                    if (file.Exists)
                    {
                        if (!File.Exists(BuilderSMR.GetPathToSMRDataFileBySMRMeta(file)))
                            file.Delete();
                    }

                    continue;
                }

                TreeNode nodeFile = CreateTreeNode(treeNode, file);
                SMRDataFile smrData = BuilderSMR.CheckOnSMRFile(file) ? new SMRDataSMRFile(nodeFile, file) : new SMRDataFile(nodeFile, file);
            }
        }


    }
}
