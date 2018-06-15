namespace BankView
{
    partial class FormAccount
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
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxXls = new System.Windows.Forms.CheckBox();
            this.checkBoxDoc = new System.Windows.Forms.CheckBox();
            this.textBoxMail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(50, 41);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(71, 24);
            this.buttonSend.TabIndex = 14;
            this.buttonSend.Text = "Отправить";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(132, 41);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(71, 24);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxXls);
            this.groupBox1.Controls.Add(this.checkBoxDoc);
            this.groupBox1.Location = new System.Drawing.Point(219, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(72, 67);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Формат";
            // 
            // checkBoxXls
            // 
            this.checkBoxXls.AutoSize = true;
            this.checkBoxXls.Location = new System.Drawing.Point(4, 39);
            this.checkBoxXls.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxXls.Name = "checkBoxXls";
            this.checkBoxXls.Size = new System.Drawing.Size(41, 17);
            this.checkBoxXls.TabIndex = 1;
            this.checkBoxXls.Text = ".xls";
            this.checkBoxXls.UseVisualStyleBackColor = true;
            // 
            // checkBoxDoc
            // 
            this.checkBoxDoc.AutoSize = true;
            this.checkBoxDoc.Location = new System.Drawing.Point(4, 17);
            this.checkBoxDoc.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxDoc.Name = "checkBoxDoc";
            this.checkBoxDoc.Size = new System.Drawing.Size(47, 17);
            this.checkBoxDoc.TabIndex = 0;
            this.checkBoxDoc.Text = ".doc";
            this.checkBoxDoc.UseVisualStyleBackColor = true;
            // 
            // textBoxMail
            // 
            this.textBoxMail.Location = new System.Drawing.Point(50, 7);
            this.textBoxMail.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMail.Name = "textBoxMail";
            this.textBoxMail.Size = new System.Drawing.Size(165, 20);
            this.textBoxMail.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Email:";
            // 
            // FormAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 81);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxMail);
            this.Controls.Add(this.label1);
            this.Name = "FormAccount";
            this.Text = "Рассчитать сотрудников";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxXls;
        private System.Windows.Forms.CheckBox checkBoxDoc;
        private System.Windows.Forms.TextBox textBoxMail;
        private System.Windows.Forms.Label label1;
    }
}