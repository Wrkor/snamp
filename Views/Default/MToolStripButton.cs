using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class MToolStripButton : ToolStripButton
    {
        public MToolStripButton(string text, Image image )
        {
            Size = new Size(30, 30);
            Image = image;
            BackColor = Color.Transparent;
            ForeColor = Color.Transparent;
            ImageScaling = ToolStripItemImageScaling.None;
            ImageTransparentColor = Color.Magenta;
            DisplayStyle = ToolStripItemDisplayStyle.Image;
            Padding = new Padding(1, 0, 1, 0);
            Margin = new Padding(0, 5, 5, 5);
            Text = text;
            Alignment = ToolStripItemAlignment.Right;
        }
    }
}
