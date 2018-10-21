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
            this.cellNameField = new System.Windows.Forms.TextBox();
            this.cellContentsField = new System.Windows.Forms.TextBox();
            this.setButton = new System.Windows.Forms.Button();
            this.cellValueField = new System.Windows.Forms.TextBox();
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
            // cellNameField
            // 
            this.cellNameField.Location = new System.Drawing.Point(28, 8);
            this.cellNameField.Name = "cellNameField";
            this.cellNameField.ReadOnly = true;
            this.cellNameField.Size = new System.Drawing.Size(146, 22);
            this.cellNameField.TabIndex = 1;
            this.cellNameField.TabStop = false;
            this.cellNameField.Text = "A1";
            // 
            // cellContentsField
            // 
            this.cellContentsField.Location = new System.Drawing.Point(332, 8);
            this.cellContentsField.Name = "cellContentsField";
            this.cellContentsField.Size = new System.Drawing.Size(146, 22);
            this.cellContentsField.TabIndex = 3;
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(484, 7);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 4;
            this.setButton.Text = "Set Cell";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // cellValueField
            // 
            this.cellValueField.Location = new System.Drawing.Point(180, 8);
            this.cellValueField.Name = "cellValueField";
            this.cellValueField.ReadOnly = true;
            this.cellValueField.Size = new System.Drawing.Size(146, 22);
            this.cellValueField.TabIndex = 5;
            this.cellValueField.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cellValueField);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.cellContentsField);
            this.Controls.Add(this.cellNameField);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox cellNameField;
        private System.Windows.Forms.TextBox cellContentsField;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.TextBox cellValueField;
    }
}

