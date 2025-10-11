using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebarSampling
{
    internal static class Program
    {
 
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 创建唯一命名的 Mutex
            using (Mutex mutex = new Mutex(true, "Global\\RebarSampling", out bool createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("程序已在运行中", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
