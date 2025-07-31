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
            this.cleanShaderBtn = new System.Windows.Forms.Button();
            this.cleanDiscordBtn = new System.Windows.Forms.Button();
            this.cleanSteamBtn = new System.Windows.Forms.Button();
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
            this.cleanTempBtn.Click += new System.EventHandler(this.ClearWindows);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cleanShaderBtn
            // 
            this.cleanShaderBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanShaderBtn.CausesValidation = false;
            this.cleanShaderBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanShaderBtn, "cleanShaderBtn");
            this.cleanShaderBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanShaderBtn.Name = "cleanShaderBtn";
            this.cleanShaderBtn.UseVisualStyleBackColor = false;
            this.cleanShaderBtn.Click += new System.EventHandler(this.ClearShader);
            // 
            // cleanDiscordBtn
            // 
            this.cleanDiscordBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanDiscordBtn.CausesValidation = false;
            this.cleanDiscordBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanDiscordBtn, "cleanDiscordBtn");
            this.cleanDiscordBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanDiscordBtn.Name = "cleanDiscordBtn";
            this.cleanDiscordBtn.UseVisualStyleBackColor = false;
            this.cleanDiscordBtn.Click += new System.EventHandler(this.ClearDiscord);
            // 
            // cleanSteamBtn
            // 
            this.cleanSteamBtn.BackColor = System.Drawing.SystemColors.Window;
            this.cleanSteamBtn.CausesValidation = false;
            this.cleanSteamBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.cleanSteamBtn, "cleanSteamBtn");
            this.cleanSteamBtn.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cleanSteamBtn.Name = "cleanSteamBtn";
            this.cleanSteamBtn.UseVisualStyleBackColor = false;
            this.cleanSteamBtn.Click += new System.EventHandler(this.ClearSteam);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cleanSteamBtn);
            this.Controls.Add(this.cleanDiscordBtn);
            this.Controls.Add(this.cleanShaderBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cleanTempBtn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button cleanSteamBtn;

        private System.Windows.Forms.Button cleanDiscordBtn;

        #endregion
        private System.Windows.Forms.Button cleanTempBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cleanShaderBtn;
    }
}

