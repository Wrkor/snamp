using System;
using System.IO;
using SNAMP.Utils;
using System.Drawing;
using SNAMP.Properties;
using System.Windows.Forms;
using System.ComponentModel;

namespace SNAMP.Views
{
    public class PanelFieldInputFile : Panel
    {
        public Action<string> ChangedPathToImageHandler;
        public Label LabelField { get; private set; }
        public Button ButtonField { get; private set; }
        public TextBox TextBoxField { get; private set; }

        private const string CHOICE_IMAGE = "Выберите изображение";
        private string pathRoot;

        public PanelFieldInputFile(string label, string pathRoot = "") : base()
        {
            this.pathRoot = pathRoot;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.FixedSingle;

            LabelField = new Label()
            {
                Text = label,
                Font = DataDefault.textFont12,
                ForeColor = DataDefault.textWhite,
                Dock = DockStyle.Top,
                Height = 20,
            };

            ButtonField = new MButton(new Size(30, 30), image: Resources.Dir24)
            {
                Dock = DockStyle.Fill,
                ImageAlign = ContentAlignment.MiddleCenter,
            };

            ButtonField.Click += OnButtonFieldClick;

            TextBoxField = new MTextBox(DockStyle.Fill);

            SetTextBoxFieldDefault();
            TextBoxField.Validating += OnTextBoxFieldValidating;
            TextBoxField.Click += OnTextBoxFieldClick;
            TextBoxField.Leave += OnTextBoxFieldLeave;
            TextBoxField.TextChanged += OnTextBoxFieldTextChanged;
            
            Controls.Add(LabelField);
            InitializeElements();
        }

        private void InitializeElements()
        {
            TableLayoutPanel panelFile = new TableLayoutPanel()
            {
                Dock = DockStyle.Bottom,
                Height = 34,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0),
            };

            panelFile.ColumnStyles.Clear();
            panelFile.ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Absolute, Width = 34 });
            panelFile.ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.AutoSize });

            panelFile.RowStyles.Clear();
            panelFile.RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 30 });

            panelFile.Controls.Add(ButtonField, 0, 0);
            panelFile.Controls.Add(TextBoxField, 1, 0);

            Controls.Add(panelFile);
        }

        public void SetTextBoxFieldData(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                SetTextBoxFieldDefault();
            }

            else
            {
                TextBoxField.ForeColor = DataDefault.textWhite;
                TextBoxField.Text = text;
                OnTextBoxFieldValidating(TextBoxField, new CancelEventArgs());
            }
        }

        public void FormClosed()
        {
            TextBoxField.Validating -= OnTextBoxFieldValidating;
            TextBoxField.Click -= OnTextBoxFieldClick;
            TextBoxField.Leave -= OnTextBoxFieldLeave;
            TextBoxField.TextChanged -= OnTextBoxFieldTextChanged;
        }

        private void SetTextBoxFieldDefault()
        {
            TextBoxField.Text = CHOICE_IMAGE;
            TextBoxField.ForeColor = DataDefault.textDefault;
        }

        private void OnButtonFieldClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = DataDefault.IMAGE_FILTER,
                InitialDirectory = pathRoot,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK && File.Exists(openFileDialog.FileName))
            {
                if (!openFileDialog.FileName.Contains(pathRoot))
                {
                    DialogWindow.MessageError("Изображение должно находиться внутри проекта: " + pathRoot);
                    return;
                }

                SetTextBoxFieldData(openFileDialog.FileName);
                ChangedPathToImageHandler?.Invoke(openFileDialog.FileName);
            }
        }

        private void OnTextBoxFieldClick(object sender, EventArgs e)
        {
            if (TextBoxField.Text == CHOICE_IMAGE)
            {
                TextBoxField.Text = string.Empty;
                TextBoxField.ForeColor = DataDefault.textWhite;
            }
        }

        private void OnTextBoxFieldLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxField.Text) || TextBoxField.Text.Equals(CHOICE_IMAGE))
                SetTextBoxFieldDefault();

            ChangedPathToImageHandler?.Invoke(TextBoxField.Text);
        }

        private void OnTextBoxFieldValidating(object sender, CancelEventArgs e)
        {
            if (TextBoxField.Text != CHOICE_IMAGE)
                TextBoxField.ForeColor = PathValidation() ? DataDefault.textError : DataDefault.textWhite;
            else
                SetTextBoxFieldDefault();
        }

        private bool PathValidation() => string.IsNullOrEmpty(TextBoxField.Text) || !File.Exists(TextBoxField.Text) || !TextBoxField.Text.Contains(pathRoot);
        
        private void OnTextBoxFieldTextChanged(object sender, EventArgs e) => OnTextBoxFieldValidating(TextBoxField, new CancelEventArgs());
    }
}
