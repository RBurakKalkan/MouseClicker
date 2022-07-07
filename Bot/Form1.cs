using IWshRuntimeLibrary;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Bot
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        public Form1()
        {
            InitializeComponent();
            timeSpanEdit1.EditValue = DateTime.Now.TimeOfDay;
        }

        DateTime baslangic, bitis;
        TimeSpan fark;

        private void timer1_Tick(object sender, EventArgs e)
        {
            baslangic = DateTime.Now.AddMilliseconds(Convert.ToInt16(textBox1.Text));
            fark = bitis - baslangic;
            if (baslangic >= bitis)
            {
                timer1.Stop();
                this.Text = "00:00:00.0000000";
                mouse_event(MOUSEEVENTF_LEFTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
                MessageBox.Show(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." + DateTime.Now.Millisecond, "Click Zamanı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Enabled = true;
            }
            else
            {
                this.Text = fark.ToString();
                textBox1.Enabled = false;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (msg.WParam.ToInt32())
            {
                case (int)Keys.F1:
                    timer1.Stop();
                    textBox1.Enabled = true;
                    break;
                case (int)Keys.F2:
                    baslangic = DateTime.Now;
                    bitis = DateTime.Today + timeSpanEdit1.TimeSpan;
                    if (bitis > baslangic)
                    {
                        timer1.Start();
                    }
                    else
                    {
                        MessageBox.Show("Belirlediğiniz Click saati geçti.", "Dikkat!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                case (int)Keys.Escape:
                    this.Close();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "0";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            textBox1.Enabled = true;
        }
        private void CreateShortcut()
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\MouseClicker.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Mouse Clicker Kısayolu";
            shortcut.TargetPath = Application.StartupPath.Trim()+ @"\Bot.exe";
            shortcut.Save();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateShortcut();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baslangic = DateTime.Now;
            bitis = DateTime.Today + timeSpanEdit1.TimeSpan;
            if (bitis > baslangic)
            {
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Belirlediğiniz Click saati geçti.", "Dikkat!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
