namespace SpreadsheetGUI
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
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cellTextBox = new System.Windows.Forms.TextBox();
            this.fileMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cellNameField
            // 
            this.cellNameField.Enabled = false;
            this.cellNameField.Location = new System.Drawing.Point(9, 25);
            this.cellNameField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cellNameField.Name = "cellNameField";
            this.cellNameField.ReadOnly = true;
            this.cellNameField.Size = new System.Drawing.Size(110, 20);
            this.cellNameField.TabIndex = 1;
            this.cellNameField.TabStop = false;
            this.cellNameField.Text = "A1";
            // 
            // cellContentsField
            // 
            this.cellContentsField.Location = new System.Drawing.Point(237, 25);
            this.cellContentsField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cellContentsField.Name = "cellContentsField";
            this.cellContentsField.Size = new System.Drawing.Size(110, 20);
            this.cellContentsField.TabIndex = 3;
            this.cellContentsField.TextChanged += new System.EventHandler(this.cellContentsField_TextChanged);
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(351, 24);
            this.setButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(56, 19);
            this.setButton.TabIndex = 4;
            this.setButton.Text = "Set Cell";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // cellValueField
            // 
            this.cellValueField.Enabled = false;
            this.cellValueField.Location = new System.Drawing.Point(123, 25);
            this.cellValueField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cellValueField.Name = "cellValueField";
            this.cellValueField.ReadOnly = true;
            this.cellValueField.Size = new System.Drawing.Size(110, 20);
            this.cellValueField.TabIndex = 5;
            this.cellValueField.TabStop = false;
            // 
            // fileMenuStrip
            // 
            this.fileMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.fileMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.fileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.fileMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.fileMenuStrip.Name = "fileMenuStrip";
            this.fileMenuStrip.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.fileMenuStrip.Size = new System.Drawing.Size(1343, 24);
            this.fileMenuStrip.TabIndex = 6;
            this.fileMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 46);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1341, 503);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(412, 24);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 19);
            this.button1.TabIndex = 7;
            this.button1.Text = "undo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(473, 24);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 19);
            this.button2.TabIndex = 8;
            this.button2.Text = "revert";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cellTextBox
            // 
            this.cellTextBox.Location = new System.Drawing.Point(29, 76);
            this.cellTextBox.Name = "cellTextBox";
            this.cellTextBox.Size = new System.Drawing.Size(80, 20);
            this.cellTextBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 559);
            this.Controls.Add(this.cellTextBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cellValueField);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.cellContentsField);
            this.Controls.Add(this.cellNameField);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.fileMenuStrip);
            this.MainMenuStrip = this.fileMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox cellTextBox;
    }
}