using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Views
{
    public class TableLayoutPanelCard : TableLayoutPanel
    {
        public Action SaveSMRDataSMRFileHandler;
        public Action CloseSMRDataSMRFileHandler;

        public PanelFieldInput PanelFieldPath { get; private set; }
        public PanelFieldInput PanelFieldName { get; private set; }
        public PanelFieldInput PanelFieldPosition { get; private set; }
        public PanelFieldInput PanelFieldDescription { get; private set; }
        public PanelFieldInputFile PanelFieldImage { get; private set; }
        public PanelPictureBoxImage PanelPictureImage { get; private set; }
        public Button ButtonCancel { get; private set; }
        public PanelButton PanelButtonSaveCancel { get; private set; }
        public TableLayoutPanelSpecification TableLayoutPanelSpecification { get; private set; }
        public ListViewSMR ListViewSMR { get; private set; }

        private const string PATH = "Файл";
        private const string NAME = "Наименование";
        private const string POSITION = "Обозначение";
        private const string DESCRIPTION = "Описание";
        private const string IMAGE = "Изображение";

        private readonly SMRDataSMRFile smrDataSMRFile;
        private readonly SMRStorage smrStorage;

        public TableLayoutPanelCard(SMRDataSMRFile smrDataSMRFile, SMRStorage smrStorage) : base()
        {
            this.smrDataSMRFile = smrDataSMRFile;
            this.smrStorage = smrStorage;
            RowCount = 7;
            ColumnCount = 2;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScroll = true;
            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            ColumnStyles.Clear();
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 30 });
            ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 70 });

            RowStyles.Clear();
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 60 });
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 80 });
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 60 });
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 200 });
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 65 });
            RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
            RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
            RowStyles.Add(new RowStyle { SizeType = SizeType.Absolute, Height = 80 });

            InitializeElements();
            InitializeData();

            Controls.Add(PanelPictureImage, 0, 0);

            Controls.Add(PanelFieldPath, 1, 0);
            Controls.Add(PanelFieldName, 1, 1);
            Controls.Add(PanelFieldPosition, 1, 2);
            Controls.Add(PanelFieldDescription, 1, 3);
            Controls.Add(PanelFieldImage, 1, 4);

            Controls.Add(TableLayoutPanelSpecification, 0, 5);
            Controls.Add(ListViewSMR, 0, 6);

            Controls.Add(PanelButtonSaveCancel, 0,7);

            SetRowSpan(PanelPictureImage, 5);
            SetColumnSpan(ListViewSMR, 2);
            SetColumnSpan(TableLayoutPanelSpecification, 2);
            SetColumnSpan(PanelButtonSaveCancel, 2);
        }

        private void InitializeData()
        {
            string pathToImage = Path.Combine(smrStorage.SMRDataRoot.PathToSMRDataDirectory.Parent.FullName, smrDataSMRFile.DataSMR.PathToImage);
            PanelPictureImage.SetPictureBoxImage(pathToImage);
            PanelFieldImage.SetTextBoxFieldData(pathToImage);

            PanelFieldPath.SetTextBoxFieldData(smrDataSMRFile.FullPathToSMRData);
            PanelFieldName.SetTextBoxFieldData(smrDataSMRFile.DataSMR.Name);
            PanelFieldPosition.SetTextBoxFieldData(smrDataSMRFile.DataSMR.Position);
            PanelFieldDescription.SetTextBoxFieldData(smrDataSMRFile.DataSMR.Description);

            PanelFieldName.TextBoxField.DataBindings.Add("Text", smrDataSMRFile.DataSMR, "Name");
            PanelFieldPosition.TextBoxField.DataBindings.Add("Text", smrDataSMRFile.DataSMR, "Position", false, DataSourceUpdateMode.OnPropertyChanged);
            PanelFieldDescription.TextBoxField.DataBindings.Add("Text", smrDataSMRFile.DataSMR, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void FormClosed()
        {
            ListViewSMR.FormClosed();
            TableLayoutPanelSpecification.FormClosed();
            PanelFieldImage.FormClosed();

            PanelButtonSaveCancel.ButtonSave.Click -= OnButtonSaveClick;
            PanelButtonSaveCancel.ButtonCancel.Click -= OnButtonCancelClick;
            PanelFieldImage.ChangedPathToImageHandler -= OnChangedPathToImage;
            smrStorage.OnUpdateSMRDataHandler -= OnUpdateSMRData;
        }

        private void InitializeElements()
        {
            PanelPictureImage = new PanelPictureBoxImage();

            PanelFieldImage = new PanelFieldInputFile(IMAGE, smrStorage.SMRProject.Path);
            PanelFieldPath = new PanelFieldInput(PATH, 20, 1000, isReadOnly: true);
            PanelFieldName = new PanelFieldInput(NAME, 45, isMultiline: true);
            PanelFieldPosition = new PanelFieldInput(POSITION, 20);
            PanelFieldDescription = new PanelFieldInput(DESCRIPTION, 165, 2000, isMultiline: true, scrollBars: ScrollBars.Vertical);

            ListViewSMR = new ListViewSMR(smrStorage, smrDataSMRFile);

            TableLayoutPanelSpecification = new TableLayoutPanelSpecification(smrDataSMRFile);

            PanelButtonSaveCancel = new PanelButton();
            PanelButtonSaveCancel.ButtonSave.Click += OnButtonSaveClick;
            PanelButtonSaveCancel.ButtonCancel.Click += OnButtonCancelClick;
            PanelFieldImage.ChangedPathToImageHandler += OnChangedPathToImage;
            smrStorage.OnUpdateSMRDataHandler += OnUpdateSMRData;
            smrStorage.OnDeleteSMRDataHandler += OnUpdateSMRData;
        }

        private void OnUpdateSMRData(List<ISMRData> smrDatas)
        {
            PanelFieldPath.SetTextBoxFieldData(smrDataSMRFile.FullPathToSMRData);
            ListViewSMR.InitializeData();
        }

        private void OnButtonSaveClick(object sender, EventArgs e) => SaveSMRDataSMRFileHandler?.Invoke();

        private void OnButtonCancelClick(object sender, EventArgs e) => CloseSMRDataSMRFileHandler?.Invoke();

        private void OnChangedPathToImage(string path)
        {
            ISMRData smrData = smrStorage.SMRActions.FindSMRData(path);

            if (smrData != null)
            {
                smrDataSMRFile.DataSMR.PathToImage = smrData.Node.FullPath;
                PanelPictureImage.SetPictureBoxImage(smrData.FullPathToSMRData);
            }
        }
    }
}
