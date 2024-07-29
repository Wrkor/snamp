using SNAMP.Properties;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class ContextMenuStripListView : ContextMenuStrip
    {
        public ToolStripMenuItem ToolStripMenuItemOpenExplorer { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemUpdate { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImport { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImportDirectory { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImportSMRFile { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImportFile { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemPaste { get; private set; }

        public ContextMenuStripListView() : base()
        {
            ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItemOpenExplorer = new ToolStripMenuItem() { Text = "Открыть проводник", Image = Resources.Dir24 };
            ToolStripMenuItemUpdate = new ToolStripMenuItem() { Text = "Обновить", Image = Resources.Update24 };

            ToolStripMenuItemImport = new ToolStripMenuItem() { Text = "Добавить", Image = Resources.Create24 };
            ToolStripMenuItemImportDirectory = new ToolStripMenuItem() { Text = "Папку", Image = Resources.Dir64 };
            ToolStripMenuItemImportSMRFile = new ToolStripMenuItem() { Text = "SMR файл", Image = Resources.SMR64 };
            ToolStripMenuItemImportFile = new ToolStripMenuItem() { Text = "Файл...", Image = Resources.File64 };
            ToolStripMenuItemPaste = new ToolStripMenuItem() { Text = "Вставить", Image = Resources.Paste24, ShortcutKeyDisplayString = "Ctrl + V" };

            ToolStripMenuItemImport.DropDownItems.AddRange(new ToolStripItem[] {
                ToolStripMenuItemImportDirectory,
                ToolStripMenuItemImportSMRFile,
                ToolStripMenuItemImportFile
            });

            ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                ToolStripMenuItemOpenExplorer,
                ToolStripMenuItemUpdate,
                ToolStripMenuItemImport,
                new ToolStripSeparator(),
                ToolStripMenuItemPaste,
            });
        }
    }
}
