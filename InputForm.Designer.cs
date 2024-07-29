namespace SNAMP
{
    partial class InputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm));
            this.cancelLinkBtn = new System.Windows.Forms.Button();
            this.createLinkBtn = new System.Windows.Forms.Button();
            this.nameNewProjectLabel = new System.Windows.Forms.Label();
            this.countInput = new System.Windows.Forms.TextBox();
            this.nameInputPanel = new System.Windows.Forms.Panel();
            this.nameInputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelLinkBtn
            // 
            this.cancelLinkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelLinkBtn.AutoSize = true;
            this.cancelLinkBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cancelLinkBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cancelLinkBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cancelLinkBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelLinkBtn.FlatAppearance.BorderSize = 0;
            this.cancelLinkBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.cancelLinkBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.cancelLinkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelLinkBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cancelLinkBtn.ForeColor = System.Drawing.Color.White;
            this.cancelLinkBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cancelLinkBtn.Location = new System.Drawing.Point(250, 103);
            this.cancelLinkBtn.Margin = new System.Windows.Forms.Padding(0);
            this.cancelLinkBtn.Name = "cancelLinkBtn";
            this.cancelLinkBtn.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.cancelLinkBtn.Size = new System.Drawing.Size(116, 38);
            this.cancelLinkBtn.TabIndex = 12;
            this.cancelLinkBtn.Text = "Отменить";
            this.cancelLinkBtn.UseVisualStyleBackColor = false;
            // 
            // createLinkBtn
            // 
            this.createLinkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createLinkBtn.AutoSize = true;
            this.createLinkBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.createLinkBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(119)))), ((int)(((byte)(194)))));
            this.createLinkBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.createLinkBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.createLinkBtn.FlatAppearance.BorderSize = 0;
            this.createLinkBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.createLinkBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.createLinkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createLinkBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.createLinkBtn.ForeColor = System.Drawing.Color.White;
            this.createLinkBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.createLinkBtn.Location = new System.Drawing.Point(118, 103);
            this.createLinkBtn.Margin = new System.Windows.Forms.Padding(0);
            this.createLinkBtn.Name = "createLinkBtn";
            this.createLinkBtn.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.createLinkBtn.Size = new System.Drawing.Size(105, 38);
            this.createLinkBtn.TabIndex = 11;
            this.createLinkBtn.Text = "Создать";
            this.createLinkBtn.UseVisualStyleBackColor = false;
            // 
            // nameNewProjectLabel
            // 
            this.nameNewProjectLabel.AutoSize = true;
            this.nameNewProjectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nameNewProjectLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.nameNewProjectLabel.Location = new System.Drawing.Point(-4, 0);
            this.nameNewProjectLabel.Margin = new System.Windows.Forms.Padding(0);
            this.nameNewProjectLabel.Name = "nameNewProjectLabel";
            this.nameNewProjectLabel.Size = new System.Drawing.Size(210, 20);
            this.nameNewProjectLabel.TabIndex = 6;
            this.nameNewProjectLabel.Text = "Номер страницы привязки";
            // 
            // countInput
            // 
            this.countInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.countInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.countInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.countInput.ForeColor = System.Drawing.Color.White;
            this.countInput.Location = new System.Drawing.Point(0, 22);
            this.countInput.MaxLength = 10;
            this.countInput.Name = "countInput";
            this.countInput.Size = new System.Drawing.Size(400, 26);
            this.countInput.TabIndex = 4;
            this.countInput.Text = "0";
            // 
            // nameInputPanel
            // 
            this.nameInputPanel.Controls.Add(this.nameNewProjectLabel);
            this.nameInputPanel.Controls.Add(this.countInput);
            this.nameInputPanel.Location = new System.Drawing.Point(40, 22);
            this.nameInputPanel.Name = "nameInputPanel";
            this.nameInputPanel.Size = new System.Drawing.Size(400, 60);
            this.nameInputPanel.TabIndex = 13;
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(484, 161);
            this.Controls.Add(this.cancelLinkBtn);
            this.Controls.Add(this.createLinkBtn);
            this.Controls.Add(this.nameInputPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SNAMP";
            this.TopMost = true;
            this.nameInputPanel.ResumeLayout(false);
            this.nameInputPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelLinkBtn;
        private System.Windows.Forms.Button createLinkBtn;
        private System.Windows.Forms.Label nameNewProjectLabel;
        private System.Windows.Forms.TextBox countInput;
        private System.Windows.Forms.Panel nameInputPanel;
    }
}