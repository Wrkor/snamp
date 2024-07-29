using System.Windows.Forms;

namespace SNAMP.Views
{
    public class MTextBox : TextBox
    {
        public MTextBox(DockStyle dock = DockStyle.None, int height = 20, int max = 300, bool isMultiline = false, ScrollBars scrollBars = ScrollBars.None, bool isReadOnly = false) : base()
        {
            ForeColor = DataDefault.textWhite;
            BackColor = DataDefault.blackHover;
            BorderStyle = BorderStyle.FixedSingle;
            Font = DataDefault.textFont12;
            Dock = dock;
            Height = height;
            ReadOnly = isReadOnly;
            Multiline = isMultiline;
            ScrollBars = scrollBars;
            MaxLength = max;
        }
    }
}
