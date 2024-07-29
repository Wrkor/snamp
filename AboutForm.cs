using System.Diagnostics;
using System.Windows.Forms;

namespace SNAMP
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void OnLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(new ProcessStartInfo(DataDefault.URL_ENGINEERING_CENTER));
    }
}
