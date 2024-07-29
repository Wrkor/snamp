namespace SNAMP
{
    partial class SettingsTemplatesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsTemplatesForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.splitContainerTemplate = new System.Windows.Forms.SplitContainer();
            this.toolStripTemplate = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnRename = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnCreate = new System.Windows.Forms.ToolStripButton();
            this.iconList16 = new System.Windows.Forms.ImageList(this.components);
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTemplate)).BeginInit();
            this.splitContainerTemplate.Panel1.SuspendLayout();
            this.splitContainerTemplate.SuspendLayout();
            this.toolStripTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoEllipsis = true;
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Roboto", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.titleLabel.Location = new System.Drawing.Point(59, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(361, 25);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Редактор шаблона новых проектов";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitContainerTemplate
            // 
            this.splitContainerTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerTemplate.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerTemplate.IsSplitterFixed = true;
            this.splitContainerTemplate.Location = new System.Drawing.Point(12, 49);
            this.splitContainerTemplate.Name = "splitContainerTemplate";
            this.splitContainerTemplate.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTemplate.Panel1
            // 
            this.splitContainerTemplate.Panel1.Controls.Add(this.toolStripTemplate);
            this.splitContainerTemplate.Size = new System.Drawing.Size(460, 539);
            this.splitContainerTemplate.SplitterDistance = 40;
            this.splitContainerTemplate.TabIndex = 4;
            // 
            // toolStripTemplate
            // 
            this.toolStripTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.toolStripTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTemplate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnRename,
            this.toolStripBtnDelete,
            this.toolStripBtnCreate});
            this.toolStripTemplate.Location = new System.Drawing.Point(0, 0);
            this.toolStripTemplate.Name = "toolStripTemplate";
            this.toolStripTemplate.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripTemplate.Size = new System.Drawing.Size(460, 40);
            this.toolStripTemplate.TabIndex = 1;
            // 
            // toolStripBtnRename
            // 
            this.toolStripBtnRename.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnRename.BackColor = System.Drawing.Color.Transparent;
            this.toolStripBtnRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnRename.ForeColor = System.Drawing.Color.Transparent;
            this.toolStripBtnRename.Image = global::SNAMP.Properties.Resources.rename24;
            this.toolStripBtnRename.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnRename.Margin = new System.Windows.Forms.Padding(0, 5, 10, 5);
            this.toolStripBtnRename.Name = "toolStripBtnRename";
            this.toolStripBtnRename.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.toolStripBtnRename.Size = new System.Drawing.Size(30, 30);
            this.toolStripBtnRename.Text = "Переименовать (F2)";
            // 
            // toolStripBtnDelete
            // 
            this.toolStripBtnDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnDelete.Image = global::SNAMP.Properties.Resources.minus24;
            this.toolStripBtnDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnDelete.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripBtnDelete.Name = "toolStripBtnDelete";
            this.toolStripBtnDelete.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.toolStripBtnDelete.Size = new System.Drawing.Size(30, 30);
            this.toolStripBtnDelete.Text = "Удалить (-)";
            // 
            // toolStripBtnCreate
            // 
            this.toolStripBtnCreate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripBtnCreate.BackColor = System.Drawing.Color.Transparent;
            this.toolStripBtnCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnCreate.ForeColor = System.Drawing.Color.Transparent;
            this.toolStripBtnCreate.Image = global::SNAMP.Properties.Resources.plus24;
            this.toolStripBtnCreate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnCreate.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.toolStripBtnCreate.Name = "toolStripBtnCreate";
            this.toolStripBtnCreate.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.toolStripBtnCreate.Size = new System.Drawing.Size(30, 30);
            this.toolStripBtnCreate.Text = "Создать (+)";
            // 
            // iconList16
            // 
            this.iconList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList16.ImageStream")));
            this.iconList16.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList16.Images.SetKeyName(0, "Dir16.png");
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.AutoSize = true;
            this.buttonSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(119)))), ((int)(((byte)(194)))));
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSave.FlatAppearance.BorderSize = 0;
            this.buttonSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.buttonSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Font = new System.Drawing.Font("Roboto", 12F);
            this.buttonSave.ForeColor = System.Drawing.Color.White;
            this.buttonSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSave.Location = new System.Drawing.Point(353, 606);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.buttonSave.Size = new System.Drawing.Size(119, 37);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Roboto", 12F);
            this.buttonCancel.ForeColor = System.Drawing.Color.White;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(221, 606);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.buttonCancel.Size = new System.Drawing.Size(113, 37);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Отменить";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.AutoSize = true;
            this.buttonReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonReset.FlatAppearance.BorderSize = 0;
            this.buttonReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.buttonReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReset.Font = new System.Drawing.Font("Roboto", 12F);
            this.buttonReset.ForeColor = System.Drawing.Color.White;
            this.buttonReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonReset.Location = new System.Drawing.Point(12, 606);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.buttonReset.Size = new System.Drawing.Size(110, 37);
            this.buttonReset.TabIndex = 7;
            this.buttonReset.Text = "Сбросить";
            this.buttonReset.UseVisualStyleBackColor = false;
            // 
            // SettingsTemplatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(484, 661);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.splitContainerTemplate);
            this.Controls.Add(this.titleLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "SettingsTemplatesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SNAMP - Настройка шаблонов";
            this.splitContainerTemplate.Panel1.ResumeLayout(false);
            this.splitContainerTemplate.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTemplate)).EndInit();
            this.splitContainerTemplate.ResumeLayout(false);
            this.toolStripTemplate.ResumeLayout(false);
            this.toolStripTemplate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.SplitContainer splitContainerTemplate;
        private System.Windows.Forms.ToolStrip toolStripTemplate;
        private System.Windows.Forms.ToolStripButton toolStripBtnRename;
        private System.Windows.Forms.ToolStripButton toolStripBtnDelete;
        private System.Windows.Forms.ToolStripButton toolStripBtnCreate;
        private System.Windows.Forms.ImageList iconList16;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonReset;
    }
}