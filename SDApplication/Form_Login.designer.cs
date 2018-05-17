namespace SDApplication
{
    partial class Form_Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Login));
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit_pwd = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit_user = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_pwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_user.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(93, 181);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "确定";
            this.btn_OK.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // textEdit_pwd
            // 
            this.textEdit_pwd.Location = new System.Drawing.Point(208, 135);
            this.textEdit_pwd.Name = "textEdit_pwd";
            this.textEdit_pwd.Properties.PasswordChar = '*';
            this.textEdit_pwd.Size = new System.Drawing.Size(100, 20);
            this.textEdit_pwd.TabIndex = 1;
            this.textEdit_pwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEdit1_KeyPress);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(93, 138);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(28, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "密码:";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(233, 181);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(93, 98);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(40, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "用户名:";
            // 
            // comboBoxEdit_user
            // 
            this.comboBoxEdit_user.EditValue = "user";
            this.comboBoxEdit_user.Location = new System.Drawing.Point(208, 95);
            this.comboBoxEdit_user.Name = "comboBoxEdit_user";
            this.comboBoxEdit_user.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit_user.Properties.Items.AddRange(new object[] {
            "user",
            "admin"});
            this.comboBoxEdit_user.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboBoxEdit_user.Size = new System.Drawing.Size(100, 20);
            this.comboBoxEdit_user.TabIndex = 0;
            // 
            // Form_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Tile;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.ClientSize = new System.Drawing.Size(417, 258);
            this.Controls.Add(this.comboBoxEdit_user);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.textEdit_pwd);
            this.Controls.Add(this.btn_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Login";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.Form_ChangeAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_pwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit_user.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.TextEdit textEdit_pwd;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit_user;
    }
}