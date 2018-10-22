namespace WindowsFormsApp1
{
    partial class Form1
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
            this.cellNameField = new System.Windows.Forms.TextBox();
            this.cellContentsField = new System.Windows.Forms.TextBox();
            this.setButton = new System.Windows.Forms.Button();
            this.cellValueField = new System.Windows.Forms.TextBox();
            this.fileMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.combineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aNDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.navigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savingAndOpeningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.combiningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cellNameField
            // 
            this.cellNameField.Enabled = false;
            this.cellNameField.Location = new System.Drawing.Point(12, 31);
            this.cellNameField.Name = "cellNameField";
            this.cellNameField.ReadOnly = true;
            this.cellNameField.Size = new System.Drawing.Size(146, 22);
            this.cellNameField.TabIndex = 1;
            this.cellNameField.TabStop = false;
            this.cellNameField.Text = "A1";
            // 
            // cellContentsField
            // 
            this.cellContentsField.Location = new System.Drawing.Point(316, 31);
            this.cellContentsField.Name = "cellContentsField";
            this.cellContentsField.Size = new System.Drawing.Size(146, 22);
            this.cellContentsField.TabIndex = 3;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(468, 30);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 4;
            this.setButton.Text = "Set Cell";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // cellValueField
            // 
            this.cellValueField.Enabled = false;
            this.cellValueField.Location = new System.Drawing.Point(164, 31);
            this.cellValueField.Name = "cellValueField";
            this.cellValueField.ReadOnly = true;
            this.cellValueField.Size = new System.Drawing.Size(146, 22);
            this.cellValueField.TabIndex = 5;
            this.cellValueField.TabStop = false;
            // 
            // fileMenuStrip
            // 
            this.fileMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.fileMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.fileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.combineToolStripMenuItem});
            this.fileMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.fileMenuStrip.Name = "fileMenuStrip";
            this.fileMenuStrip.Size = new System.Drawing.Size(951, 28);
            this.fileMenuStrip.TabIndex = 6;
            this.fileMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.saveToolStripMenuItem.Text = "Save As";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigationToolStripMenuItem,
            this.savingAndOpeningToolStripMenuItem,
            this.combiningToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 57);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(948, 457);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // combineToolStripMenuItem
            // 
            this.combineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aNDToolStripMenuItem,
            this.oRToolStripMenuItem});
            this.combineToolStripMenuItem.Name = "combineToolStripMenuItem";
            this.combineToolStripMenuItem.Size = new System.Drawing.Size(173, 24);
            this.combineToolStripMenuItem.Text = "Combine Spreadsheets";
            // 
            // aNDToolStripMenuItem
            // 
            this.aNDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem});
            this.aNDToolStripMenuItem.Name = "aNDToolStripMenuItem";
            this.aNDToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.aNDToolStripMenuItem.Text = "AND";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // oRToolStripMenuItem
            // 
            this.oRToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem1});
            this.oRToolStripMenuItem.Name = "oRToolStripMenuItem";
            this.oRToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.oRToolStripMenuItem.Text = "OR";
            // 
            // openFileToolStripMenuItem1
            // 
            this.openFileToolStripMenuItem1.Name = "openFileToolStripMenuItem1";
            this.openFileToolStripMenuItem1.Size = new System.Drawing.Size(216, 26);
            this.openFileToolStripMenuItem1.Text = "Open File";
            this.openFileToolStripMenuItem1.Click += new System.EventHandler(this.openFileToolStripMenuItem1_Click);
            // 
            // navigationToolStripMenuItem
            // 
            this.navigationToolStripMenuItem.Name = "navigationToolStripMenuItem";
            this.navigationToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
            this.navigationToolStripMenuItem.Text = "Navigation";
            this.navigationToolStripMenuItem.Click += new System.EventHandler(this.navigationToolStripMenuItem_Click);
            // 
            // savingAndOpeningToolStripMenuItem
            // 
            this.savingAndOpeningToolStripMenuItem.Name = "savingAndOpeningToolStripMenuItem";
            this.savingAndOpeningToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
            this.savingAndOpeningToolStripMenuItem.Text = "Saving and Opening";
            this.savingAndOpeningToolStripMenuItem.Click += new System.EventHandler(this.savingAndOpeningToolStripMenuItem_Click);
            // 
            // combiningToolStripMenuItem
            // 
            this.combiningToolStripMenuItem.Name = "combiningToolStripMenuItem";
            this.combiningToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
            this.combiningToolStripMenuItem.Text = "Combining";
            this.combiningToolStripMenuItem.Click += new System.EventHandler(this.combiningToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 526);
            this.Controls.Add(this.cellValueField);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.cellContentsField);
            this.Controls.Add(this.cellNameField);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.fileMenuStrip);
            this.MainMenuStrip = this.fileMenuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.fileMenuStrip.ResumeLayout(false);
            this.fileMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox cellNameField;
        private System.Windows.Forms.TextBox cellContentsField;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.TextBox cellValueField;
        private System.Windows.Forms.MenuStrip fileMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem combineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aNDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem navigationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savingAndOpeningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem combiningToolStripMenuItem;
    }
}