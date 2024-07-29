using System;
using SNAMP.Utils;
using SNAMP.Models;
using System.Drawing;
using SNAMP.Properties;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class TableLayoutPanelSpecification : TableLayoutPanel
    {
        private readonly SMRDataSMRFile smrDataSMRFile;
        private Button buttonAdd;

        public TableLayoutPanelSpecification(SMRDataSMRFile smrDataSMRFile) : base()
        {
            this.smrDataSMRFile = smrDataSMRFile;

            RowCount = 0;
            ColumnCount = 8;
            Dock = DockStyle.Fill;
            AutoSize = true;

            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 15 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 25 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 2 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 20 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 8 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 10 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 5 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 15 });

            InitializeElements();
        }

        public void FormClosed()
        {
            buttonAdd.Click -= OnButtonAddClick;
            Controls.Remove(buttonAdd);

            foreach (Control control in Controls)
            {
                if (control is Button button)
                    button.Click -= OnButtonDeleteClick;
            }
        }

        private void InitializeElements()
        {
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 40 });
            RowCount++;

            Controls.Add(new MLabel("Наименование") { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            Controls.Add(new MLabel("Значение") { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 3, 0);

            Label labelMeasuring = new MLabel("Единица измерения") { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter};
            Controls.Add(labelMeasuring, 4, 0);
            SetColumnSpan(labelMeasuring, 2);

            smrDataSMRFile.DataSMR.SMRSpecifications.ForEach(smrSpecification =>
            {
                if (smrSpecification != null)
                {
                    RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
                    RowCount++;

                    AddRow(smrSpecification, RowCount - 1);
                }
            });

            Panel panelAdd = new Panel { Dock = DockStyle.Fill };

            buttonAdd = new MButton(new Size(30, 30), image: Resources.plus24);
            buttonAdd.Left = panelAdd.Width - buttonAdd.Width;
            buttonAdd.Top = (34 - buttonAdd.Height) / 2;

            buttonAdd.Click += OnButtonAddClick;
            panelAdd.Controls.Add(buttonAdd);

            RowCount++;
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 40 });
            Controls.Add(panelAdd, 1, RowCount - 1);
            SetColumnSpan(panelAdd, 5);
        }

        private void AddRow(SMRSpecification smrSpecification, int rowIndex)
        {
            TextBox textBoxName = new MTextBox(DockStyle.Fill);
            TextBox textBoxValue = new MTextBox(DockStyle.Fill);
            TextBox textBoxDecimalPrefixes = new MTextBox(DockStyle.Fill);
            TextBox textBoxUnit = new MTextBox(DockStyle.Fill);

            textBoxName.DataBindings.Add("Text", smrSpecification, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxValue.DataBindings.Add("Text", smrSpecification, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxDecimalPrefixes.DataBindings.Add("Text", smrSpecification, "DecimalPrefixes", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxUnit.DataBindings.Add("Text", smrSpecification, "Unit", false, DataSourceUpdateMode.OnPropertyChanged);

            Controls.Add(textBoxName, 1, rowIndex);
            Controls.Add(new MLabel("=") { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, 2, rowIndex);
            Controls.Add(textBoxValue, 3, rowIndex);
            Controls.Add(textBoxDecimalPrefixes, 4, rowIndex);
            Controls.Add(textBoxUnit, 5, rowIndex);

            Button buttonDelete = new MButton(new Size(30, 30), image: Resources.minus24) { Tag = smrSpecification };
            buttonDelete.Click += OnButtonDeleteClick;
            Controls.Add(buttonDelete, 6, rowIndex);
        }

        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            if (!(sender is Button buttonDelete) || !(buttonDelete.Tag is SMRSpecification smrSpecification))
                return;

            smrDataSMRFile.DataSMR.isSave = false;

            buttonDelete.Click -= OnButtonDeleteClick;
            smrDataSMRFile.DataSMR.SMRSpecifications.Remove(smrSpecification);
            int rowIndex = GetRow(buttonDelete);
            BuilderTableLayoutPanel.RemoveArbitraryRow(this, rowIndex);
            RowCount--;

            if (RowStyles.Count > RowCount)
                RowStyles.RemoveAt(rowIndex);

        }

        private void OnButtonAddClick(object sender, EventArgs e)
        {
            smrDataSMRFile.DataSMR.isSave = false;
            BuilderTableLayoutPanel.InsertArbitraryRow(this, RowStyles.Count - 1, new RowStyle { SizeType = SizeType.AutoSize });
            
            SMRSpecification smrSpecification = new SMRSpecification();
            smrDataSMRFile.DataSMR.SMRSpecifications.Add(smrSpecification);
            AddRow(smrSpecification, RowCount - 2);
        }
    }
}
