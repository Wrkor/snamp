using System.Drawing;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class PanelButton : Panel
    {
        public Button ButtonSave { get; private set; }
        public Button ButtonCancel { get; private set; }

        public PanelButton() : base() 
        { 
            Dock = DockStyle.Fill;

            InitializeElements();
            Controls.AddRange(new Control[] { ButtonSave, ButtonCancel });
        }

        private void InitializeElements()
        {
            ButtonSave = new MButton("Сохранить", new Size(145, 37), isActive: true);
            ButtonSave.Location = new Point(Width - ButtonSave.Width - 10, 10);

            ButtonCancel = new MButton("Отменить", new Size(145, 37), isActive: false);
            ButtonCancel.Location = new Point(Width - ButtonSave.Width - ButtonCancel.Width - 20, 10);
        }
    }
}
