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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.setButton = new System.Windows.Forms.Button();
            this.cellNameField = new System.Windows.Forms.TextBox();
            this.cellValueField = new System.Windows.Forms.TextBox();
            this.cellContentsField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(12, 53);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(797, 403);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(468, 10);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 24);
            this.setButton.TabIndex = 16;
            this.setButton.Text = "Set Cell";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // cellNameField
            // 
            this.cellNameField.Enabled = false;
            this.cellNameField.Location = new System.Drawing.Point(12, 12);
            this.cellNameField.Name = "cellNameField";
            this.cellNameField.ReadOnly = true;
            this.cellNameField.Size = new System.Drawing.Size(146, 22);
            this.cellNameField.TabIndex = 15;
            this.cellNameField.TabStop = false;
            this.cellNameField.Text = "A1";
            // 
            // cellValueField
            // 
            this.cellValueField.Enabled = false;
            this.cellValueField.Location = new System.Drawing.Point(164, 12);
            this.cellValueField.Name = "cellValueField";
            this.cellValueField.ReadOnly = true;
            this.cellValueField.Size = new System.Drawing.Size(146, 22);
            this.cellValueField.TabIndex = 17;
            this.cellValueField.TabStop = false;
            // 
            // cellContentsField
            // 
            this.cellContentsField.Location = new System.Drawing.Point(316, 12);
            this.cellContentsField.Name = "cellContentsField";
            this.cellContentsField.Size = new System.Drawing.Size(146, 22);
            this.cellContentsField.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.cellNameField);
            this.Controls.Add(this.cellValueField);
            this.Controls.Add(this.cellContentsField);
            this.Controls.Add(this.spreadsheetPanel1);
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "Form1";
            this.Text = "Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.TextBox cellNameField;
        private System.Windows.Forms.TextBox cellValueField;
        private System.Windows.Forms.TextBox cellContentsField;
    }
}

