using System.Security.Principal;
using System.Windows.Forms;

namespace marjtp.Startup
{
    public static class MainClass
    {
        private static void Main()
        {
            WindowsPrincipal winPrin = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = winPrin.IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
            {
                MessageBox.Show("What's the point in running this in administrator mode? Run normally.", "???", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                DialogResult startMessage = MessageBox.Show("If you got this virus on any other platform but the official Github site from HGThePublisher, this might be a real destructive virus!!! More info is on the Github repository.", "HEADS UP!!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (startMessage == DialogResult.OK)
                {
                    DialogResult message1 = MessageBox.Show("Hi, I've seen that you're currently running something I, HGThePublisher, has created. If you want to continue, click yes. This isn't a real virus and won't do anything but play funky sounds and make cool visuals. The last part might require internet to work correctly!!!", "Until you run...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (message1 == DialogResult.Yes)
                    {
                        MessageBox.Show("Alright, well, lets get started!! (Click OK to continue.) Thanks for using!!", "Let's go!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        marjtp.Main.MainClass.MTPMain();
                    }
                    else
                    {
                        MessageBox.Show("I'm quitting then, you do you. Come back anytime if you want!", "Bye bye!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
