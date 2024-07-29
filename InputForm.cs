using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace SNAMP
{
    public partial class InputForm : Form
    {
        public string Input { get; private set; }

        private readonly int minLength;

        private readonly Color textColor = DataDefault.textWhite;
        private readonly Color textErrorColor = DataDefault.textError;
        private readonly Color textDefault= DataDefault.textDefault;

        public InputForm(string Name)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Text = $"Объект: {Name}";

            createLinkBtn.Click += OnCreateLinkBtnClick;
            cancelLinkBtn.Click += OnCancelLinkBtnClick;
            countInput.Click += OnCountInputClick;
            countInput.Leave += OnCountInputLeave;
            countInput.TextChanged += (sender, e) => OnCountInputValidating(countInput, new CancelEventArgs());
            countInput.Validating += OnCountInputValidating;
            FormClosed += OnFormClosed;
        }

        private void SetNameNewProjectInputByDefault()
        {
            countInput.Text = "0";
            countInput.ForeColor = textDefault;
        }

        private void OnCreateLinkBtnClick(object sender, EventArgs e)
        {
            if (!TextValidation())
            {
                Input = countInput.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void OnCountInputClick(object sender, EventArgs e)
        {
            if (countInput.Text == "0")
            {
                countInput.Text = "";
                countInput.ForeColor = Color.White;
            }
        }

        private void OnCountInputLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(countInput.Text))
                SetNameNewProjectInputByDefault();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            createLinkBtn.Click -= OnCreateLinkBtnClick;
            cancelLinkBtn.Click -= OnCancelLinkBtnClick;
            countInput.Click -= OnCountInputClick;
            countInput.Leave -= OnCountInputLeave;
            countInput.TextChanged -= (sender2, e2) => OnCountInputValidating(countInput, new CancelEventArgs());
            countInput.Validating -= OnCountInputValidating;
            FormClosed -= OnFormClosed;
        }

        private void OnCancelLinkBtnClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void OnCountInputValidating(object sender, CancelEventArgs e) => countInput.ForeColor = TextValidation() ? textErrorColor : textColor;

        private bool TextValidation() => string.IsNullOrWhiteSpace(countInput.Text) || countInput.Text.Length < minLength || !int.TryParse(countInput.Text, out _);
    }
}
