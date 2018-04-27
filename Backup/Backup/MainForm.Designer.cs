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
            this.handlerComboBox = new System.Windows.Forms.ComboBox();
            this.extensionsListBox = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.keyTimer = new System.Windows.Forms.Timer(this.components);
            this.showOnlyRegisteredCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // handlerComboBox
            // 
            this.handlerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.handlerComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.handlerComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.handlerComboBox.FormattingEnabled = true;
            this.handlerComboBox.Location = new System.Drawing.Point(136, 9);
            this.handlerComboBox.Name = "handlerComboBox";
            this.handlerComboBox.Size = new System.Drawing.Size(257, 21);
            this.handlerComboBox.TabIndex = 0;
            this.handlerComboBox.SelectedIndexChanged += new System.EventHandler(this.handlerComboBox_SelectedIndexChanged);
            // 
            // extensionsListBox
            // 
            this.extensionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionsListBox.FormattingEnabled = true;
            this.extensionsListBox.Location = new System.Drawing.Point(136, 69);
            this.extensionsListBox.Name = "extensionsListBox";
            this.extensionsListBox.Size = new System.Drawing.Size(257, 229);
            this.extensionsListBox.TabIndex = 1;
            this.extensionsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.extensionsListBox_ItemCheck);
            this.extensionsListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.extensionsListBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Registered handlers:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 69);
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
            this.showOnlyRegisteredCheckBox.Location = new System.Drawing.Point(136, 35);
            this.showOnlyRegisteredCheckBox.Name = "showOnlyRegisteredCheckBox";
            this.showOnlyRegisteredCheckBox.Size = new System.Drawing.Size(246, 17);
            this.showOnlyRegisteredCheckBox.TabIndex = 4;
            this.showOnlyRegisteredCheckBox.Text = "Only show classes associated with this handler";
            this.showOnlyRegisteredCheckBox.UseVisualStyleBackColor = true;
            this.showOnlyRegisteredCheckBox.CheckedChanged += new System.EventHandler(this.showOnlyRegisteredCheckBox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 304);
            this.Controls.Add(this.showOnlyRegisteredCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.extensionsListBox);
            this.Controls.Add(this.handlerComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MainForm";
            this.Text = "Preview Handler Association Editor";
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.Load += new System.EventHandler(this.MainForm_Load);
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
    }
}

