namespace Deployment
{
     partial class CForm
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
               System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CForm));
               this.btnClose = new System.Windows.Forms.Button();
               this.label2 = new System.Windows.Forms.Label();
               this.tbSoftware = new System.Windows.Forms.TextBox();
               this.tbData = new System.Windows.Forms.TextBox();
               this.label3 = new System.Windows.Forms.Label();
               this.groupBox1 = new System.Windows.Forms.GroupBox();
               this.label5 = new System.Windows.Forms.Label();
               this.label4 = new System.Windows.Forms.Label();
               this.rbCustom = new System.Windows.Forms.RadioButton();
               this.rbPrivate = new System.Windows.Forms.RadioButton();
               this.rbCommon = new System.Windows.Forms.RadioButton();
               this.btnInstall = new System.Windows.Forms.Button();
               this.timing = new System.Windows.Forms.Timer(this.components);
               this.tbTitle = new System.Windows.Forms.TextBox();
               this.pictureBox1 = new System.Windows.Forms.PictureBox();
               this.cbShortcut = new System.Windows.Forms.CheckBox();
               this.tbVersion = new System.Windows.Forms.TextBox();
               this.groupBox1.SuspendLayout();
               ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
               this.SuspendLayout();
               // 
               // btnClose
               // 
               this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
               this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.btnClose.Location = new System.Drawing.Point(769, 702);
               this.btnClose.Name = "btnClose";
               this.btnClose.Size = new System.Drawing.Size(150, 50);
               this.btnClose.TabIndex = 8;
               this.btnClose.Text = "Close";
               this.btnClose.UseVisualStyleBackColor = true;
               this.btnClose.Click += new System.EventHandler(this.Event_ClickClose);
               // 
               // label2
               // 
               this.label2.AutoSize = true;
               this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.label2.Location = new System.Drawing.Point(430, 154);
               this.label2.Name = "label2";
               this.label2.Size = new System.Drawing.Size(367, 25);
               this.label2.TabIndex = 0;
               this.label2.Text = "Executable files will be placed here...";
               // 
               // tbSoftware
               // 
               this.tbSoftware.BackColor = System.Drawing.Color.LightSteelBlue;
               this.tbSoftware.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.tbSoftware.Location = new System.Drawing.Point(435, 184);
               this.tbSoftware.Name = "tbSoftware";
               this.tbSoftware.ReadOnly = true;
               this.tbSoftware.Size = new System.Drawing.Size(672, 31);
               this.tbSoftware.TabIndex = 0;
               this.tbSoftware.TabStop = false;
               // 
               // tbData
               // 
               this.tbData.BackColor = System.Drawing.Color.LightSteelBlue;
               this.tbData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.tbData.Location = new System.Drawing.Point(29, 195);
               this.tbData.Name = "tbData";
               this.tbData.ReadOnly = true;
               this.tbData.Size = new System.Drawing.Size(609, 31);
               this.tbData.TabIndex = 5;
               this.tbData.TextChanged += new System.EventHandler(this.Event_TextChanged);
               // 
               // label3
               // 
               this.label3.AutoSize = true;
               this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.label3.Location = new System.Drawing.Point(24, 165);
               this.label3.Name = "label3";
               this.label3.Size = new System.Drawing.Size(251, 25);
               this.label3.TabIndex = 0;
               this.label3.Text = "Application\'s Data Folder";
               // 
               // groupBox1
               // 
               this.groupBox1.Controls.Add(this.label5);
               this.groupBox1.Controls.Add(this.label4);
               this.groupBox1.Controls.Add(this.rbCustom);
               this.groupBox1.Controls.Add(this.rbPrivate);
               this.groupBox1.Controls.Add(this.rbCommon);
               this.groupBox1.Controls.Add(this.label3);
               this.groupBox1.Controls.Add(this.tbData);
               this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.groupBox1.Location = new System.Drawing.Point(435, 238);
               this.groupBox1.Name = "groupBox1";
               this.groupBox1.Size = new System.Drawing.Size(672, 332);
               this.groupBox1.TabIndex = 7;
               this.groupBox1.TabStop = false;
               this.groupBox1.Text = "Data Storage Folder";
               // 
               // label5
               // 
               this.label5.AutoSize = true;
               this.label5.Location = new System.Drawing.Point(26, 272);
               this.label5.Name = "label5";
               this.label5.Size = new System.Drawing.Size(191, 25);
               this.label5.TabIndex = 0;
               this.label5.Text = "for technical support.";
               // 
               // label4
               // 
               this.label4.AutoSize = true;
               this.label4.Location = new System.Drawing.Point(24, 248);
               this.label4.Name = "label4";
               this.label4.Size = new System.Drawing.Size(578, 25);
               this.label4.TabIndex = 0;
               this.label4.Text = "This folder will contain user preferences and application logs used";
               // 
               // rbCustom
               // 
               this.rbCustom.AutoSize = true;
               this.rbCustom.Location = new System.Drawing.Point(29, 115);
               this.rbCustom.Name = "rbCustom";
               this.rbCustom.Size = new System.Drawing.Size(206, 29);
               this.rbCustom.TabIndex = 4;
               this.rbCustom.TabStop = true;
               this.rbCustom.Text = "Custom data folder.";
               this.rbCustom.UseVisualStyleBackColor = true;
               this.rbCustom.CheckedChanged += new System.EventHandler(this.Event_RadioButton);
               // 
               // rbPrivate
               // 
               this.rbPrivate.AutoSize = true;
               this.rbPrivate.Location = new System.Drawing.Point(29, 82);
               this.rbPrivate.Name = "rbPrivate";
               this.rbPrivate.Size = new System.Drawing.Size(365, 29);
               this.rbPrivate.TabIndex = 3;
               this.rbPrivate.Text = "Restrict this application to my account.";
               this.rbPrivate.UseVisualStyleBackColor = true;
               this.rbPrivate.CheckedChanged += new System.EventHandler(this.Event_RadioButton);
               // 
               // rbCommon
               // 
               this.rbCommon.AutoSize = true;
               this.rbCommon.Checked = true;
               this.rbCommon.Location = new System.Drawing.Point(29, 46);
               this.rbCommon.Name = "rbCommon";
               this.rbCommon.Size = new System.Drawing.Size(359, 29);
               this.rbCommon.TabIndex = 2;
               this.rbCommon.TabStop = true;
               this.rbCommon.Text = "Share with all users on this computer.";
               this.rbCommon.UseVisualStyleBackColor = true;
               this.rbCommon.CheckedChanged += new System.EventHandler(this.Event_RadioButton);
               // 
               // btnInstall
               // 
               this.btnInstall.Cursor = System.Windows.Forms.Cursors.Hand;
               this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.btnInstall.Location = new System.Drawing.Point(957, 702);
               this.btnInstall.Name = "btnInstall";
               this.btnInstall.Size = new System.Drawing.Size(150, 50);
               this.btnInstall.TabIndex = 7;
               this.btnInstall.Text = "Install";
               this.btnInstall.UseVisualStyleBackColor = true;
               this.btnInstall.Click += new System.EventHandler(this.Event_ClickNext);
               // 
               // timing
               // 
               this.timing.Tick += new System.EventHandler(this.Event_Timer);
               // 
               // tbTitle
               // 
               this.tbTitle.BackColor = System.Drawing.Color.LightSlateGray;
               this.tbTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
               this.tbTitle.Cursor = System.Windows.Forms.Cursors.Default;
               this.tbTitle.Font = new System.Drawing.Font("Candara", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.tbTitle.ForeColor = System.Drawing.Color.White;
               this.tbTitle.Location = new System.Drawing.Point(435, 37);
               this.tbTitle.MaxLength = 128;
               this.tbTitle.Name = "tbTitle";
               this.tbTitle.ReadOnly = true;
               this.tbTitle.ShortcutsEnabled = false;
               this.tbTitle.Size = new System.Drawing.Size(672, 46);
               this.tbTitle.TabIndex = 0;
               this.tbTitle.TabStop = false;
               this.tbTitle.Text = "Software Title";
               // 
               // pictureBox1
               // 
               this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
               this.pictureBox1.Location = new System.Drawing.Point(0, 0);
               this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
               this.pictureBox1.Name = "pictureBox1";
               this.pictureBox1.Size = new System.Drawing.Size(392, 800);
               this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
               this.pictureBox1.TabIndex = 15;
               this.pictureBox1.TabStop = false;
               // 
               // cbShortcut
               // 
               this.cbShortcut.AutoSize = true;
               this.cbShortcut.Checked = true;
               this.cbShortcut.CheckState = System.Windows.Forms.CheckState.Checked;
               this.cbShortcut.Location = new System.Drawing.Point(435, 597);
               this.cbShortcut.Name = "cbShortcut";
               this.cbShortcut.Size = new System.Drawing.Size(248, 29);
               this.cbShortcut.TabIndex = 6;
               this.cbShortcut.Text = "Create Desktop shortcut";
               this.cbShortcut.UseVisualStyleBackColor = true;
               // 
               // tbVersion
               // 
               this.tbVersion.BackColor = System.Drawing.Color.LightSlateGray;
               this.tbVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
               this.tbVersion.Cursor = System.Windows.Forms.Cursors.Default;
               this.tbVersion.Font = new System.Drawing.Font("Candara", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
               this.tbVersion.ForeColor = System.Drawing.Color.White;
               this.tbVersion.Location = new System.Drawing.Point(435, 78);
               this.tbVersion.MaxLength = 128;
               this.tbVersion.Name = "tbVersion";
               this.tbVersion.ReadOnly = true;
               this.tbVersion.ShortcutsEnabled = false;
               this.tbVersion.Size = new System.Drawing.Size(545, 40);
               this.tbVersion.TabIndex = 0;
               this.tbVersion.TabStop = false;
               this.tbVersion.Text = "Version";
               // 
               // CForm
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(168F, 168F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
               this.BackColor = System.Drawing.Color.LightSlateGray;
               this.ClientSize = new System.Drawing.Size(1146, 800);
               this.Controls.Add(this.tbVersion);
               this.Controls.Add(this.cbShortcut);
               this.Controls.Add(this.pictureBox1);
               this.Controls.Add(this.tbTitle);
               this.Controls.Add(this.btnInstall);
               this.Controls.Add(this.groupBox1);
               this.Controls.Add(this.tbSoftware);
               this.Controls.Add(this.label2);
               this.Controls.Add(this.btnClose);
               this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
               this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
               this.MaximizeBox = false;
               this.MinimizeBox = false;
               this.Name = "CForm";
               this.Text = "Application Deployment";
               this.Move += new System.EventHandler(this.Event_FormMoved);
               this.groupBox1.ResumeLayout(false);
               this.groupBox1.PerformLayout();
               ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
               this.ResumeLayout(false);
               this.PerformLayout();

          }

          #endregion

          private System.Windows.Forms.Button btnClose;
          private System.Windows.Forms.Label label2;
          private System.Windows.Forms.TextBox tbSoftware;
          private System.Windows.Forms.TextBox tbData;
          private System.Windows.Forms.Label label3;
          private System.Windows.Forms.GroupBox groupBox1;
          private System.Windows.Forms.RadioButton rbPrivate;
          private System.Windows.Forms.RadioButton rbCommon;
          private System.Windows.Forms.RadioButton rbCustom;
          private System.Windows.Forms.Label label5;
          private System.Windows.Forms.Label label4;
          private System.Windows.Forms.Button btnInstall;
          private System.Windows.Forms.Timer timing;
          private System.Windows.Forms.TextBox tbTitle;
          private System.Windows.Forms.PictureBox pictureBox1;
          private System.Windows.Forms.CheckBox cbShortcut;
          private System.Windows.Forms.TextBox tbVersion;
    }
}

