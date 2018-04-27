namespace PreviewHandlerEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.handlerComboBox = new System.Windows.Forms.ComboBox();
            this.extensionsListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.keyTimer = new System.Windows.Forms.Timer(this.components);
            this.showOnlyRegisteredCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.llParentHandler = new System.Windows.Forms.LinkLabel();
            this.llParent = new System.Windows.Forms.LinkLabel();
            this.llHandler = new System.Windows.Forms.LinkLabel();
            this.llExtension = new System.Windows.Forms.LinkLabel();
            this.lblDirectExtensionInfo = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ddbRegLinks = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // handlerComboBox
            // 
            this.handlerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.handlerComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.handlerComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.handlerComboBox.FormattingEnabled = true;
            this.handlerComboBox.Location = new System.Drawing.Point(136, 145);
            this.handlerComboBox.Name = "handlerComboBox";
            this.handlerComboBox.Size = new System.Drawing.Size(467, 21);
            this.handlerComboBox.TabIndex = 0;
            this.handlerComboBox.SelectedIndexChanged += new System.EventHandler(this.handlerComboBox_SelectedIndexChanged);
            // 
            // extensionsListBox
            // 
            this.extensionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionsListBox.FormattingEnabled = true;
            this.extensionsListBox.Location = new System.Drawing.Point(136, 191);
            this.extensionsListBox.Name = "extensionsListBox";
            this.extensionsListBox.Size = new System.Drawing.Size(467, 379);
            this.extensionsListBox.TabIndex = 1;
            this.extensionsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.extensionsListBox_ItemCheck);
            this.extensionsListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.extensionsListBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Registered handlers:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Classes/extensions:";
            // 
            // keyTimer
            // 
            this.keyTimer.Interval = 500;
            this.keyTimer.Tick += new System.EventHandler(this.keyTimer_Tick);
            // 
            // showOnlyRegisteredCheckBox
            // 
            this.showOnlyRegisteredCheckBox.AutoSize = true;
            this.showOnlyRegisteredCheckBox.Checked = true;
            this.showOnlyRegisteredCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOnlyRegisteredCheckBox.Location = new System.Drawing.Point(136, 171);
            this.showOnlyRegisteredCheckBox.Name = "showOnlyRegisteredCheckBox";
            this.showOnlyRegisteredCheckBox.Size = new System.Drawing.Size(246, 17);
            this.showOnlyRegisteredCheckBox.TabIndex = 4;
            this.showOnlyRegisteredCheckBox.Text = "Only show classes associated with this handler";
            this.showOnlyRegisteredCheckBox.UseVisualStyleBackColor = true;
            this.showOnlyRegisteredCheckBox.CheckedChanged += new System.EventHandler(this.showOnlyRegisteredCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.llParentHandler);
            this.groupBox1.Controls.Add(this.llParent);
            this.groupBox1.Controls.Add(this.llHandler);
            this.groupBox1.Controls.Add(this.llExtension);
            this.groupBox1.Controls.Add(this.lblDirectExtensionInfo);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.txtExtension);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(615, 139);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Direct add new Extension:";
            // 
            // llParentHandler
            // 
            this.llParentHandler.AutoSize = true;
            this.llParentHandler.Enabled = false;
            this.llParentHandler.LinkArea = new System.Windows.Forms.LinkArea(6, 3);
            this.llParentHandler.Location = new System.Drawing.Point(15, 111);
            this.llParentHandler.Name = "llParentHandler";
            this.llParentHandler.Size = new System.Drawing.Size(55, 17);
            this.llParentHandler.TabIndex = 7;
            this.llParentHandler.TabStop = true;
            this.llParentHandler.Text = "linkLabel1";
            this.llParentHandler.UseCompatibleTextRendering = true;
            this.llParentHandler.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_LinkClicked);
            // 
            // llParent
            // 
            this.llParent.AutoSize = true;
            this.llParent.Enabled = false;
            this.llParent.Location = new System.Drawing.Point(13, 90);
            this.llParent.Name = "llParent";
            this.llParent.Size = new System.Drawing.Size(55, 13);
            this.llParent.TabIndex = 6;
            this.llParent.TabStop = true;
            this.llParent.Text = "linkLabel1";
            this.llParent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_LinkClicked);
            // 
            // llHandler
            // 
            this.llHandler.AutoSize = true;
            this.llHandler.Enabled = false;
            this.llHandler.Location = new System.Drawing.Point(13, 69);
            this.llHandler.Name = "llHandler";
            this.llHandler.Size = new System.Drawing.Size(55, 13);
            this.llHandler.TabIndex = 5;
            this.llHandler.TabStop = true;
            this.llHandler.Text = "linkLabel1";
            this.llHandler.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_LinkClicked);
            // 
            // llExtension
            // 
            this.llExtension.AutoSize = true;
            this.llExtension.Enabled = false;
            this.llExtension.Location = new System.Drawing.Point(13, 48);
            this.llExtension.Name = "llExtension";
            this.llExtension.Size = new System.Drawing.Size(55, 13);
            this.llExtension.TabIndex = 4;
            this.llExtension.TabStop = true;
            this.llExtension.Text = "linkLabel1";
            this.llExtension.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_LinkClicked);
            // 
            // lblDirectExtensionInfo
            // 
            this.lblDirectExtensionInfo.AutoSize = true;
            this.lblDirectExtensionInfo.Location = new System.Drawing.Point(150, 40);
            this.lblDirectExtensionInfo.Name = "lblDirectExtensionInfo";
            this.lblDirectExtensionInfo.Size = new System.Drawing.Size(46, 13);
            this.lblDirectExtensionInfo.TabIndex = 3;
            this.lblDirectExtensionInfo.Text = "LABEL3";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(153, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(456, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(72, 16);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtExtension
            // 
            this.txtExtension.Location = new System.Drawing.Point(16, 19);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(50, 20);
            this.txtExtension.TabIndex = 0;
            this.txtExtension.TextChanged += new System.EventHandler(this.txtExtension_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatus,
            this.ddbRegLinks,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 584);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(615, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatus
            // 
            this.slStatus.Name = "slStatus";
            this.slStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // ddbRegLinks
            // 
            this.ddbRegLinks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddbRegLinks.Image = ((System.Drawing.Image)(resources.GetObject("ddbRegLinks.Image")));
            this.ddbRegLinks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbRegLinks.Name = "ddbRegLinks";
            this.ddbRegLinks.Size = new System.Drawing.Size(29, 20);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 606);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.showOnlyRegisteredCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.extensionsListBox);
            this.Controls.Add(this.handlerComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MainForm";
            this.Text = "Preview Handler Association Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox handlerComboBox;
        private System.Windows.Forms.CheckedListBox extensionsListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer keyTimer;
        private System.Windows.Forms.CheckBox showOnlyRegisteredCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatus;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblDirectExtensionInfo;
        private System.Windows.Forms.LinkLabel llExtension;
        private System.Windows.Forms.ToolStripDropDownButton ddbRegLinks;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.LinkLabel llParentHandler;
        private System.Windows.Forms.LinkLabel llParent;
        private System.Windows.Forms.LinkLabel llHandler;
    }
}

