using System.IO;
using SNAMP.Utils;
using System.Drawing;
using System.Windows.Forms;
using Gnostice.Controls.WinForms;

namespace SNAMP.Views
{
    public class TabPageFile : TabPage
    {
        public DocumentViewer DocumentViewer { get; private set; }
        public SMRDataFile SMRDataFile { get; private set; }
        public MTabControl MTabControl { get; private set; }

        public TabPageFile(SMRDataFile smrDataFile, MTabControl mTabControl) : base()
        {
            SMRDataFile = smrDataFile;
            MTabControl = mTabControl;

            ImageIndex = smrDataFile.IndexImg;
            BackColor = DataDefault.bg;
            Dock = DockStyle.Fill;
            Size = new Size(50, 1);
            Text = smrDataFile.Name;

            InitializeElements();
        }

        public bool GoToPage(int page)
        {
            if (DocumentViewer == null)
                return false;

            if (DocumentViewer.PageCount < page)
                return false;

            DocumentViewer.GoToPage(page);
            return true;
        }

        public void InitializeElements()
        {
            if (SMRDataFile == null)
            {
                DialogWindow.MessageError("Ошибка открытия файла");
                return;
            }

            if (SMRDataFile.TabPageFile == this)
                return;

            if (SMRDataFile is SMRDataSMRFile || !File.Exists(SMRDataFile.FullPathToSMRData) || SMRDataFile.FileState == DataDefault.FileState.None || SMRDataFile.FileState == DataDefault.FileState.Video || SMRDataFile.FileState == DataDefault.FileState.Archive || SMRDataFile.FileState == DataDefault.FileState.Dwg)
                return;

            DocumentViewer = new DocumentViewer() { Dock = DockStyle.Fill };
            DocumentViewer.LoadDocument(SMRDataFile.FullPathToSMRData);
            DocumentViewer.NavigationPane.VisibilityState = Gnostice.Core.Viewer.VisibilityState.Collapsed;
            DocumentViewer.NavigationPane.Visibility = Gnostice.Core.Viewer.Visibility.Never;

            if (DocumentViewer == null)
                return;

            SMRDataFile.TabPageFile = this;
            Controls.Add(DocumentViewer);
        }

        public void CloseSMRDataFile()
        {
            if (SMRDataFile == null)
            {
                DialogWindow.MessageError("Ошибка закрытия файла");
                return;
            }

            SMRDataFile.TabPageFile = null;
            DocumentViewer.CloseDocument();
            Controls.Remove(DocumentViewer);
        }
    }
}
