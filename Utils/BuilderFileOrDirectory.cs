using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SNAMP.Utils
{
    public static class BuilderFileOrDirectory
    {
        public static List<ISMRData> DeleteSMRDatas(IEnumerable<ISMRData> smrDatas)
        {
            List<ISMRData> smrDatasDelete = new List<ISMRData>();

            foreach (ISMRData smrData in smrDatas)
            {
                try
                {
                    if (smrData is SMRDataDirectory smrDataDirectory && !(smrData is SMRDataDirectoryRoot))
                    {
                        Directory.Delete(smrDataDirectory.FullPathToSMRData, true);
                        smrDatasDelete.Add(smrData);
                    }

                    else if (smrData is SMRDataFile smrDataFile)
                    {
                        File.Delete(smrDataFile.FullPathToSMRData);

                        if (File.Exists(smrDataFile.FullPathToSMRDataMeta))
                            File.Delete(smrDataFile.FullPathToSMRData);

                        smrDatasDelete.Add(smrData);
                    }
                }

                catch (IOException)
                {
                    DialogWindow.MessageError("Перед удалением закройте просматриваемый файл: " + smrData.FullPathToSMRData);
                }

                catch 
                {
                    DialogWindow.MessageError("Непредвиденная ошибка удаления (возможно объект используется другими процессами): " + smrData.FullPathToSMRData);
                }
            }

            return smrDatasDelete;
        }
        
        public static List<ISMRData> MoveSMRDatas(DirectoryInfo targetDir, IEnumerable<ISMRData> smrDatas)
        {
            List<ISMRData> smrDataMove = new List<ISMRData>();

            foreach (ISMRData smrData in smrDatas)
            {
                try
                {
                    if (smrData is SMRDataDirectory smrDataDirectory)
                    {
                        if (targetDir.FullName.Contains(smrDataDirectory.PathToSMRDataDirectory.FullName))
                        {
                            DialogWindow.MessageError("Нельзя перместить папку внутрь этой же папки: " + smrDataDirectory.FullPathToSMRData);
                            return null;
                        }

                        if (smrDataDirectory.PathToSMRDataDirectory.Parent.FullName == targetDir.FullName)
                            continue;
                        string path = GetCheckedFullPath(targetDir, smrDataDirectory.PathToSMRDataDirectory);
                        smrDataDirectory.PathToSMRDataDirectory.MoveTo(path);
                        smrDataMove.Add(smrData);
                    }

                    else if (smrData is SMRDataFile smrDataFile)
                    {
                        if (smrDataFile.PathToSMRDataFile.Directory.FullName == targetDir.FullName)
                            continue;

                        string path = GetCheckedFullPath(targetDir, smrDataFile.PathToSMRDataFile);
                        smrDataFile.PathToSMRDataFile.MoveTo(path);
                        smrDataMove.Add(smrData);
                    }
                }
                catch
                {
                    DialogWindow.MessageError("Не предвиденная ошибка c копированием (возможно объект используется другими процессами): " + smrData.FullPathToSMRData);
                    continue;
                }
            }

            return smrDataMove;
        }
        
        public static List<FileSystemInfo> CopySMRDatas(DirectoryInfo targetDir, IEnumerable<ISMRData> smrDatas)
        {
            List<FileSystemInfo> fileSystemInfosTo = new List<FileSystemInfo>();

            foreach (ISMRData smrData in smrDatas)
            {
                try
                {
                    if (smrData is SMRDataDirectory smrDataDirectory)
                    {
                        DirectoryInfo newDirRoot = Directory.CreateDirectory(GetCheckedFullPath(targetDir, smrDataDirectory.PathToSMRDataDirectory));
                        fileSystemInfosTo.Add(newDirRoot);

                        foreach (string dirPath in Directory.GetDirectories(smrDataDirectory.FullPathToSMRData, "*", SearchOption.AllDirectories))
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath.Replace(smrDataDirectory.FullPathToSMRData, newDirRoot.FullName));
                            directoryInfo.Create();
                        }

                        foreach (FileInfo file in smrDataDirectory.PathToSMRDataDirectory.GetFiles(DataDefault.FILE_FILTER, SearchOption.AllDirectories))
                        {
                            if (BuilderSMR.CheckOnSMRMetaFile(file))
                                continue;

                            string path = file.FullName.Replace(smrDataDirectory.FullPathToSMRData, newDirRoot.FullName);

                            File.Copy(file.FullName, path);
                        }
                    }

                    else if (smrData is SMRDataFile smrDataFile)
                    {
                        string newPath = GetCheckedFullPath(targetDir, smrDataFile.PathToSMRDataFile);

                        File.Copy(smrDataFile.FullPathToSMRData, newPath);
                        fileSystemInfosTo.Add(new FileInfo(newPath));
                    }
                }
                catch 
                {
                    DialogWindow.MessageError("Не предвиденная ошибка c копированием (возможно объект используется другими процессами): " + smrData.FullPathToSMRData);
                }
                
            }

            return fileSystemInfosTo;
        }

        public static List<FileInfo> ImportFiles(DirectoryInfo targetDir, string[] filePaths)
        {
            List<FileInfo> files = new List<FileInfo>();

            foreach (string filePathFrom in filePaths)
            {
                FileInfo filePathTo = new FileInfo(GetCheckedFullPath(targetDir, new FileInfo(filePathFrom)));

                if (BuilderSMR.CheckOnSMRProjectFile(filePathTo))
                {
                    DialogWindow.MessageError("Проектный файл нельзя импортировать: " + filePathFrom);
                    continue;
                }
                try
                {
                    File.Copy(filePathFrom, filePathTo.FullName);
                    filePathTo.Refresh();
                    files.Add(filePathTo);
                }
                catch
                {
                    DialogWindow.MessageError("Не предвиденная ошибка c копированием файла (возможно объект используется другими процессами): " + targetDir.FullName);
                }
            }
            return files;
        }
        
        public static DirectoryInfo CreateDirectory(DirectoryInfo targetDir, string name)
        {
            try
            {
                DirectoryInfo dirNew = new DirectoryInfo(Path.Combine(targetDir.FullName, name));
                DirectoryInfo dirNewCheck = new DirectoryInfo(GetCheckedFullPath(targetDir, dirNew));
                dirNewCheck.Create();
                dirNewCheck.Refresh();
                return dirNewCheck;
            }
            catch
            {
                DialogWindow.MessageError("Ошибка создания папки (возможно объект используется другими процессами): " + name);
                return null;
            }
        }

        public static FileInfo CreateSMRFile(DirectoryInfo targetDir, string name)
        {
            try
            {
                FileInfo fileNew = new FileInfo(Path.Combine(targetDir.FullName, name + DataDefault.SMR_EXT));
                FileInfo fileNewCheck = new FileInfo(GetCheckedFullPath(targetDir, fileNew));
                fileNewCheck.Create().Close();
                fileNewCheck.Refresh();
                return fileNewCheck;
            }
            catch
            {
                DialogWindow.MessageError("Ошибка создания SMR файла: " + name);
                return null;
            }
        }

        public static bool Rename(FileInfo file, string newName)
        {
            if (!file.Exists || string.IsNullOrWhiteSpace(newName))
                return false;

            string newPath = Path.Combine(file.DirectoryName, newName);

            try
            {
                if (File.Exists(newPath) && newPath.ToLower() != file.FullName.ToLower())
                {
                    DialogWindow.MessageError("Файл с таким именем уже существует: " + newName);
                    return false;
                }

                file.MoveTo(newPath);
                return true;
            }
            catch
            {
                DialogWindow.MessageError("Ошибка переименования (возможно объект используется другими процессами): " + newPath);
                return false;
            }
            
        }

        public static bool Rename(DirectoryInfo dir, string newName)
        {
            if (!dir.Exists || string.IsNullOrWhiteSpace(newName) || newName.Contains("."))
                return false;

            string newPath = Path.Combine(dir.Parent.FullName, newName);

            try
            {
                if (Directory.Exists(newPath))
                {
                    DialogWindow.MessageError("Папка с таким именем уже существует: " + newName);
                    return false;
                }

                dir.MoveTo(newPath);
                return true;
            }

            catch
            {
                DialogWindow.MessageError("Ошибка переименования (возможно объект используется другими процессами): " + newPath);
                return false;
            }
        }

        private static string GetCheckedFullPath(DirectoryInfo targetDir, FileInfo fileInfo)
        {
            string filePathTo = Path.Combine(targetDir.FullName, fileInfo.Name);
            try
            {
                if (File.Exists(filePathTo))
                {
                    Match matchSuffix = Regex.Match(new FileInfo(filePathTo).Name, DataDefault.REG_SUFIX);

                    if (!matchSuffix.Success)
                    {
                        filePathTo = string.IsNullOrEmpty(fileInfo.Extension) 
                            ? 
                                Path.Combine(targetDir.FullName, $"{fileInfo.Name} (2)")
                            :
                                Path.Combine(targetDir.FullName, $"{fileInfo.Name.Replace(fileInfo.Extension, "")} (2){fileInfo.Extension}");
                        
                        matchSuffix = Regex.Match(filePathTo, DataDefault.REG_SUFIX);

                        if (!matchSuffix.Success)
                            throw new Exception();
                    }

                    Match matchNumber = Regex.Match(matchSuffix.Groups[0].Value, DataDefault.REG_NUMBER);

                    if (!matchNumber.Success)
                        throw new Exception();

                    
                    int suffix = Convert.ToInt32(matchNumber.Groups[0].Value);

                    if (string.IsNullOrEmpty(fileInfo.Extension))
                    {
                        string fullName = Path.Combine(targetDir.FullName, fileInfo.Name.Replace(matchSuffix.Groups[0].Value, "").TrimEnd());
                        while (File.Exists(filePathTo))
                            filePathTo = $"{fullName} ({++suffix})";
                    }
                    else
                    {
                        string fullName = Path.Combine(targetDir.FullName, fileInfo.Name.Replace(fileInfo.Extension, "").Replace(matchSuffix.Groups[0].Value, "").TrimEnd()); 
                        while (File.Exists(filePathTo))
                            filePathTo = $"{fullName} ({++suffix}){fileInfo.Extension}";
                    }
                }
            }

            catch 
            { 
                DialogWindow.MessageError("Неккоректное имя файла: " + fileInfo.FullName); 
            }

            return filePathTo;
        }

        private static string GetCheckedFullPath(DirectoryInfo targetDir, DirectoryInfo directoryInfo)
        {
            string directoryPathTo = Path.Combine(targetDir.FullName, directoryInfo.Name);
            try
            {
                if (Directory.Exists(directoryPathTo))
                {
                    Match matchSuffix = Regex.Match(new DirectoryInfo(directoryPathTo).Name, DataDefault.REG_SUFIX);
                    
                    if (!matchSuffix.Success)
                    {
                        directoryPathTo = Path.Combine(targetDir.FullName, $"{directoryInfo.Name} (2)");
                        matchSuffix = Regex.Match(directoryPathTo, DataDefault.REG_SUFIX);

                        if (!matchSuffix.Success)
                            throw new Exception();
                    }

                    Match matchNumber = Regex.Match(matchSuffix.Groups[0].Value, DataDefault.REG_NUMBER);

                    if (!matchNumber.Success)
                        throw new Exception();

                    int suffix = Convert.ToInt32(matchNumber.Groups[0].Value);

                    string fullName = Path.Combine(targetDir.FullName, directoryInfo.Name.Replace(matchSuffix.Groups[0].Value, "").TrimEnd());

                    while (Directory.Exists(directoryPathTo))
                        directoryPathTo = $"{fullName} ({++suffix})";
                }
            }

            catch 
            {
                DialogWindow.MessageError("Неккоректное имя папки: " + directoryInfo.FullName);
            }

            return directoryPathTo;
        }
    }
}
