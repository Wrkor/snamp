using System;
using System.IO;
using SNAMP.Models;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SNAMP
{
    public partial class NewProjectForm : Form
    {
        public SMRNewProject data;

        private const string PATH = @"C:\";
        private const string NAME = "Имя нового проекта";

        private readonly Color textColor = DataDefault.textWhite;
        private readonly Color textErrorColor = DataDefault.textError;
        private readonly Color textDefault = DataDefault.textDefault;

        public NewProjectForm()
        {
            InitializeComponent();
            data = new SMRNewProject();

            SetNameNewProjectInputByDefault();
            SetPathNewProjectInputByDefault();

            nameNewProjectInput.Click += OnNameNewProjectInputClick;
            nameNewProjectInput.Leave += OnNameNewProjectInputLeave;
            nameNewProjectInput.Validating += OnNameNewProjectInputValidating;

            pathNewProjectInput.Click += OnPathNewProjectInputClick;
            pathNewProjectInput.Leave += OnPathNewProjectInputLeave;
            pathNewProjectInput.Validating += OnPathNewProjectInputValidating;
            pathNewProjectInput.TextChanged += OnPathNewProjectInputTextChanged;

            pathBtn.Click += OnPathBtnClick;
            createProjectBtn.Click += OnCreateProjectBtnClick;
            cancelNewProjectBtn.Click += OnCancelNewProjectBtnClick;

            FormClosed += OnFormClosed;
        }

        private void OnNameNewProjectInputClick(object sender, EventArgs e)
        {
            if (nameNewProjectInput.Text == NAME)
            {
                nameNewProjectInput.Text = "";
                nameNewProjectInput.ForeColor = textColor;
            }
        }

        private void OnPathNewProjectInputClick(object sender, EventArgs e)
        {
            if (pathNewProjectInput.Text == PATH)
            {
                pathNewProjectInput.Text = "";
                pathNewProjectInput.ForeColor = textColor;
            }
        }

        private void OnNameNewProjectInputLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameNewProjectInput.Text))
                SetNameNewProjectInputByDefault();
        }

        private void OnPathNewProjectInputLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pathNewProjectInput.Text))
                SetPathNewProjectInputByDefault();
        }

        private void SetNameNewProjectInputByDefault()
        {
            nameNewProjectInput.Text = NAME;
            nameNewProjectInput.ForeColor = textDefault;
        }

        private void SetPathNewProjectInputByDefault()
        {
            pathNewProjectInput.Text = PATH;
            pathNewProjectInput.ForeColor = textDefault;
        }

        private void OnCreateProjectBtnClick(object sender, EventArgs e)
        {
            OnNameNewProjectInputValidating(nameNewProjectInput, new CancelEventArgs());
            OnPathNewProjectInputValidating(pathNewProjectInput, new CancelEventArgs());

            if (TextValidation() || PathValidation())
                return;

            data.name = nameNewProjectInput.Text;
            data.path = pathNewProjectInput.Text;
            data.isCreateFolder = newProjectCheckbox.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnPathBtnClick(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                pathNewProjectInput.Text = folderDialog.SelectedPath;
                OnPathNewProjectInputValidating(pathNewProjectInput, new CancelEventArgs());
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            nameNewProjectInput.Click -= OnNameNewProjectInputClick;
            nameNewProjectInput.Leave -= OnNameNewProjectInputLeave;
            nameNewProjectInput.Validating -= OnNameNewProjectInputValidating;

            pathNewProjectInput.Click -= OnPathNewProjectInputClick;
            pathNewProjectInput.Leave -= OnPathNewProjectInputLeave;
            pathNewProjectInput.Validating -= OnPathNewProjectInputValidating;
            pathNewProjectInput.TextChanged -= OnPathNewProjectInputTextChanged;

            pathBtn.Click -= OnPathBtnClick;
            createProjectBtn.Click -= OnCreateProjectBtnClick;
            cancelNewProjectBtn.Click -= OnCancelNewProjectBtnClick;

            FormClosed -= OnFormClosed;
        }

        private bool TextValidation() => string.IsNullOrWhiteSpace(nameNewProjectInput.Text) || nameNewProjectInput.Text.Equals(NAME) || nameNewProjectInput.Text.Length > 255 || Regex.Match(nameNewProjectInput.Text, DataDefault.REG_NAME_DIRECTORY).Success;

        private bool PathValidation() => string.IsNullOrWhiteSpace(pathNewProjectInput.Text) || pathNewProjectInput.Text.Equals(PATH) || !Directory.Exists(pathNewProjectInput.Text);

        private void OnCancelNewProjectBtnClick(object sender, EventArgs e) => Close();

        private void OnPathNewProjectInputTextChanged(object sender, EventArgs e) => OnPathNewProjectInputValidating(this, new CancelEventArgs());

        private void OnNameNewProjectInputValidating(object sender, CancelEventArgs e) => nameNewProjectInput.ForeColor = TextValidation() ? textErrorColor : textColor;

        private void OnPathNewProjectInputValidating(object sender, CancelEventArgs e) => pathNewProjectInput.ForeColor = PathValidation() ? textErrorColor : textColor;
    }
}
