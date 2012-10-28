namespace DataMiningTeam.WindowsForms
{
    partial class DMForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMForm));
            this.AprioriButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConfidence = new System.Windows.Forms.TextBox();
            this.txtSupport = new System.Windows.Forms.TextBox();
            this.AddTransactionButton = new System.Windows.Forms.Button();
            this.TransactionsListView = new System.Windows.Forms.ListView();
            this.ResultsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AprioriButton
            // 
            this.AprioriButton.Location = new System.Drawing.Point(12, 180);
            this.AprioriButton.Name = "AprioriButton";
            this.AprioriButton.Size = new System.Drawing.Size(79, 23);
            this.AprioriButton.TabIndex = 0;
            this.AprioriButton.Text = "Apriori";
            this.AprioriButton.UseVisualStyleBackColor = true;
            this.AprioriButton.Click += new System.EventHandler(this.AprioriButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(97, 209);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Bonus 1";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 44);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 26);
            this.button3.TabIndex = 2;
            this.button3.Text = "View Data File";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(97, 238);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 26);
            this.button4.TabIndex = 3;
            this.button4.Text = "Bonus 2";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(566, 374);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "Clear";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.Location = new System.Drawing.Point(647, 374);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(75, 23);
            this.ExecuteButton.TabIndex = 5;
            this.ExecuteButton.Text = "Execute";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            this.ExecuteButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(641, 14);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 6;
            this.button7.Text = "Help";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(12, 238);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(79, 26);
            this.button8.TabIndex = 7;
            this.button8.Text = "Eclat";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(12, 209);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(79, 23);
            this.button9.TabIndex = 8;
            this.button9.Text = "FPGrowth";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(12, 12);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 26);
            this.button10.TabIndex = 9;
            this.button10.Text = "Data Source";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(328, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Results";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Minimum Support Percentage";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Minimum Confidence Percentage";
            // 
            // txtConfidence
            // 
            this.txtConfidence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfidence.Location = new System.Drawing.Point(195, 129);
            this.txtConfidence.Name = "txtConfidence";
            this.txtConfidence.Size = new System.Drawing.Size(100, 20);
            this.txtConfidence.TabIndex = 14;
            // 
            // txtSupport
            // 
            this.txtSupport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupport.Location = new System.Drawing.Point(195, 99);
            this.txtSupport.Name = "txtSupport";
            this.txtSupport.Size = new System.Drawing.Size(100, 20);
            this.txtSupport.TabIndex = 15;
            // 
            // AddTransactionButton
            // 
            this.AddTransactionButton.Location = new System.Drawing.Point(12, 319);
            this.AddTransactionButton.Name = "AddTransactionButton";
            this.AddTransactionButton.Size = new System.Drawing.Size(163, 23);
            this.AddTransactionButton.TabIndex = 16;
            this.AddTransactionButton.Text = "Add Transactions";
            this.AddTransactionButton.UseVisualStyleBackColor = true;
            this.AddTransactionButton.Click += new System.EventHandler(this.AddTransactionButton_Click);
            // 
            // TransactionsListView
            // 
            this.TransactionsListView.Location = new System.Drawing.Point(331, 76);
            this.TransactionsListView.Name = "TransactionsListView";
            this.TransactionsListView.Size = new System.Drawing.Size(363, 116);
            this.TransactionsListView.TabIndex = 17;
            this.TransactionsListView.UseCompatibleStateImageBehavior = false;
            // 
            // ResultsTextBox
            // 
            this.ResultsTextBox.Location = new System.Drawing.Point(331, 215);
            this.ResultsTextBox.Multiline = true;
            this.ResultsTextBox.Name = "ResultsTextBox";
            this.ResultsTextBox.Size = new System.Drawing.Size(363, 113);
            this.ResultsTextBox.TabIndex = 18;
            // 
            // DMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(744, 409);
            this.Controls.Add(this.ResultsTextBox);
            this.Controls.Add(this.TransactionsListView);
            this.Controls.Add(this.AddTransactionButton);
            this.Controls.Add(this.txtSupport);
            this.Controls.Add(this.txtConfidence);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.ExecuteButton);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.AprioriButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DMForm";
            this.Text = "Data Mining Team Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AprioriButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button ExecuteButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConfidence;
        private System.Windows.Forms.TextBox txtSupport;
        private System.Windows.Forms.Button AddTransactionButton;
        private System.Windows.Forms.ListView TransactionsListView;
        private System.Windows.Forms.TextBox ResultsTextBox;


    }

  
}