using System;
using System.IO;
using SNAMP.Utils;
using SNAMP.Models;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP.Views
{
    public class TableLayoutPanelProperties : TableLayoutPanel
    {
        public Action<string> OnClickLinkLabelPropertyHandler;

        private const string CHOICE_OBJECT = "Выберите объект";

        private readonly SMRStorage smrStorage;

        private SMRDataFile smrDataFileCurrent;

        public TableLayoutPanelProperties(SMRStorage smrStorage) : base()
        {
            this.smrStorage = smrStorage;
            smrStorage.FormClosedHandler += OnFormClosedHandler;
            Dock = DockStyle.Fill;
            ColumnCount = 1;
            RowCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));
            AutoScroll = true;
            AutoSize = true;
            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
            UpdateScroll();
        }

        public void CreateProperty()
        {
            if (smrDataFileCurrent == null)
            {
                DialogWindow.MessageError(CHOICE_OBJECT);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = DataDefault.ALL_FILTER,
                Multiselect = true,
                InitialDirectory = smrDataFileCurrent?.PathToSMRDataParent?.FullName ?? smrStorage.SMRProject.Path,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileNames.Length != 0)
            {
                bool isSave = false;
                List<ISMRData> smrDatasUpdate = new List<ISMRData>();
                foreach (string fileName in openFileDialog.FileNames)
                {
                    if (!fileName.StartsWith(smrStorage.SMRProject.Path))
                    {
                        DialogWindow.MessageError($"Элемент {fileName} должен находиться внутри проекта: {smrStorage.SMRProject.Name}");
                        continue;
                    }

                    if (fileName == smrDataFileCurrent.FullPathToSMRData)
                    {
                        DialogWindow.MessageError("Нельзя для привязки выбрать этот же объект");
                        continue;
                    }

                    if (smrDataFileCurrent.DataMeta.links.Exists(link => link.Link == fileName))
                    {
                        DialogWindow.MessageError($"Элемент {fileName} уже привязан к объекту: {smrDataFileCurrent.Name}");
                        continue;
                    }

                    if (!File.Exists(fileName))
                    {
                        DialogWindow.MessageError($"Непредвиденная ошибка с элементом: {fileName}");
                        continue;
                    }

                    FileInfo fileInfo = new FileInfo(fileName);

                    if (BuilderSMR.CheckOnSMRProjectFile(fileInfo))
                    {
                        DialogWindow.MessageError($"Привязываемый элемент {fileName} не должен быть SMR проектом!");
                        continue;
                    }

                    if (BuilderSMR.CheckOnSMRMetaFile(fileInfo))
                    {
                        DialogWindow.MessageError($"Привязываемый элемент {fileName} не должен быть SMR мета файлом!");
                        continue;
                    }

                    ISMRData smrDataFind = smrStorage.SMRActions.FindSMRData(fileInfo.FullName);
                    
                    if (!(smrDataFind is SMRDataFile smrDataFile))
                    {
                        DialogWindow.MessageError($"Непредвиденная ошибка с элементом: {fileName}");
                        continue;
                    }

                    InputForm inputForm = new InputForm(smrDataFind.Name);
                    DialogResult result = inputForm.ShowDialog();

                    if (result == DialogResult.Cancel)
                        return;

                    else if (result == DialogResult.No)
                        continue;

                    if (!int.TryParse(inputForm.Input, out int page))
                        continue;

                    smrDataFile.DataMeta.links.Add(new LinkToFile(smrDataFileCurrent.Node.FullPath, smrDataFileCurrent.FullPathToSMRData));
                    DataSerialize.WriteData(smrDataFile.DataMeta, smrDataFile.FullPathToSMRDataMeta);
                    smrDatasUpdate.Add(smrDataFile);

                    LinkToFile linkToFile = new LinkToFile(smrDataFind.Node.FullPath, smrDataFind.FullPathToSMRData, page);
                    smrDataFileCurrent.DataMeta.links.Add(linkToFile);

                    AddRow(linkToFile);
                    UpdateScroll();
                    isSave = true;
                    
                }

                if (isSave)
                {
                    DataSerialize.WriteData(smrDataFileCurrent.DataMeta, smrDataFileCurrent.FullPathToSMRDataMeta);
                    smrDatasUpdate.Add(smrDataFileCurrent);
                    smrStorage.UpdateSMRData(smrDatasUpdate);
                }
            }
        }

        public void UpdateTable(SMRDataFile smrDataFile = null)
        {
            Visible = false;
            smrDataFileCurrent = smrDataFile;
            if (smrDataFile == null)
            {
                for (int i = Controls.Count - 1; i >= 0; i--)
                {
                    if (GetControlFromPosition(0, i) is PanelProperty panelProperty)
                    {
                        panelProperty.LinkLabel.LinkClicked -= OnLinkClickedLinkLabel;
                        panelProperty.PageLabel.LinkClicked -= OnLinkClickedPageLabel;
                        panelProperty.ButtonDelete.Click -= OnButtonDeleteClick;

                        Controls.Remove(panelProperty);
                        RowCount--;
                        if (RowStyles.Count > RowCount - 1)
                            RowStyles.RemoveAt(i);
                    }
                }
            }

            else
            {
                bool isLinkError = false;
                for (int i = smrDataFile.DataMeta.links.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(smrDataFile.DataMeta.links[i].Link))
                        smrDataFile.DataMeta.links[i].Link = Path.Combine(smrStorage.SMRDataRoot.PathToSMRDataDirectory.Parent.FullName, smrDataFile.DataMeta.links[i].LinkTreeNode);
                    
                    if (!File.Exists(smrDataFile.DataMeta.links[i].Link))
                    {
                        isLinkError = true;
                        smrDataFile.DataMeta.links.Remove(smrDataFile.DataMeta.links[i]);
                    }
                }

                if (isLinkError)
                    DataSerialize.WriteData(smrDataFile.DataMeta, smrDataFile.FullPathToSMRDataMeta);

                if (RowCount - 1 < smrDataFile.DataMeta.links.Count)
                {
                    for (int i = 0; i < smrDataFile.DataMeta.links.Count; i++)
                    {
                        if (i < RowCount - 1)
                        {
                            if (GetControlFromPosition(0, i) is PanelProperty panelProperty)
                                panelProperty.SetData(smrDataFile.DataMeta.links[i]);
                        }

                        else
                        {
                            AddRow(smrDataFile.DataMeta.links[i]);
                        }
                    }
                }

                else
                {
                    for (int i = RowCount - 2; i >= 0; i--)
                    {
                        if (i < smrDataFile.DataMeta.links.Count)
                        {
                            if (GetControlFromPosition(0, i) is PanelProperty panelProperty)
                                panelProperty.SetData(smrDataFile.DataMeta.links[i]);
                        }

                        else
                        {
                            if (GetControlFromPosition(0, i) is PanelProperty panelProperty)
                            {
                                panelProperty.LinkLabel.LinkClicked -= OnLinkClickedLinkLabel;
                                panelProperty.PageLabel.LinkClicked -= OnLinkClickedPageLabel;
                                panelProperty.ButtonDelete.Click -= OnButtonDeleteClick;

                                Controls.Remove(panelProperty);
                                RowCount--;
                                if (RowStyles.Count > RowCount - 1)
                                    RowStyles.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            UpdateScroll();
            Visible = true;
        }

        private void AddRow(LinkToFile linkToFile)
        {
            PanelProperty panelProperty = new PanelProperty(linkToFile);

            panelProperty.LinkLabel.LinkClicked += OnLinkClickedLinkLabel;
            panelProperty.PageLabel.LinkClicked += OnLinkClickedPageLabel;
            panelProperty.ButtonDelete.Click += OnButtonDeleteClick;
            RowStyles.Insert(RowStyles.Count - 1, new RowStyle(SizeType.Absolute, Height = 70));
            RowCount++;

            Controls.Add(panelProperty, 0, RowCount - 2);
        }

        private void UpdateScroll()
        {
            AutoScroll = false;
            HorizontalScroll.Maximum = 0;
            AutoScroll = true;
        }

        private void OnFormClosedHandler()
        {
            smrStorage.FormClosedHandler -= OnFormClosedHandler;

            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if (GetControlFromPosition(0, i) is PanelProperty panelProperty)
                {
                    panelProperty.LinkLabel.LinkClicked -= OnLinkClickedLinkLabel;
                    panelProperty.PageLabel.LinkClicked -= OnLinkClickedPageLabel;
                    panelProperty.ButtonDelete.Click -= OnButtonDeleteClick;
                }
            }
        }

        private void OnLinkClickedLinkLabel(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (smrDataFileCurrent == null)
            {
                DialogWindow.MessageError(CHOICE_OBJECT);
                return;
            }

            if (!(sender is LinkLabel linkLabel) || !(linkLabel.Tag is string path) || string.IsNullOrWhiteSpace(path))
                return;

            ISMRData smrDataFind = smrStorage.SMRActions.FindSMRData(path);
            if ( smrDataFind == null)
                return;

            smrStorage.SMRActions.CheckUpdateActivePath(smrDataFind);
        }

        private void OnLinkClickedPageLabel(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (smrDataFileCurrent == null)
            {
                DialogWindow.MessageError(CHOICE_OBJECT);
                return;
            }

            if (!(sender is LinkLabel linkLabel) || !(linkLabel.Tag is string path) || string.IsNullOrWhiteSpace(path))
                return;

            ISMRData smrDataFind = smrStorage.SMRActions.FindSMRData(path);
            if (smrDataFind == null)
                return;

            smrStorage.SMRActions.SMRToolOpen(smrDataFind);

            if (smrDataFind is SMRDataFile smrDataFile && smrDataFile.TabPageFile != null && int.TryParse(linkLabel.Text, out int page))
                smrDataFile.TabPageFile.GoToPage(page);
        }

        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            if (smrDataFileCurrent == null)
            {
                DialogWindow.MessageError(CHOICE_OBJECT);
                return;
            }

            if (!(sender is Button buttonDelete) || !(buttonDelete.Tag is PanelProperty panelProperty))
                return;

            List<ISMRData> smrDatasUpdate = new List<ISMRData>();

            if (panelProperty.File.Exists)
            {
                ISMRData smrDataUpdate = smrStorage.SMRActions.FindSMRData(panelProperty.File.FullName);
                if (smrDataUpdate is SMRDataFile smrDataFile && smrDataFile.DataMeta.links.Remove(smrDataFile.DataMeta.links.Find(link => link.LinkTreeNode == smrDataFileCurrent.Node.FullPath)))
                {
                    DataSerialize.WriteData(smrDataFile.DataMeta, smrDataFile.FullPathToSMRDataMeta);
                    smrDatasUpdate.Add(smrDataUpdate);
                }
            }
            

            if (smrDataFileCurrent.DataMeta.links.Remove(panelProperty.LinkToFile))
            {
                DataSerialize.WriteData(smrDataFileCurrent.DataMeta, smrDataFileCurrent.FullPathToSMRDataMeta);
                smrDatasUpdate.Add(smrDataFileCurrent);
                BuilderTableLayoutPanel.RemoveArbitraryRow(this, GetRow(panelProperty));
                RowCount--;

                if (RowStyles.Count > RowCount)
                    RowStyles.RemoveAt(RowStyles.Count - 2);
                UpdateScroll();
            }

            if (smrDatasUpdate.Count > 0)
                smrStorage.UpdateSMRData(smrDatasUpdate);
        }
    }
}
