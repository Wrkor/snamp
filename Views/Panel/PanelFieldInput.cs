using System.Windows.Forms;

namespace SNAMP.Views
{
    public class PanelFieldInput : Panel
    {
        public Label LabelField { get; private set; }
        public TextBox TextBoxField { get; private set; }

        public PanelFieldInput(string label, int height, int max = 300, bool isReadOnly = false, bool isMultiline = false, ScrollBars scrollBars = ScrollBars.None) : base()
        {
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

            TextBoxField = new MTextBox(DockStyle.Bottom, height, max, isMultiline, scrollBars, isReadOnly);

            Controls.Add(LabelField);
            Controls.Add(TextBoxField);
        }

        public void SetTextBoxFieldData(string text)
        {
            TextBoxField.ForeColor = DataDefault.textWhite;
            TextBoxField.Text = text;
        }
    }
}
