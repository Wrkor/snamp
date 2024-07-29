using System.IO;

namespace SNAMP.Utils
{
    public static class BuilderDocument
    {
        public static bool CheckOnImage(FileInfo fileInfo) => fileInfo.Exists && !string.IsNullOrEmpty(fileInfo.Extension) && (fileInfo.Extension == DataDefault.PNG_EXT || fileInfo.Extension == DataDefault.JPG_EXT || fileInfo.Extension == DataDefault.JPEG_EXT || fileInfo.Extension == DataDefault.BMP_EXT || fileInfo.Extension == DataDefault.TIFF_EXT);
        
        public static bool CheckOnOpenFileState(DataDefault.FileState fileState) => fileState == DataDefault.FileState.Excel || fileState == DataDefault.FileState.Document || fileState == DataDefault.FileState.Pdf || fileState == DataDefault.FileState.Picture;
    }
}
