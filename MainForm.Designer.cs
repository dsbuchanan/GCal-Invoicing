namespace GCal_Invoicing
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.displayText = new System.Windows.Forms.TextBox();
            this.monthCalendarDates = new System.Windows.Forms.MonthCalendar();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.buttonGetInvoices = new System.Windows.Forms.Button();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.downloadsEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.invoiceCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.buttonCreateInvoices = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // displayText
            // 
            this.displayText.Location = new System.Drawing.Point(12, 227);
            this.displayText.Multiline = true;
            this.displayText.Name = "displayText";
            this.displayText.ReadOnly = true;
            this.displayText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.displayText.Size = new System.Drawing.Size(776, 350);
            this.displayText.TabIndex = 2;
            this.displayText.TextChanged += new System.EventHandler(this.displayText_TextChanged);
            // 
            // monthCalendarDates
            // 
            this.monthCalendarDates.Location = new System.Drawing.Point(12, 33);
            this.monthCalendarDates.MaxSelectionCount = 28;
            this.monthCalendarDates.Name = "monthCalendarDates";
            this.monthCalendarDates.TabIndex = 3;
            this.monthCalendarDates.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendarDates_DateSelected);
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(12, 9);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(72, 15);
            this.labelStartDate.TabIndex = 4;
            this.labelStartDate.Text = "Select dates:";
            // 
            // buttonGetInvoices
            // 
            this.buttonGetInvoices.Location = new System.Drawing.Point(244, 33);
            this.buttonGetInvoices.Name = "buttonGetInvoices";
            this.buttonGetInvoices.Size = new System.Drawing.Size(75, 40);
            this.buttonGetInvoices.TabIndex = 7;
            this.buttonGetInvoices.Text = "Get Invoices";
            this.buttonGetInvoices.UseVisualStyleBackColor = true;
            this.buttonGetInvoices.Click += new System.EventHandler(this.buttonGetShifts_Click);
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Location = new System.Drawing.Point(713, 583);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 8;
            this.buttonClearLog.Text = "Clear Log";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // downloadsEnabledCheckBox
            // 
            this.downloadsEnabledCheckBox.AutoSize = true;
            this.downloadsEnabledCheckBox.Checked = true;
            this.downloadsEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadsEnabledCheckBox.Location = new System.Drawing.Point(625, 12);
            this.downloadsEnabledCheckBox.Name = "downloadsEnabledCheckBox";
            this.downloadsEnabledCheckBox.Size = new System.Drawing.Size(163, 19);
            this.downloadsEnabledCheckBox.TabIndex = 9;
            this.downloadsEnabledCheckBox.Text = "Download as PDF enabled";
            this.downloadsEnabledCheckBox.UseVisualStyleBackColor = true;
            this.downloadsEnabledCheckBox.CheckedChanged += new System.EventHandler(this.downloadsEnabledCheckbox_CheckedChanged);
            // 
            // invoiceCheckedListBox
            // 
            this.invoiceCheckedListBox.CheckOnClick = true;
            this.invoiceCheckedListBox.FormattingEnabled = true;
            this.invoiceCheckedListBox.Location = new System.Drawing.Point(360, 33);
            this.invoiceCheckedListBox.Name = "invoiceCheckedListBox";
            this.invoiceCheckedListBox.Size = new System.Drawing.Size(347, 166);
            this.invoiceCheckedListBox.TabIndex = 10;
            // 
            // buttonCreateInvoices
            // 
            this.buttonCreateInvoices.Enabled = false;
            this.buttonCreateInvoices.Location = new System.Drawing.Point(713, 33);
            this.buttonCreateInvoices.Name = "buttonCreateInvoices";
            this.buttonCreateInvoices.Size = new System.Drawing.Size(75, 40);
            this.buttonCreateInvoices.TabIndex = 11;
            this.buttonCreateInvoices.Text = "Create Invoices";
            this.buttonCreateInvoices.UseVisualStyleBackColor = true;
            this.buttonCreateInvoices.Click += new System.EventHandler(this.buttonCreateInvoices_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 625);
            this.Controls.Add(this.buttonCreateInvoices);
            this.Controls.Add(this.invoiceCheckedListBox);
            this.Controls.Add(this.downloadsEnabledCheckBox);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.buttonGetInvoices);
            this.Controls.Add(this.labelStartDate);
            this.Controls.Add(this.monthCalendarDates);
            this.Controls.Add(this.displayText);
            this.Name = "MainForm";
            this.Text = "GCal-Invoicing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox displayText;
        private MonthCalendar monthCalendarDates;
        private Label labelStartDate;
        private Button buttonGetInvoices;
        private Button buttonClearLog;
        private CheckBox downloadsEnabledCheckBox;
        private CheckedListBox invoiceCheckedListBox;
        private Button buttonCreateInvoices;
    }
}