using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class MLinkLabel : LinkLabel
    {
        public MLinkLabel(string text, DockStyle dock = DockStyle.Fill, ContentAlignment textAlign = ContentAlignment.MiddleLeft, Font font = null) : base()
        {
            Text = text;
            Dock = dock;
            TextAlign = textAlign;
            Font = font ?? DataDefault.textFont12;
            ForeColor = DataDefault.textWhite;
            LinkColor = DataDefault.textWhite;
            ActiveLinkColor = DataDefault.textDefault;
        }
    }
}
