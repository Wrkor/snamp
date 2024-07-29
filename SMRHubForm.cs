using System;
using System.IO;
using SNAMP.Views;
using System.Drawing;
using System.Windows.Forms;

namespace SNAMP
{
    public partial class SMRHubForm : Form
    {
        public Storage storage;

        public SMRHubForm()
        {
            InitializeComponent();
            InitializeElements();
        }

        private void InitializeElements()
        {
            storage = new Storage(this);
            storage.OpenSMRProjectHandler += OnOpenSMRProject;
            storage.CloseSMRProjectHandler += OnCloseSMRProject;
            FormClosing += OnSMRHubFormFormClosing;
            FormClosed += OnSMRHubFormFormClosed;

            Panel pannelTable = new Panel() { Location = new Point(40, 220), Size = new Size(900, 322), Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left };
            pannelTable.Controls.Add(new TableLayoutPanelSMRProjects(storage));

            Controls.Add(pannelTable);
            storage.UpdateSMRProjects();
            ButtonSettings.Click += OnButtonSettingsClick;
            ButtonNewProject.Click += OnButtonNewProjectClick;
            ButtonOpenProject.Click += OnButtonOpenProjectClick;
        }

        private void OnSMRHubFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (storage.activeSMRForms.Count > 0)
            {
                e.Cancel = true;
                Hide();
                return;
            }
        }

        private void OnSMRHubFormFormClosed(object sender, FormClosedEventArgs e)
        {
            storage.OpenSMRProjectHandler -= OnOpenSMRProject;
            storage.CloseSMRProjectHandler -= OnCloseSMRProject;
            ButtonSettings.Click -= OnButtonSettingsClick;
            ButtonNewProject.Click -= OnButtonNewProjectClick;
            ButtonOpenProject.Click -= OnButtonOpenProjectClick;
            FormClosing -= OnSMRHubFormFormClosing;
            FormClosed -= OnSMRHubFormFormClosed;
        }

        private void OnCloseSMRProject(SMRForm smrForm)
        {
            if (storage.activeSMRForms.Count == 0)
                Show();
        }

        private void OnButtonOpenProjectClick(object sender, EventArgs e)
        {
            OpenFileDialog openSMRFileDialog = new OpenFileDialog() { Filter = DataDefault.SMRP_FILTER };

            if (openSMRFileDialog.ShowDialog() == DialogResult.OK && File.Exists(openSMRFileDialog.FileName))
                storage.OpenSMRProject(openSMRFileDialog.FileName);
        }

        private void OnOpenSMRProject(SMRForm smrForm) => Hide();

        private void OnButtonSettingsClick(object sender, EventArgs e) => new SettingsTemplatesForm().ShowDialog();
       
        private void OnButtonNewProjectClick(object sender, EventArgs e) => storage.CreateSMRProject();
    }
}
