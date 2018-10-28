namespace HelseIdSampleApp
{
    partial class HelseIdEnabler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelseIdEnabler));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btStoreClient = new System.Windows.Forms.Button();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.txtResult);
            this.pnlBottom.Controls.Add(this.btStoreClient);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 605);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1008, 36);
            this.pnlBottom.TabIndex = 0;
            // 
            // btStoreClient
            // 
            this.btStoreClient.Location = new System.Drawing.Point(921, 6);
            this.btStoreClient.Name = "btStoreClient";
            this.btStoreClient.Size = new System.Drawing.Size(75, 23);
            this.btStoreClient.TabIndex = 0;
            this.btStoreClient.Text = "Store Client";
            this.btStoreClient.UseVisualStyleBackColor = true;
            this.btStoreClient.Click += new System.EventHandler(this.btStoreClient_Click);
            // 
            // pnlControls
            // 
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1008, 605);
            this.pnlControls.TabIndex = 1;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(12, 8);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(775, 20);
            this.txtResult.TabIndex = 1;
            this.txtResult.TabStop = false;
            // 
            // HelseIdEnabler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 641);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelseIdEnabler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HelseId Client Registration";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btStoreClient;
        private System.Windows.Forms.TextBox txtResult;
    }
}