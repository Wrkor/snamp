using SNAMP.Models;
using System.Drawing;
using SNAMP.Properties;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace SNAMP.Views
{
    internal class ToolStripNavigation : ToolStrip
    {     
        public ToolStripButton ButtonPrev { get; private set; }
        public ToolStripButton ButtonNext { get; private set; }
        public ToolStripButton ButtonDelete { get; private set; }
        public ToolStripButton ButtonClearBuffer { get; private set; }
        public ToolStripButton ButtonPaste { get; private set; }
        public ToolStripButton ButtonCut { get; private set; }
        public ToolStripButton ButtonCopy { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImportDirectory { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImportSMRFile { get; private set; }
        public ToolStripMenuItem ToolStripMenuItemImportFile { get; private set; }

        private LimitedStack<SMRDataDirectory> prevSMRData;
        private LimitedStack<SMRDataDirectory> nextSMRData;
        private List<ISMRData> selectedItems;

        private SMRDataDirectory currentSMRDataDirectory;

        private readonly SMRStorage smrStorage;

        private bool isClearNextSMRData;
        private bool isAddPrevSMRData;

        public ToolStripNavigation(SMRStorage smrStorage) : base()
        { 
            this.smrStorage = smrStorage;
            currentSMRDataDirectory = smrStorage.SMRDataRoot;
            BackColor = DataDefault.bg;
            Dock = DockStyle.Fill;
            GripStyle = ToolStripGripStyle.Hidden;
            selectedItems = new List<ISMRData>();
            isClearNextSMRData = true;
            isAddPrevSMRData = true;
            InitializeElements();

            prevSMRData = new LimitedStack<SMRDataDirectory>(10);
            nextSMRData = new LimitedStack<SMRDataDirectory>(10);

            smrStorage.OnReadSMRDataDirectoryHandler += OnReadSMRDataDirectory;
            smrStorage.OnDeleteSMRDataHandler += CheckStacks;
            smrStorage.OnSelectedFilesHandler += OnSelectedFiles;
            smrStorage.FormLoadHandler += OnFormLoad;
            smrStorage.FormClosedHandler += OnFormClosed;

            smrStorage.ActivedSMRDataToolHandler += (smrDatas, smtTool) => UpdateBtns();
        }

        private void InitializeElements()
        {
            ButtonPrev = new MToolStripButton("Предыдущая", Resources.arrow_left24) { Alignment = ToolStripItemAlignment.Left };
            ButtonNext = new MToolStripButton("Следующая", Resources.arrow_right24) { Alignment = ToolStripItemAlignment.Left };
            ButtonDelete = new MToolStripButton("Удалить", Resources.Delete24 ) { Margin = new Padding(0, 5, 10, 5) };
            ButtonClearBuffer = new MToolStripButton("Очистить буффер", Resources.Cancel24);
            ButtonPaste = new MToolStripButton("Вставить", Resources.Paste24);
            ButtonCut = new MToolStripButton("Вырезать", Resources.Cut24);
            ButtonCopy = new MToolStripButton("Копировать", Resources.Copy24);

            ToolStripDropDownButton buttonAdd = new ToolStripDropDownButton()
            {
                Size = new Size(37, 30),
                Image = Resources.Create24,
                ImageScaling = ToolStripItemImageScaling.None,
                ImageTransparentColor = Color.Magenta,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Margin = new Padding(0, 5, 5, 5),
                Text = "Добавить",
                Alignment = ToolStripItemAlignment.Right,
            };

            ToolStripMenuItemImportDirectory = new ToolStripMenuItem() { Text = "Папку", Image = Resources.Dir64 };
            ToolStripMenuItemImportSMRFile = new ToolStripMenuItem() { Text = "SMR файл", Image = Resources.SMR64 };
            ToolStripMenuItemImportFile = new ToolStripMenuItem() { Text = "Файл...", Image = Resources.File64 };

            buttonAdd.DropDownItems.AddRange(new ToolStripItem[] {
                ToolStripMenuItemImportDirectory,
                ToolStripMenuItemImportSMRFile,
                ToolStripMenuItemImportFile,
            });

            ButtonPrev.Click += (sender, e) => OnButtonPrevClick();
            ButtonNext.Click += (sender, e) => OnButtonNextClick();
            ButtonDelete.Click += (sender, e) => smrStorage.SMRActions.SMRToolDelete();
            ButtonClearBuffer.Click += (sender, e) => smrStorage.SMRActions.SMRToolCancel();
            ButtonPaste.Click += (sender, e) => smrStorage.SMRActions.SMRToolPaste(currentSMRDataDirectory);
            ButtonCut.Click += (sender, e) => smrStorage.SMRActions.SMRToolCut();
            ButtonCopy.Click += (sender, e) => smrStorage.SMRActions.SMRToolCopy();

            ToolStripMenuItemImportDirectory.Click += (sender, e) => smrStorage.SMRActions.CreateDirectory(currentSMRDataDirectory);
            ToolStripMenuItemImportSMRFile.Click += (sender, e) => smrStorage.SMRActions.CreateSMRFile(currentSMRDataDirectory);
            ToolStripMenuItemImportFile.Click += (sender, e) => smrStorage.SMRActions.ImportFiles(currentSMRDataDirectory);

            Items.AddRange(new ToolStripItem[] {
                ButtonPrev,
                ButtonNext,
                ButtonDelete,
                ButtonClearBuffer,
                ButtonPaste,
                ButtonCut,
                ButtonCopy,
                buttonAdd,
            });
        }

        private void UpdateBtns()
        {
            ButtonPrev.Enabled = prevSMRData.Count > 0;
            ButtonNext.Enabled = nextSMRData.Count > 0;

            if (selectedItems.Count == 0)
            {
                ButtonCopy.Enabled = false;
                ButtonCut.Enabled = false;
                ButtonDelete.Enabled = false;
            }

            else
            {
                ButtonCopy.Enabled = true;
                ButtonCut.Enabled = true;
                ButtonDelete.Enabled = true;
            }

            ButtonClearBuffer.Enabled = smrStorage.SMRActions.SMRDataBuffer.Count > 0;
            ButtonPaste.Enabled = smrStorage.SMRActions.SMRDataBuffer.Count > 0;
        }

        private void PushInStack(LimitedStack<SMRDataDirectory> stack, SMRDataDirectory smrDataDirectory)
        {
            if (smrDataDirectory.Node.TreeView == null || (stack.Count != 0 && stack.Peek() == smrDataDirectory))
                return;

            stack.Push(smrDataDirectory);
        }

        private void CheckStacks(List<ISMRData> smrDatas)
        {
            List<SMRDataDirectory> smrDatasDirectory = smrDatas.Where(smrData => smrData is SMRDataDirectory).Cast<SMRDataDirectory>().ToList();
            if (prevSMRData.Count != 0)
            {
                LimitedStack<SMRDataDirectory> newStack = new LimitedStack<SMRDataDirectory>(10);
                foreach (SMRDataDirectory smrData in prevSMRData)
                    PushInStack(newStack, smrData);

                newStack.Reverse();
                if (newStack.Count != 0 && newStack.Peek() == currentSMRDataDirectory)
                    newStack.Pop();
                prevSMRData = newStack;
            }

            if (nextSMRData.Count != 0)
            {
                LimitedStack<SMRDataDirectory> newStack = new LimitedStack<SMRDataDirectory>(10);

                foreach (SMRDataDirectory smrData in nextSMRData)
                    PushInStack(newStack, smrData);

                newStack.Reverse();
                if (newStack.Count != 0 && newStack.Peek() == currentSMRDataDirectory)
                    newStack.Pop();
                nextSMRData = newStack;
            }
        }

        private void OnButtonPrevClick()
        {
            if (prevSMRData.Count != 0)
            {
                SMRDataDirectory smrDataDirectoryPrev = prevSMRData.Pop();

                if (smrDataDirectoryPrev.Node.TreeView != null)
                {
                    isClearNextSMRData = false;
                    isAddPrevSMRData = false;
                    PushInStack(nextSMRData, currentSMRDataDirectory);
                    smrStorage.SMRActions.CheckUpdateActivePath(smrDataDirectoryPrev);
                    isAddPrevSMRData = true;
                    isClearNextSMRData = true;
                }
            }

            UpdateBtns();
        }

        private void OnButtonNextClick()
        {
            if (nextSMRData.Count != 0)
            {
                SMRDataDirectory smrDataDirectoryNext = nextSMRData.Pop();

                if (smrDataDirectoryNext.Node.TreeView != null)
                {
                    isClearNextSMRData = false;
                    PushInStack(prevSMRData, currentSMRDataDirectory);
                    smrStorage.SMRActions.CheckUpdateActivePath(smrDataDirectoryNext);
                    isClearNextSMRData = true;
                }
            }

            UpdateBtns();
        }

        private void OnReadSMRDataDirectory(SMRDataDirectory data)
        {
            if (!smrStorage.IsLoad || currentSMRDataDirectory == data)
                return;

            if (isAddPrevSMRData)
                PushInStack(prevSMRData, currentSMRDataDirectory);

            currentSMRDataDirectory = data;

            if (isClearNextSMRData)
                nextSMRData.Clear();

            UpdateBtns();
        }

        private void OnFormLoad()
        {
            UpdateBtns();
        }

        private void OnFormClosed()
        {
            ButtonPrev.Click -= (sender, e) => OnButtonPrevClick();
            ButtonNext.Click -= (sender, e) => OnButtonNextClick();
            ButtonDelete.Click -= (sender, e) => smrStorage.SMRActions.SMRToolDelete();
            ButtonClearBuffer.Click -= (sender, e) => smrStorage.SMRActions.SMRToolCancel();
            ButtonPaste.Click -= (sender, e) => smrStorage.SMRActions.SMRToolPaste(currentSMRDataDirectory);
            ButtonCut.Click -= (sender, e) => smrStorage.SMRActions.SMRToolCut();
            ButtonCopy.Click -= (sender, e) => smrStorage.SMRActions.SMRToolCopy();

            ToolStripMenuItemImportDirectory.Click -= (sender, e) => smrStorage.SMRActions.CreateDirectory(currentSMRDataDirectory);
            ToolStripMenuItemImportSMRFile.Click -= (sender, e) => smrStorage.SMRActions.CreateSMRFile(currentSMRDataDirectory);
            ToolStripMenuItemImportFile.Click -= (sender, e) => smrStorage.SMRActions.ImportFiles(currentSMRDataDirectory);

            smrStorage.OnReadSMRDataDirectoryHandler -= OnReadSMRDataDirectory;
            smrStorage.OnDeleteSMRDataHandler -= CheckStacks;
            smrStorage.ActivedSMRDataToolHandler -= (smrDatas, smtTool) => UpdateBtns();
            smrStorage.OnSelectedFilesHandler -= OnSelectedFiles;
            smrStorage.FormLoadHandler -= OnFormLoad;
            smrStorage.FormClosedHandler -= OnFormClosed;
        }

        private void OnSelectedFiles(List<ISMRData> list)
        {
            selectedItems = list;
            UpdateBtns();
        }
    }
}
