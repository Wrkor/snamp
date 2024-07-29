using System.Windows.Forms;

namespace SNAMP.Utils
{
    public static class DialogWindow
    {
        public static DialogResult MessageError(string text) => MessageBox.Show(text, "Ошибка");

        public static DialogResult MessageSuccess(string text) => MessageBox.Show(text, "Успешно");

        public static DialogResult MessageWarning(string text, MessageBoxButtons messageBoxButtons = MessageBoxButtons.YesNoCancel) => MessageBox.Show(text, "Вы уверены?", messageBoxButtons);
    }
}
