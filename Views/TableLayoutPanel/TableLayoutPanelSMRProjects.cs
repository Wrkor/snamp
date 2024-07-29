using System.IO;
using SNAMP.Utils;
using SNAMP.Models;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Views
{
    public class TableLayoutPanelSMRProjects : TableLayoutPanel
    {
        private readonly Storage storage;
        private readonly Dictionary<SMRProject, TableLayoutPanel> smrProjectsActive;

        public TableLayoutPanelSMRProjects(Storage storage) : base()
        {
            this.storage = storage;
            smrProjectsActive = new Dictionary<SMRProject, TableLayoutPanel>();

            BackColor = DataDefault.bg;
            Dock = DockStyle.Fill;
            AutoScroll = true;
            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            ColumnCount = 2;
            RowCount = 0;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));

            InitializeElements();
        }

        private void AddRow(SMRProject smrProject, int rowIndex)
        {
            MLabel colName = new MLabel(smrProject.Name) { TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0), Cursor = Cursors.Hand };
            MLabel colPath = new MLabel(smrProject.Path) { TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0), Cursor = Cursors.Hand };
            MLabel colDate = new MLabel(BuilderDateTime.DateTimeToString(smrProject.Date)) { TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0), Cursor = Cursors.Hand };
            MButton buttonDelete = new MButton("X", new Size(40, 40)) { BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(10, 15, 10, 10) };

            colName.Click += (sender, e) => OnRowBtnClick(smrProject);
            colPath.Click += (sender, e) => OnRowBtnClick(smrProject);
            colDate.Click += (sender, e) => OnRowBtnClick(smrProject);
            buttonDelete.Click += (sender, e) => OnRowBtnDeleteClick(smrProject);

            TableLayoutPanel tableLayoutPanelProject = new TableLayoutPanel() { Dock = DockStyle.Fill, Height = 80, ColumnCount = 3, RowCount = 1, Cursor = Cursors.Hand, Tag = smrProject };
            tableLayoutPanelProject.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            tableLayoutPanelProject.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tableLayoutPanelProject.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            tableLayoutPanelProject.RowStyles.Add(new RowStyle(SizeType.Percent, Height = 100));

            tableLayoutPanelProject.Controls.Add(colName, 0, 0);
            tableLayoutPanelProject.Controls.Add(colPath, 1, 0);
            tableLayoutPanelProject.Controls.Add(colDate, 2, 0);

            tableLayoutPanelProject.MouseEnter += (sender, e) => OnRowBtnMouseEnter(tableLayoutPanelProject);
            colName.MouseEnter += (sender, e) => OnRowBtnMouseEnter(tableLayoutPanelProject);
            colPath.MouseEnter += (sender, e) => OnRowBtnMouseEnter(tableLayoutPanelProject);
            colDate.MouseEnter += (sender, e) => OnRowBtnMouseEnter(tableLayoutPanelProject);

            tableLayoutPanelProject.MouseLeave += (sender, e) => OnRowBtnMouseLeave(tableLayoutPanelProject);
            colName.MouseLeave += (sender, e) => OnRowBtnMouseLeave(tableLayoutPanelProject);
            colPath.MouseLeave += (sender, e) => OnRowBtnMouseLeave(tableLayoutPanelProject);
            colDate.MouseLeave += (sender, e) => OnRowBtnMouseLeave(tableLayoutPanelProject);

            tableLayoutPanelProject.Click += (sender, e) => OnRowBtnClick(smrProject);

            Controls.Add(tableLayoutPanelProject, 0, rowIndex);
            Controls.Add(buttonDelete, 1, rowIndex);
            smrProjectsActive.Add(smrProject, tableLayoutPanelProject);
        }

        private void InitializeList(List<SMRProject> smrProjects)
        {
            Visible = false;
            Controls.Clear();
            RowStyles.Clear();
            RowCount = 1;
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            smrProjectsActive.Clear();

            smrProjects.ForEach(smrProject =>
            {
                if (smrProject != null)
                {
                    RowStyles.Insert(RowStyles.Count - 1, new RowStyle(SizeType.Absolute, 80));
                    RowCount++;

                    AddRow(smrProject, RowCount - 2);
                }
            });

            UpdateScroll();
            Visible = true;
        }

        private void InitializeElements()
        {
            storage.AddSMRProjectHandler += OnAddSMRProject;
            storage.DeleteSMRProjectHandler += OnDeleteSMRProject;
            storage.UpdateSMRProjectsHandler += OnUpdateSMRProjects;
            storage.smrHubForm.FormClosed += OnFormClosed;
        }

        private void UpdateScroll()
        {
            AutoScroll = false;
            HorizontalScroll.Maximum = 0;
            AutoScroll = true;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is TableLayoutPanel tableLayoutPanel)
                {
                    foreach (Control contolColumn in tableLayoutPanel.Controls)
                    {
                        if (contolColumn is MLabel mLabel)
                        {
                            mLabel.Click -= (sender2, e2) => OnRowBtnClick(tableLayoutPanel.Tag as SMRProject);
                            mLabel.MouseEnter -= (sender2, e2) => OnRowBtnMouseEnter(tableLayoutPanel);
                            mLabel.MouseLeave -= (sender2, e2) => OnRowBtnMouseLeave(tableLayoutPanel);
                        }

                        else if (contolColumn is MButton mButton)
                        {
                            mButton.Click -= (sender2, e2) => OnRowBtnDeleteClick(tableLayoutPanel.Tag as SMRProject);
                        }
                    }

                    tableLayoutPanel.Click += (sender2, e2) => OnRowBtnClick(tableLayoutPanel.Tag as SMRProject);
                    tableLayoutPanel.MouseEnter += (sender2, e2) => OnRowBtnMouseEnter(tableLayoutPanel);
                    tableLayoutPanel.MouseLeave += (sender2, e2) => OnRowBtnMouseLeave(tableLayoutPanel);
                }
            }

            storage.AddSMRProjectHandler -= OnAddSMRProject;
            storage.DeleteSMRProjectHandler -= OnDeleteSMRProject;
            storage.UpdateSMRProjectsHandler -= OnUpdateSMRProjects;

            storage.smrHubForm.FormClosed -= OnFormClosed;
        }

        private void OnAddSMRProject(SMRProject smrProject)
        {
            BuilderTableLayoutPanel.InsertArbitraryRow(this, 0, new RowStyle(SizeType.Absolute, 80));
            AddRow(smrProject, 0);
            UpdateScroll();
        }

        private void OnDeleteSMRProject(SMRProject smrProject)
        {
            foreach (Control contolColumn in smrProjectsActive[smrProject].Controls)
            {
                if (contolColumn is MLabel mLabel)
                {
                    mLabel.Click -= (sender2, e2) => OnRowBtnClick(smrProject);
                    mLabel.MouseEnter -= (sender2, e2) => OnRowBtnMouseEnter(smrProjectsActive[smrProject]);
                    mLabel.MouseLeave -= (sender2, e2) => OnRowBtnMouseLeave(smrProjectsActive[smrProject]);
                }

                else if (contolColumn is MButton mButton)
                {
                    mButton.Click -= (sender2, e2) => OnRowBtnDeleteClick(smrProject);
                }
            }

            smrProjectsActive[smrProject].Click += (sender2, e2) => OnRowBtnClick(smrProject);
            smrProjectsActive[smrProject].MouseEnter += (sender2, e2) => OnRowBtnMouseEnter(smrProjectsActive[smrProject]);
            smrProjectsActive[smrProject].MouseLeave += (sender2, e2) => OnRowBtnMouseLeave(smrProjectsActive[smrProject]);

            BuilderTableLayoutPanel.RemoveArbitraryRow(this, GetRow(smrProjectsActive[smrProject]));
            RowCount--;

            if (RowStyles.Count > RowCount)
                RowStyles.RemoveAt(RowStyles.Count - 2);

            smrProjectsActive.Remove(smrProject);
            UpdateScroll();
        }

        private void OnRowBtnClick(SMRProject smrProject)
        {
            if (!File.Exists(smrProject.GetFullPath()))
            {
                DialogWindow.MessageError($"SMR проект - \"{smrProject.Name}\" по пути {smrProject.GetFullPath()} не был найден");

                storage.DeleteSMRProject(smrProject);
                return;
            }
                
            storage.OpenSMRProject(smrProject);
        }

        private void OnRowBtnMouseEnter(TableLayoutPanel projectTablePanel) => projectTablePanel.BackColor = DataDefault.blackHover;

        private void OnRowBtnMouseLeave(TableLayoutPanel projectTablePanel) => projectTablePanel.BackColor = Color.Transparent;

        private void OnRowBtnDeleteClick(SMRProject smrProject) => storage.DeleteSMRProject(smrProject);

        private void OnUpdateSMRProjects(List<SMRProject> smrProjects) => InitializeList(smrProjects);
    }
}
