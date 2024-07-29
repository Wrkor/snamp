using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class MLabel : Label
    {
        public MLabel(string text, Font font = null) : base()
        {
            Text = text;
            ForeColor = DataDefault.textWhite;
            Font = font ?? DataDefault.textFont12;
        }
    }
}
