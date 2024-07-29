using System.Windows.Forms;

namespace SNAMP.Utils
{
    public static class BuilderTableLayoutPanel
    {
        public static void InsertArbitraryRow(TableLayoutPanel panel, int rowIndex, RowStyle rowStyle)
        {
            panel.RowStyles.Insert(rowIndex, rowStyle);
            panel.RowCount++;

            foreach (Control control in panel.Controls)
            {
                if (panel.GetRow(control) >= rowIndex)
                    panel.SetRow(control, panel.GetRow(control) + 1);
            }
        }

        public static void RemoveArbitraryRow(TableLayoutPanel panel, int rowIndex)
        {
            if (rowIndex == -1 || rowIndex >= panel.RowCount)
                return;

            for (int i = 0; i < panel.ColumnCount; i++)
            {
                Control control = panel.GetControlFromPosition(i, rowIndex);
                panel.Controls.Remove(control);
            }

            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    Control control = panel.GetControlFromPosition(j, i);

                    if (control != null)
                        panel.SetRow(control, i - 1);
                }
            }
        }
    }
}
