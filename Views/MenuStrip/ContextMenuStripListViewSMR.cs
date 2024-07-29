using SNAMP.Properties;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class ContextMenuStripListViewSMR : ContextMenuStrip
    {
        public ToolStripMenuItem ToolStripMenuItemOpen { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemWatch { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemOpenExplorer { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemOpenFile { get; private set; }

        public ContextMenuStripListViewSMR() : base()
        {
            ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItemOpen = new ToolStripMenuItem() { Text = "Открыть", Image = Resources.Open24 };
            ToolStripMenuItemWatch = new ToolStripMenuItem() { Text = "Перейти", Image = Resources.watch24 };
            ToolStripMenuItemOpenExplorer = new ToolStripMenuItem() { Text = "Открыть проводник", Image = Resources.Dir24 };
            ToolStripMenuItemOpenFile = new ToolStripMenuItem() { Text = "Открыть программу", Image = Resources.File24 };

            ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                ToolStripMenuItemOpen,
                ToolStripMenuItemWatch,
                new ToolStripSeparator(),
                ToolStripMenuItemOpenExplorer,
                ToolStripMenuItemOpenFile,
            });
        }
    }
}
