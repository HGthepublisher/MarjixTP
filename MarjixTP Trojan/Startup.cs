using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MarjixTP_Trojan.DllImports;

namespace MarjixTP_Trojan
{
    public static class Startup
    {
        private static void Main()
        {
            WindowsPrincipal winPrin = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = winPrin.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
            {
                MessageBox.Show("Why'd you run this as admin? I don't feel like destroying your computer. Instead, run this normally. (Without admin!)", "Hold on.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                MessageBox.Show("If you got this virus on any other platform but the official Github site from HGThePublisher, this might be a real destructive virus!!! More info is on the Github repository.", "HEADS UP!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult message1 = MessageBox.Show("Hi, I've seen that you're currently running something I, HGThePublisher, has created. If you want to continue, click yes. This isn't a real virus and won't do anything but play funky sounds and make cool visuals.", "Until you run...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (message1 == DialogResult.Yes)
                {
                    MessageBox.Show("Alright, well, lets get started!! (Click OK to continue.) Thanks for using!!", "Let's go!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MarjixTP_Trojan.FX.MTPMain();
                }
                else
                {
                    MessageBox.Show("I'm quitting then, you do you. Come back anytime if you want!", "Bye bye!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
