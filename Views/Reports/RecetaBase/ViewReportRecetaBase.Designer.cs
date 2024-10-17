namespace OpticaMultivisual.Views.Reports.RecetaBase
{
    partial class ViewReportRecetaBase
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dataSet_ReportRecetaBase = new OpticaMultivisual.Views.Reports.RecetaBase.DataSet_ReportRecetaBase();
            this.dRefractivoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dRefractivoTableAdapter = new OpticaMultivisual.Views.Reports.RecetaBase.DataSet_ReportRecetaBaseTableAdapters.DRefractivoTableAdapter();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_ReportRecetaBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dRefractivoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(289, 12);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 21);
            this.button1.TabIndex = 2;
            this.button1.Text = "Generar reporte";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(61)))), ((int)(((byte)(115)))));
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.comboBox1);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(915, 50);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 15, 10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Categoría del libro:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(119, 12);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(164, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.dRefractivoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "OpticaMultivisual.Views.Reports.RecetaBase.ReportGeneralRecetaBase.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 50);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(915, 494);
            this.reportViewer1.TabIndex = 2;
            // 
            // dataSet_ReportRecetaBase
            // 
            this.dataSet_ReportRecetaBase.DataSetName = "DataSet_ReportRecetaBase";
            this.dataSet_ReportRecetaBase.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dRefractivoBindingSource
            // 
            this.dRefractivoBindingSource.DataMember = "DRefractivo";
            this.dRefractivoBindingSource.DataSource = this.dataSet_ReportRecetaBase;
            // 
            // dRefractivoTableAdapter
            // 
            this.dRefractivoTableAdapter.ClearBeforeFill = true;
            // 
            // ViewReportRecetaBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 544);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ViewReportRecetaBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de Receta Base";
            this.Load += new System.EventHandler(this.ViewReportRecetaBase_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_ReportRecetaBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dRefractivoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DataSet_ReportRecetaBase dataSet_ReportRecetaBase;
        private System.Windows.Forms.BindingSource dRefractivoBindingSource;
        private DataSet_ReportRecetaBaseTableAdapters.DRefractivoTableAdapter dRefractivoTableAdapter;
    }
}