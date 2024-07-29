using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class MButton : Button
    {
        public MButton(Size size, AnchorStyles anchor = AnchorStyles.Top | AnchorStyles.Right, Bitmap image = null, Point? location = null) : base()
        {
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Transparent;
            Anchor = anchor;
            Location = location ?? new Point(0, 0);
            Size = size;
            Image = image;
            Cursor = Cursors.Hand;

            FlatAppearance.MouseOverBackColor = DataDefault.blackHover;
            FlatAppearance.MouseDownBackColor = DataDefault.blackClick;
            FlatAppearance.BorderSize = 0;
        }

        public MButton(string text, Size size, AnchorStyles anchor = AnchorStyles.Top | AnchorStyles.Right, bool isActive = false, Point? location = null) : base()
        {
            FlatStyle = FlatStyle.Flat;
            Text = text;
            BackColor = isActive ? DataDefault.blue : DataDefault.black;
            ForeColor = DataDefault.textWhite;
            Font = DataDefault.textFont12;
            Anchor = anchor;
            Location = location ?? new Point(0, 0);
            Size = size;
            Cursor = Cursors.Hand;

            FlatAppearance.MouseOverBackColor = DataDefault.blackHover;
            FlatAppearance.MouseDownBackColor = DataDefault.blackClick;
            FlatAppearance.BorderSize = 0;
        }
    }
}
