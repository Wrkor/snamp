using System.Drawing;
using System.Collections.Generic;

namespace SNAMP
{
    public static class DataDefault
    {
        public enum SMRTool { None, Copy, Cut, Paste, Delete, Update };
        public enum SMRState { Root, Directory, SMRFile, File };
        public enum FileState { None, Document, Pdf, Excel, Dwg, Video, Picture, Archive };

        public const int ARH_VIEW_PANELS_SD = 300;
        public const int VIEW_PROPERTY_PANELS_SD = 1300;
        public const int VIEW_MANAGER_PANELS_SD = 0;

        public const string URL_HELP = @"https://disk.yandex.ru/d/4lCCxoFnedBZsQ";
        public const string URL_ENGINEERING_CENTER = @"https://www.istu.edu/deyatelnost/nauka/sno/nst_avtomatika";

        public const string SMR_EXT = ".smr";
        public const string SMR_META_EXT = ".smrm";
        public const string SMR_PROJECT_EXT = ".smrp";
        public const string DOCX_EXT = ".docx";
        public const string DOC_EXT = ".doc";
        public const string ODS_EXT = ".ods";
        public const string XLSX_EXT = ".xlsx";
        public const string XLS_EXT = ".xls";
        public const string DWG_EXT = ".dwg";
        public const string PDF_EXT = ".pdf";
        public const string PNG_EXT = ".png";
        public const string GIF_EXT = ".gif";
        public const string TIFF_EXT = ".tiff";
        public const string JPG_EXT = ".jpg";
        public const string JPEG_EXT = ".jpeg";
        public const string BMP_EXT = ".bmp";
        public const string MP4_EXT = ".mp4";
        public const string MOV_EXT = ".mov";
        public const string AVI_EXT = ".avi";
        public const string WMV_EXT = ".wmv";
        public const string MKV_EXT = ".mkv";
        public const string ZIP_EXT = ".zip";

        public const string SMRP_FILTER = "SMR проект|*.smrp";
        public const string ALL_FILTER = "Все файлы|*.*";
        public const string IMAGE_FILTER = "Изображения|*.jpg;*.jpeg;*.png; *.bmp; *.tiff";
        public const string NAME_DATA_PROJECTS = "data_projects.json";
        public const string NAME_DATA_BUILDER = "data_builder.json";

        public const string IMPORT_FILE = "Файл...";
        public const string CREATE_DIRECTORY = "Папку";
        public const string CREATE_SMR_FILE = "SMR файл";

        public const string NAME_NEW_DIRECTORY = "Новая папка";
        public const string NAME_NEW_SMR_FILE = "SMR";

        public const string SEPARATOR = @"\";
        public const string REG_SUFIX = @"( \(\d+\))";
        public const string REG_NUMBER = @"(\d+)";

        public const string REG_NAME_DIRECTORY = "[.\\:*?\"<>|+]";
        public const string REG_NAME_FILE = "[\\:*?\"<>|+]";

        public const string FILE_FILTER = "*.*";

        public static readonly Font textFont10 = new Font("Roboto", 10);
        public static readonly Font textFont12 = new Font("Roboto", 12);
        public static readonly Font textFont14 = new Font("Roboto", 14);

        public static readonly Color textWhite = Color.White;
        public static readonly Color textError = Color.Red;
        public static readonly Color textDefault = Color.Gray;

        public static readonly Color bg = Color.FromArgb(20, 20, 20);
        public static readonly Color blackClick = Color.FromArgb(40, 40, 40);
        public static readonly Color blackHover = Color.FromArgb(44, 44, 44);
        public static readonly Color black = Color.FromArgb(64, 64, 64);
        public static readonly Color blue = Color.FromArgb(61, 119, 194);
        public static readonly Color smrDataCut = Color.Red;
        public static readonly Color smrDataCopy = Color.FromArgb(61, 119, 194);
        public static readonly Color Transparent = Color.FromArgb(255, 20, 20, 20);

        public static readonly Size windowSize = new Size(1920, 1080);

        public static readonly List<string> Directories = new List<string> {
            "Источники питания",
            "Двигатели",
            "Клапаны",
            "Защитные устройства",
            "Коммутационные устройства",
            "Реле, контакторы",
            "Клеммы",
            "Измерительные устройства",
            "ПЛК",
            "Другие",
            "Электротехнические изделия",
            "Аппараты",
            "Емкости",
            "Документы",
        };

        public static FileState GetFileState(string Extension)
        {
            if (string.IsNullOrEmpty(Extension))
                return FileState.None;

            switch (Extension.ToLower())
            {
                case AVI_EXT:
                case MKV_EXT:
                case WMV_EXT:
                case MP4_EXT:
                    return FileState.Video;

                case DOCX_EXT:
                case DOC_EXT:
                case ODS_EXT:
                    return FileState.Document;

                case DWG_EXT:
                    return FileState.Dwg;

                case JPG_EXT:
                case JPEG_EXT:
                case BMP_EXT:
                case PNG_EXT:
                case TIFF_EXT:
                case GIF_EXT:
                    return FileState.Picture;

                case PDF_EXT:
                    return FileState.Pdf;

                case XLSX_EXT:
                case XLS_EXT:
                    return FileState.Excel;

                case ZIP_EXT:
                    return FileState.Archive;

                default:
                    return FileState.None;
            }
        }

        public static int GetIndexImageByFileState(FileState fileState)
        {
            switch (fileState)
            {
                case FileState.Dwg:
                    return 3;

                case FileState.Pdf:
                    return 4;

                case FileState.Document:
                    return 5;

                case FileState.Excel:
                    return 6;

                case FileState.Picture:
                    return 7;

                case FileState.Archive:
                    return 8;

                case FileState.Video:
                    return 9;

                default:
                    return 2;
            }
        }
    }
}
