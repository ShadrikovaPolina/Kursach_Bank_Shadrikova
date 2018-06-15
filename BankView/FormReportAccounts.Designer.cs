namespace BankView
{
    partial class FormReportAccounts
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
            this.buttonPdf = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // buttonPdf
            // 
            this.buttonPdf.Location = new System.Drawing.Point(553, 0);
            this.buttonPdf.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPdf.Name = "buttonPdf";
            this.buttonPdf.Size = new System.Drawing.Size(87, 23);
            this.buttonPdf.TabIndex = 18;
            this.buttonPdf.Text = "В Pdf";
            this.buttonPdf.UseVisualStyleBackColor = true;
            this.buttonPdf.Click += new System.EventHandler(this.buttonPdf_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(381, 0);
            this.buttonCreate.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(110, 23);
            this.buttonCreate.TabIndex = 17;
            this.buttonCreate.Text = "Сформировать";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(206, 3);
            this.dateTimePickerTo.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(151, 20);
            this.dateTimePickerTo.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "по";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "C";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(28, 3);
            this.dateTimePickerFrom.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(151, 20);
            this.dateTimePickerFrom.TabIndex = 13;
            // 
            // reportViewer
            // 
            this.reportViewer.LocalReport.ReportEmbeddedResource = "BankView.ReportAccounts.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(1, 27);
            this.reportViewer.Margin = new System.Windows.Forms.Padding(2);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new System.Drawing.Size(659, 420);
            this.reportViewer.TabIndex = 19;
            // 
            // FormReportAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 450);
            this.Controls.Add(this.reportViewer);
            this.Controls.Add(this.buttonPdf);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Name = "FormReportAccounts";
            this.Text = "Отчёт по расчётам";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPdf;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
    }
}