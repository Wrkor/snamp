using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace SNAMP.Views
{
    public static class IconList
    {
        public static ImageList IconsList16 { 
            get {
                if (_iconsList16 == null)
                {
                    ComponentResourceManager resources = new ComponentResourceManager(typeof(SMRForm));

                    _iconsList16 = new ImageList()
                    {
                        ImageStream = ((ImageListStreamer)(resources.GetObject("iconList16.ImageStream"))),
                        TransparentColor = Color.Transparent,
                    };

                    _iconsList16.Images.SetKeyName(0, "Dir16.png");
                    _iconsList16.Images.SetKeyName(1, "SMR16.png");
                    _iconsList16.Images.SetKeyName(2, "File16.png");
                    _iconsList16.Images.SetKeyName(3, "File-dwg16.png");
                    _iconsList16.Images.SetKeyName(4, "File-pdf16.png");
                    _iconsList16.Images.SetKeyName(5, "File-doc16.png");
                    _iconsList16.Images.SetKeyName(6, "File-xls16.png");
                    _iconsList16.Images.SetKeyName(7, "File-jpg16.png");
                    _iconsList16.Images.SetKeyName(8, "File-zip16.png");
                    _iconsList16.Images.SetKeyName(9, "File-mov16.png");
                }
                return _iconsList16;
            }
            private set => _iconsList16 = value; 
        }
        public static ImageList IconsList64
        {
            get
            {
                if (_iconsList64 == null)
                {
                    ComponentResourceManager resources = new ComponentResourceManager(typeof(SMRForm));

                    _iconsList64 = new ImageList()
                    {
                        ImageStream = ((ImageListStreamer)(resources.GetObject("iconList64.ImageStream"))),
                        TransparentColor = Color.Transparent,
                    };
                    _iconsList64.Images.SetKeyName(0, "Dir64.png");
                    _iconsList64.Images.SetKeyName(1, "SMR64.png");
                    _iconsList64.Images.SetKeyName(2, "File64.png");
                    _iconsList64.Images.SetKeyName(3, "File-dwg64.png");
                    _iconsList64.Images.SetKeyName(4, "File-pdf64.png");
                    _iconsList64.Images.SetKeyName(5, "File-doc64.png");
                    _iconsList64.Images.SetKeyName(6, "File-xls64.png");
                    _iconsList64.Images.SetKeyName(7, "File-jpg64.png");
                    _iconsList64.Images.SetKeyName(8, "File-zip64.png");
                    _iconsList64.Images.SetKeyName(9, "File-mov64.png");
                }
                return _iconsList64;
            }
            private set => _iconsList64 = value;
        }

        private static ImageList _iconsList16;
        private static ImageList _iconsList64;
    }
}
