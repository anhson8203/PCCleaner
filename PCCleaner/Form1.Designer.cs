namespace PCCleaner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cleanTempBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cleanNvidiaBtn = new System.Windows.Forms.Button();
            this.cleanPrefetchBtn = new System.Windows.Forms.Button();
            this.cleanWindowsUpdateBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cleanTempBtn
            // 
            this.cleanTempBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanTempBtn.CausesValidation = false;
            this.cleanTempBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanTempBtn, "cleanTempBtn");
            this.cleanTempBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanTempBtn.Name = "cleanTempBtn";
            this.cleanTempBtn.UseVisualStyleBackColor = false;
            this.cleanTempBtn.Click += new System.EventHandler(this.CleanTempFolder);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cleanNvidiaBtn
            // 
            this.cleanNvidiaBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanNvidiaBtn.CausesValidation = false;
            this.cleanNvidiaBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanNvidiaBtn, "cleanNvidiaBtn");
            this.cleanNvidiaBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanNvidiaBtn.Name = "cleanNvidiaBtn";
            this.cleanNvidiaBtn.UseVisualStyleBackColor = false;
            this.cleanNvidiaBtn.Click += new System.EventHandler(this.CleanNvidiaCache);
            // 
            // cleanPrefetchBtn
            // 
            this.cleanPrefetchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanPrefetchBtn.CausesValidation = false;
            this.cleanPrefetchBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanPrefetchBtn, "cleanPrefetchBtn");
            this.cleanPrefetchBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanPrefetchBtn.Name = "cleanPrefetchBtn";
            this.cleanPrefetchBtn.UseVisualStyleBackColor = false;
            this.cleanPrefetchBtn.Click += new System.EventHandler(this.CleanPrefetch);
            // 
            // cleanWindowsUpdateBtn
            // 
            this.cleanWindowsUpdateBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanWindowsUpdateBtn.CausesValidation = false;
            this.cleanWindowsUpdateBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanWindowsUpdateBtn, "cleanWindowsUpdateBtn");
            this.cleanWindowsUpdateBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanWindowsUpdateBtn.Name = "cleanWindowsUpdateBtn";
            this.cleanWindowsUpdateBtn.UseVisualStyleBackColor = false;
            this.cleanWindowsUpdateBtn.Click += new System.EventHandler(this.CleanWindowsUpdatePackages);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cleanWindowsUpdateBtn);
            this.Controls.Add(this.cleanPrefetchBtn);
            this.Controls.Add(this.cleanNvidiaBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cleanTempBtn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cleanTempBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cleanNvidiaBtn;
        private System.Windows.Forms.Button cleanPrefetchBtn;
        private System.Windows.Forms.Button cleanWindowsUpdateBtn;
    }
}

