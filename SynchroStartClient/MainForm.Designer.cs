namespace SynchroStartClient
{
    partial class MainForm
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
            this.checkReady = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStatusTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelTimer = new System.Windows.Forms.Label();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.textNick = new System.Windows.Forms.TextBox();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkReady
            // 
            this.checkReady.AutoSize = true;
            this.checkReady.Location = new System.Drawing.Point(244, 127);
            this.checkReady.Name = "checkReady";
            this.checkReady.Size = new System.Drawing.Size(66, 17);
            this.checkReady.TabIndex = 15;
            this.checkReady.Text = "Ready ?";
            this.checkReady.UseVisualStyleBackColor = true;
            this.checkReady.CheckedChanged += new System.EventHandler(this.checkReady_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "In-game nick:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(125, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Countdown";
            // 
            // toolStatusTitle
            // 
            this.toolStatusTitle.Name = "toolStatusTitle";
            this.toolStatusTitle.Size = new System.Drawing.Size(0, 17);
            // 
            // labelTimer
            // 
            this.labelTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTimer.Location = new System.Drawing.Point(12, 4);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(290, 118);
            this.labelTimer.TabIndex = 11;
            this.labelTimer.Text = "00:00";
            this.labelTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatusTitle,
            this.toolStatusText});
            this.statusBar.Location = new System.Drawing.Point(0, 167);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(333, 22);
            this.statusBar.TabIndex = 10;
            this.statusBar.Text = "statusStrip1";
            // 
            // toolStatusText
            // 
            this.toolStatusText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStatusText.ForeColor = System.Drawing.Color.Green;
            this.toolStatusText.Name = "toolStatusText";
            this.toolStatusText.Size = new System.Drawing.Size(78, 17);
            this.toolStatusText.Text = "Connecting...";
            // 
            // textNick
            // 
            this.textNick.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SynchroStartClient.Properties.Settings.Default, "nickName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textNick.Location = new System.Drawing.Point(89, 125);
            this.textNick.Name = "textNick";
            this.textNick.Size = new System.Drawing.Size(149, 20);
            this.textNick.TabIndex = 13;
            this.textNick.Text = global::SynchroStartClient.Properties.Settings.Default.nickName;
            this.textNick.TextChanged += new System.EventHandler(this.textNick_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 189);
            this.Controls.Add(this.checkReady);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textNick);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTimer);
            this.Controls.Add(this.statusBar);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkReady;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textNick;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusTitle;
        private System.Windows.Forms.Label labelTimer;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusText;
    }
}

