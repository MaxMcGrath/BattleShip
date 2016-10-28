using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class newgame : Form
    {
        public newgame()
        {
            InitializeComponent();
            difficultyComboBox.SelectedIndex=0;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void move_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            difficulty = -1;
            this.Close();
        }

        private int _difficulty;

        public int difficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string difficultyLevel;
            if (difficultyComboBox.SelectedIndex != -1)
            {
                difficultyLevel = difficultyComboBox.SelectedItem.ToString();
                switch (difficultyLevel)
                {
                    case "Normal":
                        difficulty= 10;
                        break;
                    case "Hard":
                        difficulty = 9;
                        break;
                    case "Brutal":
                        difficulty = 8;
                        break;
                }
            }
            else
            {
                MessageBox.Show("Select a Difficulty.");
            }
            this.Close();
        }
    }
}
