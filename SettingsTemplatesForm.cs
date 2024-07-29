using SNAMP.Utils;
using SNAMP.Views;
using System.Windows.Forms;

namespace SNAMP
{
    public partial class SettingsTemplatesForm : Form
    {
        private DataBuilder DataBuilder
        {
            get
            {
                if (_dataBuilder == null)
                    _dataBuilder = DataSerialize.ReadData<DataBuilder>(DataDefault.NAME_DATA_BUILDER);

                return _dataBuilder;
            }
            set => _dataBuilder = value;
        }

        private DataBuilder _dataBuilder;
        private TreeViewTemplate treeViewTemplate;

        public SettingsTemplatesForm()
        {
            InitializeComponent();
            InitializeElements();
        }

        private void InitializeElements()
        {
            toolStripBtnCreate.Click += (sender, e) => OnToolStripBtnCreateClick();
            toolStripBtnDelete.Click += (sender, e) => OnToolStripBtnDeleteClick();
            toolStripBtnRename.Click += (sender, e) => OnToolStripBtnRenameClick();
            buttonCancel.Click += (sender, e) => OnButtonCancelClick();
            buttonSave.Click += (sender, e) => OnButtonSaveClick();
            buttonReset.Click += (sender, e) => OnButtonResetClick();
            FormClosing += OnSettingsTemplatesFormFormClosing;
            FormClosed += OnSettingsTemplatesFormFormClosed;

            treeViewTemplate = new TreeViewTemplate(iconList16);
            treeViewTemplate.InitializeData(DataBuilder.directories);
            splitContainerTemplate.Panel2.Controls.Add(treeViewTemplate);
        }

        private void OnButtonSaveClick()
        {
            if (!treeViewTemplate.IsSave)
            {                
                DataBuilder.directories = treeViewTemplate.GetData();
                DataSerialize.WriteData(DataBuilder, DataDefault.NAME_DATA_BUILDER);
                treeViewTemplate.IsSave = true;
            }

            Close();
        }

        private void OnButtonResetClick()
        {
            if (DialogWindow.MessageWarning("Сбросить весь шаблон до структуры по умолчанию?") == DialogResult.Yes)
            {
                DataBuilder.SetDataByDefalut();
                DataSerialize.WriteData(DataBuilder, DataDefault.NAME_DATA_BUILDER);
                treeViewTemplate.InitializeData(DataBuilder.directories);
                treeViewTemplate.IsSave = true;
            }
        }

        private void OnSettingsTemplatesFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (treeViewTemplate.IsSave || DialogWindow.MessageWarning("Выйти без сохранения?") == DialogResult.Yes)
                return;

            e.Cancel = true;
        }

        private void OnSettingsTemplatesFormFormClosed(object sender, FormClosedEventArgs e)
        {
            treeViewTemplate.FormClose();
            toolStripBtnCreate.Click -= (sender2, e2) => OnToolStripBtnCreateClick();
            toolStripBtnDelete.Click -= (sender2, e2) => OnToolStripBtnDeleteClick();
            toolStripBtnRename.Click -= (sender2, e2) => OnToolStripBtnRenameClick();
            buttonCancel.Click -= (sender2, e2) => OnButtonCancelClick();
            buttonSave.Click -= (sender2, e2) => OnButtonSaveClick();
            buttonReset.Click -= (sender2, e2) => OnButtonResetClick();
            FormClosing -= OnSettingsTemplatesFormFormClosing;
            FormClosed -= OnSettingsTemplatesFormFormClosed;
        }

        private void OnButtonCancelClick() => Close();

        private void OnToolStripBtnCreateClick() => treeViewTemplate.CreateNode();

        private void OnToolStripBtnDeleteClick() => treeViewTemplate.DeleteNode();

        private void OnToolStripBtnRenameClick() => treeViewTemplate.RenameNode();
    }
}
