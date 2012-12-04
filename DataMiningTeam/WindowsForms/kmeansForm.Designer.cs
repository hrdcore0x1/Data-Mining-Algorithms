﻿namespace DataMiningTeam.WindowsForms
{
    partial class kmeansForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(kmeansForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bookExampleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtKset = new System.Windows.Forms.TextBox();
            this.KButton = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.adventureWorksEntitiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.adventureWorksEntitiesBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.adventureWorksEntitiesBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.btnreset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookExampleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksEntitiesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksEntitiesBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksEntitiesBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(687, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Random Data";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(460, 71);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtData.Size = new System.Drawing.Size(328, 290);
            this.txtData.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(109, 71);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(296, 290);
            this.dataGridView1.TabIndex = 7;
            // 
            // bookExampleBindingSource
            // 
            this.bookExampleBindingSource.DataMember = "BookExample";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(47, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(271, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(332, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(122, 23);
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "Import Data File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtKset
            // 
            this.txtKset.Location = new System.Drawing.Point(247, 41);
            this.txtKset.Name = "txtKset";
            this.txtKset.Size = new System.Drawing.Size(71, 20);
            this.txtKset.TabIndex = 10;
            // 
            // KButton
            // 
            this.KButton.Location = new System.Drawing.Point(332, 42);
            this.KButton.Name = "KButton";
            this.KButton.Size = new System.Drawing.Size(122, 23);
            this.KButton.TabIndex = 11;
            this.KButton.Text = "Set K = ?";
            this.KButton.UseVisualStyleBackColor = true;
            this.KButton.Click += new System.EventHandler(this.KButton_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(713, 379);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // adventureWorksEntitiesBindingSource
            // 
            this.adventureWorksEntitiesBindingSource.DataSource = typeof(DataMiningTeam.Data.AdventureWorksEntities);
            // 
            // adventureWorksEntitiesBindingSource1
            // 
            this.adventureWorksEntitiesBindingSource1.DataSource = typeof(DataMiningTeam.Data.AdventureWorksEntities);
            // 
            // adventureWorksEntitiesBindingSource2
            // 
            this.adventureWorksEntitiesBindingSource2.DataSource = typeof(DataMiningTeam.Data.AdventureWorksEntities);
            // 
            // btnreset
            // 
            this.btnreset.Location = new System.Drawing.Point(608, 379);
            this.btnreset.Name = "btnreset";
            this.btnreset.Size = new System.Drawing.Size(88, 23);
            this.btnreset.TabIndex = 13;
            this.btnreset.Text = "Reset";
            this.btnreset.UseVisualStyleBackColor = true;
            this.btnreset.Click += new System.EventHandler(this.btnreset_Click);
            // 
            // kmeansForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 414);
            this.Controls.Add(this.btnreset);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.KButton);
            this.Controls.Add(this.txtKset);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "kmeansForm";
            this.Text = "Clustering Kmeans";
            this.Load += new System.EventHandler(this.kmeansForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookExampleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksEntitiesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksEntitiesBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adventureWorksEntitiesBindingSource2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource adventureWorksEntitiesBindingSource;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.BindingSource adventureWorksEntitiesBindingSource1;
        private System.Windows.Forms.BindingSource adventureWorksEntitiesBindingSource2;
        //private AdventureWorksDW2012DataSet adventureWorksDW2012DataSet;
        private System.Windows.Forms.BindingSource bookExampleBindingSource;
       // private AdventureWorksDW2012DataSetTableAdapters.BookExampleTableAdapter bookExampleTableAdapter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtKset;
        private System.Windows.Forms.Button KButton;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnreset;
    }
}