using System.Drawing;
using SNAMP.Properties;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Views
{
    internal class PanelProperties : Panel
    {
        public TableLayoutPanelProperties TableLayoutPanelProperties { get; private set; }
        public MLabel LabelPropertyName { get; private set; }

        private const string CHOICE_OBJECT = "Выберите объект";

        private readonly ToolTip toolTipLabel;
        private readonly SMRStorage smrStorage;

        private SMRDataFile smrDataFileCurrent;

        public PanelProperties(SMRStorage smrStorage) : base()
        {
            this.smrStorage = smrStorage;

            Size = new Size(290, 960);
            Dock = DockStyle.Fill;
            InitializeElements();
            toolTipLabel = new ToolTip();

            smrStorage.OnSelectedFilesHandler += UpdateData;
            smrStorage.OnDeleteSMRDataHandler += ResetData;
            smrStorage.FormClosedHandler += OnFormClosed;
        }

        private void UpdateData(List<ISMRData> selectedItems)
        {
            if (selectedItems.Count == 0 || !(selectedItems[0] is SMRDataFile smrDataFile))
                return;

            smrDataFileCurrent = smrDataFile;
            LabelPropertyName.Text = smrDataFile.Name;
            toolTipLabel.SetToolTip(LabelPropertyName, smrDataFile.Name);
            TableLayoutPanelProperties.UpdateTable(smrDataFile);
        }

        private void ResetData(List<ISMRData> smrDatas)
        {
            if (smrDatas.Contains(smrDataFileCurrent))
            {
                LabelPropertyName.Text = CHOICE_OBJECT;
                toolTipLabel.SetToolTip(LabelPropertyName, "");
                TableLayoutPanelProperties.UpdateTable();
            }
        }

        private void InitializeElements()
        {
            LabelPropertyName = new MLabel(CHOICE_OBJECT, DataDefault.textFont14)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(5, 5),
                Size = new Size(280, 23),
            };

            Label labelLinks = new MLabel("Зависимости:", DataDefault.textFont14)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(5, 40),
                Size = new Size(240, 23),
            };

            Button buttonAdd = new MButton(new Size(30, 30), AnchorStyles.Top | AnchorStyles.Right, Resources.plus24, new Point(250, 35));
            buttonAdd.Click += (sender, e) => OnButtonAddClick();

            Panel panelProperty = new Panel() { 
                Location = new Point(5, 70),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(280, 880),
            };

            TableLayoutPanelProperties = new TableLayoutPanelProperties(smrStorage);
            panelProperty.Controls.Add(TableLayoutPanelProperties);

            Controls.Add(LabelPropertyName);
            Controls.Add(labelLinks);
            Controls.Add(buttonAdd);
            Controls.Add(panelProperty);
            
        }

        private void OnFormClosed()
        {
            smrStorage.OnSelectedFilesHandler -= UpdateData;
            smrStorage.OnDeleteSMRDataHandler -= ResetData;
            smrStorage.FormClosedHandler -= OnFormClosed;
        }

        private void OnButtonAddClick() => TableLayoutPanelProperties.CreateProperty();
    }
}
