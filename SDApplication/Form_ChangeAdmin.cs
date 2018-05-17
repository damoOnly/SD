using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SDApplication
{
    public partial class Form_ChangeAdmin : DevExpress.XtraEditors.XtraForm
    {
        public string ValueStr = string.Empty;
        public Form_ChangeAdmin()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ValueStr = textEdit1.Text;
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
            textEdit1.Focus();
        }
    }
}