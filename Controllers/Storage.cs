using System;
using System.IO;
using SNAMP.Utils;
using SNAMP.Models;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP
{
    public class Storage
    {
        public event Action<SMRProject> AddSMRProjectHandler;
        public event Action<SMRProject> DeleteSMRProjectHandler;
        public event Action<List<SMRProject>> UpdateSMRProjectsHandler;
        public event Action<SMRForm> OpenSMRProjectHandler;
        public event Action<SMRForm> CloseSMRProjectHandler;
        
        public SMRHubForm smrHubForm;

        public List<SMRForm> activeSMRForms;

        private DataProjects DataProjects
        { 
            get
            {
                if (_dataProjects == null)
                    _dataProjects = DataSerialize.ReadData<DataProjects>(DataDefault.NAME_DATA_PROJECTS);

                return _dataProjects;
            }
            set => _dataProjects = value; 
        }
        private DataBuilder DataBuilder
        {
            get
            {
                if (_dataBuilder == null)
                    _dataBuilder = DataSerialize.ReadData<DataBuilder>(DataDefault.NAME_DATA_BUILDER);

                return _dataBuilder;
            }
            set => _dataBuilder = value;
        }

        private DataProjects _dataProjects;
        private DataBuilder _dataBuilder;

        public Storage(SMRHubForm smrHubForm)
        {
            this.smrHubForm = smrHubForm;
            activeSMRForms = new List<SMRForm>();
        }

        public void CreateSMRProject()
        {
            NewProjectForm newProjectForm = new NewProjectForm();

            if (newProjectForm.ShowDialog() != DialogResult.OK)
                return;

            if (newProjectForm.data.isCreateFolder)
            {
                newProjectForm.data.path = Path.Combine(newProjectForm.data.path, newProjectForm.data.name);
                Directory.CreateDirectory(newProjectForm.data.path);
            }

            SMRProject newProject = new SMRProject(newProjectForm.data.name, newProjectForm.data.path, DateTime.Now);

            BuilderSMR.BuildSMRProject(newProject, DataBuilder.directories);

            AddSMRProject(newProject);
            OpenSMRProject(newProject);
        }

        public void OpenSMRProject(string filePath)
        {
            FileInfo file = new FileInfo(filePath);

            if (!BuilderSMR.CheckOnSMRProjectFile(file))
            {
                DialogWindow.MessageError("Необходимо выбрать SMRP проект: *.smrp");
                return;
            }

            SMRProject smrProject = new SMRProject(file.Name.Replace(file.Extension, ""), file.DirectoryName, DateTime.Now);

            AddSMRProject(smrProject);
            OpenSMRProject(smrProject);
        }

        public void OpenSMRProject(SMRProject smrProject)
        {
            SMRForm smrFormFind = activeSMRForms.Find(smr => smr.smrStorage.SMRProject.GetFullPath() == smrProject.GetFullPath());

            if (smrFormFind != null)
            {
                smrFormFind.Show();
                smrHubForm.Hide();
                return;
            }

            SMRForm smrForm = new SMRForm(this, smrProject);
            activeSMRForms.Add(smrForm);
            OpenSMRProjectHandler?.Invoke(smrForm);
            smrForm.Show();
        }

        public void AddSMRProject(SMRProject smrProject)
        {
            if (HasSMRProject(smrProject))
                return;

            DataProjects.projects.Insert(0, smrProject);
            DataSerialize.WriteData(DataProjects, DataDefault.NAME_DATA_PROJECTS);
            AddSMRProjectHandler?.Invoke(smrProject);
        }

        public void DeleteSMRProject(SMRProject smrProject)
        {
            if (!HasSMRProject(smrProject))
                return;

            DataProjects.projects.Remove(smrProject);
            DataSerialize.WriteData(DataProjects, DataDefault.NAME_DATA_PROJECTS);
            DeleteSMRProjectHandler?.Invoke(smrProject);
        }

        public void CloseSMRProject(SMRForm smrForm)
        {
            activeSMRForms.Remove(smrForm);
            DataSerialize.WriteData(DataProjects, DataDefault.NAME_DATA_PROJECTS);
            UpdateSMRProjects();
            CloseSMRProjectHandler?.Invoke(smrForm);
        }

        public void UpdateSMRProjects() => UpdateSMRProjectsHandler?.Invoke(DataProjects.projects);

        private bool HasSMRProject(SMRProject smrProject) => DataProjects.projects.Find(pr => pr.Equals(smrProject)) != null;
    }  
}

