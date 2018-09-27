namespace Lab6
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
            this.Compute_Button = new System.Windows.Forms.Button();
            this.Bill_label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Percent_Label = new System.Windows.Forms.Label();
            this.PercentageInput = new System.Windows.Forms.TextBox();
            this.Total_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Compute_Button
            // 
            this.Compute_Button.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Compute_Button.Location = new System.Drawing.Point(52, 146);
            this.Compute_Button.Name = "Compute_Button";
            this.Compute_Button.Size = new System.Drawing.Size(164, 53);
            this.Compute_Button.TabIndex = 0;
            this.Compute_Button.Text = "Compute Tip";
            this.Compute_Button.UseVisualStyleBackColor = false;
            this.Compute_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // Bill_label
            // 
            this.Bill_label.AutoSize = true;
            this.Bill_label.Location = new System.Drawing.Point(49, 70);
            this.Bill_label.Name = "Bill_label";
            this.Bill_label.Size = new System.Drawing.Size(104, 17);
            this.Bill_label.TabIndex = 1;
            this.Bill_label.Text = "Enter Bill Total:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(259, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(259, 146);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 3;
            // 
            // Percent_Label
            // 
            this.Percent_Label.AutoSize = true;
            this.Percent_Label.Location = new System.Drawing.Point(49, 49);
            this.Percent_Label.Name = "Percent_Label";
            this.Percent_Label.Size = new System.Drawing.Size(147, 17);
            this.Percent_Label.TabIndex = 4;
            this.Percent_Label.Text = "Enter Tip Percentage:";
            // 
            // PercentageInput
            // 
            this.PercentageInput.Location = new System.Drawing.Point(259, 44);
            this.PercentageInput.Name = "PercentageInput";
            this.PercentageInput.Size = new System.Drawing.Size(100, 22);
            this.PercentageInput.TabIndex = 5;
            this.PercentageInput.TextChanged += new System.EventHandler(this.PercentageInput_TextChanged);
            // 
            // Total_Label
            // 
            this.Total_Label.AutoSize = true;
            this.Total_Label.Location = new System.Drawing.Point(256, 227);
            this.Total_Label.Name = "Total_Label";
            this.Total_Label.Size = new System.Drawing.Size(70, 17);
            this.Total_Label.TabIndex = 6;
            this.Total_Label.Text = "Total Bill: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 490);
            this.Controls.Add(this.Total_Label);
            this.Controls.Add(this.PercentageInput);
            this.Controls.Add(this.Percent_Label);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Bill_label);
            this.Controls.Add(this.Compute_Button);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Compute_Button;
        private System.Windows.Forms.Label Bill_label;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label Percent_Label;
        private System.Windows.Forms.TextBox PercentageInput;
        private System.Windows.Forms.Label Total_Label;
    }
}

