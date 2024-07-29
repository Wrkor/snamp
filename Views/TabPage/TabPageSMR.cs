using System;
using SNAMP.Utils;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class TabPageSMR : TabPage
    {
        public Action<TabPageSMR> CloseTabPageSMRHandler;

        public SMRDataSMRFile SMRDataSMRFile { get; private set; }
        public TableLayoutPanelCard TableLayoutPanelCard { get; private set; }
        public MTabControl MTabControl { get;  set; }

        private readonly SMRStorage smrStorage;

        public TabPageSMR(SMRStorage smrStorage, SMRDataSMRFile smrDataSMRFile, MTabControl mTabControl) : base()
        {
            this.smrStorage = smrStorage;
            SMRDataSMRFile = smrDataSMRFile;
            SMRDataSMRFile.DataSMR = null;
            SMRDataSMRFile.TabPageSMR = this;
            MTabControl = mTabControl;

            Text = smrDataSMRFile.Name;
            BackColor = DataDefault.bg;
            Dock = DockStyle.Fill;

            InitializeElements();
        }

        private void InitializeElements()
        {
            TableLayoutPanelCard = new TableLayoutPanelCard(SMRDataSMRFile, smrStorage);

            TableLayoutPanelCard.SaveSMRDataSMRFileHandler += SaveSMRDataSMRFile;
            TableLayoutPanelCard.CloseSMRDataSMRFileHandler += OnCloseSMRDataSMRFile;

            smrStorage.FormClosedHandler += CloseSMRDataSMRFile;

            Controls.Add(TableLayoutPanelCard);
        }

        public void SaveSMRDataSMRFile()
        {
            DataSerialize.WriteData(SMRDataSMRFile.DataSMR, SMRDataSMRFile.FullPathToSMRData);
            DialogWindow.MessageSuccess($"{SMRDataSMRFile.Name} - успешно сохранен");
        }

        public void CloseSMRDataSMRFile()
        {
            if (SMRDataSMRFile != null)
                SMRDataSMRFile.TabPageSMR = null;

            TableLayoutPanelCard.FormClosed();
            TableLayoutPanelCard.SaveSMRDataSMRFileHandler -= SaveSMRDataSMRFile;
            TableLayoutPanelCard.CloseSMRDataSMRFileHandler -= OnCloseSMRDataSMRFile;
            smrStorage.FormClosedHandler -= CloseSMRDataSMRFile;
        }

        private void OnCloseSMRDataSMRFile() => CloseTabPageSMRHandler?.Invoke(this);
    }
}
