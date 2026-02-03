namespace PulseNetworkMonitor
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            pulseNetworkMonitorToolStripMenuItem = new ToolStripMenuItem();
            scanToolStripMenuItem = new ToolStripMenuItem();
            devicesToolStripMenuItem = new ToolStripMenuItem();
            consoleToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            scanToolStripMenuItem1 = new ToolStripMenuItem();
            devicesToolStripMenuItem1 = new ToolStripMenuItem();
            consoleToolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            richTextBox1 = new RichTextBox();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            scanBtn = new Button();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            label2 = new Label();
            clearBtn = new Button();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Pulse Network Monitor";
            notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { pulseNetworkMonitorToolStripMenuItem, scanToolStripMenuItem, devicesToolStripMenuItem, consoleToolStripMenuItem, settingsToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(230, 148);
            // 
            // pulseNetworkMonitorToolStripMenuItem
            // 
            pulseNetworkMonitorToolStripMenuItem.AutoSize = false;
            pulseNetworkMonitorToolStripMenuItem.Enabled = false;
            pulseNetworkMonitorToolStripMenuItem.Name = "pulseNetworkMonitorToolStripMenuItem";
            pulseNetworkMonitorToolStripMenuItem.Size = new Size(229, 24);
            pulseNetworkMonitorToolStripMenuItem.Text = "Pulse Network Monitor";
            // 
            // scanToolStripMenuItem
            // 
            scanToolStripMenuItem.Name = "scanToolStripMenuItem";
            scanToolStripMenuItem.Size = new Size(229, 24);
            scanToolStripMenuItem.Text = "Scan";
            scanToolStripMenuItem.Click += scanToolStripMenuItem_Click;
            // 
            // devicesToolStripMenuItem
            // 
            devicesToolStripMenuItem.Name = "devicesToolStripMenuItem";
            devicesToolStripMenuItem.Size = new Size(229, 24);
            devicesToolStripMenuItem.Text = "Devices";
            devicesToolStripMenuItem.Click += devicesToolStripMenuItem_Click;
            // 
            // consoleToolStripMenuItem
            // 
            consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            consoleToolStripMenuItem.Size = new Size(229, 24);
            consoleToolStripMenuItem.Text = "Console";
            consoleToolStripMenuItem.Click += consoleToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(229, 24);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(229, 24);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(432, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { scanToolStripMenuItem1, devicesToolStripMenuItem1, consoleToolStripMenuItem1, exitToolStripMenuItem1 });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // scanToolStripMenuItem1
            // 
            scanToolStripMenuItem1.Name = "scanToolStripMenuItem1";
            scanToolStripMenuItem1.Size = new Size(145, 26);
            scanToolStripMenuItem1.Text = "Scan";
            scanToolStripMenuItem1.Click += scanToolStripMenuItem1_Click;
            // 
            // devicesToolStripMenuItem1
            // 
            devicesToolStripMenuItem1.Name = "devicesToolStripMenuItem1";
            devicesToolStripMenuItem1.Size = new Size(145, 26);
            devicesToolStripMenuItem1.Text = "Devices";
            devicesToolStripMenuItem1.Click += devicesToolStripMenuItem1_Click;
            // 
            // consoleToolStripMenuItem1
            // 
            consoleToolStripMenuItem1.Name = "consoleToolStripMenuItem1";
            consoleToolStripMenuItem1.Size = new Size(145, 26);
            consoleToolStripMenuItem1.Text = "Console";
            consoleToolStripMenuItem1.Click += consoleToolStripMenuItem1_Click;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(145, 26);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += exitToolStripMenuItem1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(13, 32);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new Size(404, 190);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(13, 237);
            numericUpDown1.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(77, 27);
            numericUpDown1.TabIndex = 3;
            numericUpDown1.Value = new decimal(new int[] { 64, 0, 0, 0 });
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(13, 276);
            numericUpDown2.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(77, 27);
            numericUpDown2.TabIndex = 4;
            numericUpDown2.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // scanBtn
            // 
            scanBtn.Location = new Point(223, 278);
            scanBtn.Name = "scanBtn";
            scanBtn.Size = new Size(94, 29);
            scanBtn.TabIndex = 5;
            scanBtn.Text = "Scan";
            scanBtn.UseVisualStyleBackColor = true;
            scanBtn.Click += scanBtn_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(223, 256);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(194, 16);
            progressBar1.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(96, 239);
            label1.Name = "label1";
            label1.Size = new Size(32, 20);
            label1.TabIndex = 7;
            label1.Text = "TTL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(96, 278);
            label2.Name = "label2";
            label2.Size = new Size(94, 20);
            label2.TabIndex = 8;
            label2.Text = "Packet Count";
            // 
            // clearBtn
            // 
            clearBtn.Location = new Point(323, 278);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(94, 29);
            clearBtn.TabIndex = 9;
            clearBtn.Text = "Clear";
            clearBtn.UseVisualStyleBackColor = true;
            clearBtn.Click += clearBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(432, 317);
            Controls.Add(clearBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(scanBtn);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(richTextBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem devicesToolStripMenuItem;
        private ToolStripMenuItem consoleToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem pulseNetworkMonitorToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem scanToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem scanToolStripMenuItem1;
        private ToolStripMenuItem devicesToolStripMenuItem1;
        private ToolStripMenuItem consoleToolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private RichTextBox richTextBox1;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Button scanBtn;
        private ProgressBar progressBar1;
        private Label label1;
        private Label label2;
        private Button clearBtn;
    }
}
