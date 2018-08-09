using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SDApplication
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //DevExpress.UserSkins.OfficeSkins.Register();//进行皮肤组件注册
            DevExpress.UserSkins.BonusSkins.Register();//进行皮肤组件注册
           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!DevExpress.Skins.SkinManager.AllowFormSkins)
            {
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.Skins.SkinManager.EnableFormSkinsIfNotVista();
                DevExpress.Skins.SkinManager.EnableMdiFormSkins();
            }
            DevExpress.XtraEditors.Controls.Localizer.Active = new LocalizationCHS();
            Application.Run(new MainForm2()); 
            
        }
    }
}
