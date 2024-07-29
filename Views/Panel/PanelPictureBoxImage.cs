using System.IO;
using SNAMP.Utils;
using System.Drawing;
using SNAMP.Properties;
using System.Windows.Forms;
using System;

namespace SNAMP.Views
{
    public class PanelPictureBoxImage : Panel
    {
        public Action<string> OnChangedPath;

        public string Path { get; private set; }

        public PictureBox PictureBoxImage { get; private set; }

        public PanelPictureBoxImage() : base()
        {
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.FixedSingle;

            PictureBoxImage = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = Resources.SMR64,
            };

            Controls.Add(PictureBoxImage);
        }

        public void SetPictureBoxImage(string path)
        {
            if (path == Path || string.IsNullOrWhiteSpace(path))
                return;

            if (File.Exists(path) && BuilderDocument.CheckOnImage(new FileInfo(path)))
            {
                PictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
                PictureBoxImage.Image = new Bitmap(path);
                Path = path;
                OnChangedPath?.Invoke(Path);
            }

            else
            {
                PictureBoxImage.SizeMode = PictureBoxSizeMode.CenterImage;
                PictureBoxImage.Image = Resources.SMR64;
                Path = string.Empty;
                OnChangedPath?.Invoke(Path);
            }
        }
    }
}
