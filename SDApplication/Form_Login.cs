using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Entity;

namespace SDApplication
{
    public partial class Form_Login : DevExpress.XtraEditors.XtraForm
    {
        public SystemConfig config = new SystemConfig();
        public Form_Login()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // 验证普通用户
            if (config.User == comboBoxEdit_user.Text.Trim()&&config.UserPWD == textEdit_pwd.Text.Trim())
            {
                Gloabl.IsAdmin = false;
            }
            else if (config.Admin == comboBoxEdit_user.Text.Trim() && config.AdminPWD == textEdit_pwd.Text.Trim())
            {
                Gloabl.IsAdmin = true;
            }
            else
            {
                XtraMessageBox.Show("用户名或密码错误，请重新输入");
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Form_ChangeAdmin_Load(object sender, EventArgs e)
        {
           
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                simpleButton1_Click(null, null);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            textEdit_pwd.Focus();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}