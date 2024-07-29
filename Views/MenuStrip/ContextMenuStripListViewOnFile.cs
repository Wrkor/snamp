using SNAMP.Properties;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class ContextMenuStripListViewOnFile : ContextMenuStrip
    {
        public ToolStripMenuItem ToolStripMenuItemOpen { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemOpenExplorer { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemOpenFile { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemRename { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemCut { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemCopy { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemDelete { get; private set; }

        public ContextMenuStripListViewOnFile() : base()
        {
            ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItemOpen = new ToolStripMenuItem() { Text = "Открыть", Image = Resources.Open24, ShortcutKeyDisplayString = "Enter" };
            ToolStripMenuItemOpenExplorer = new ToolStripMenuItem() { Text = "Открыть проводник", Image = Resources.Dir24 };
            ToolStripMenuItemOpenFile = new ToolStripMenuItem() { Text = "Открыть программу", Image = Resources.File24 };
            ToolStripMenuItemRename = new ToolStripMenuItem() { Text = "Переименовать", Image = Resources.rename24, ShortcutKeyDisplayString = "F2" };
            ToolStripMenuItemCut = new ToolStripMenuItem() { Text = "Вырезать", Image = Resources.Cut24, ShortcutKeyDisplayString = "Ctrl + X" };
            ToolStripMenuItemCopy = new ToolStripMenuItem() { Text = "Копировать", Image = Resources.Copy24, ShortcutKeyDisplayString = "Ctrl + C" };
            ToolStripMenuItemDelete = new ToolStripMenuItem() { Text = "Удалить", Image = Resources.Delete24, ShortcutKeyDisplayString = "Delete" };

            ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                ToolStripMenuItemOpen,
                ToolStripMenuItemOpenExplorer,
                ToolStripMenuItemOpenFile,
                ToolStripMenuItemRename,
                new ToolStripSeparator(),
                ToolStripMenuItemCut,
                ToolStripMenuItemCopy,
                new ToolStripSeparator(),
                ToolStripMenuItemDelete
            });

        }
    }
}
