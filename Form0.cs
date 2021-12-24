
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public partial class Form0 : Form
    {
        private Form1 Form;
        public Form0()
        {
            InitializeComponent();
            Form = new Form1(this);
        }

        public bool playWithBot { get; set; }

        private void Close()
        {
            Hide();

            Form.resetGame(playWithBot);
            
            Form.Show();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            playWithBot = true;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            playWithBot = false;
            Close();
        }

        private void mainGameTimer(object sender, EventArgs e)
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "button")
                {
                    var color = x.BackColor;
                    x.MouseHover += (a_sender, a_args) =>
                    {
                        x.BackColor = Color.Lavender;
                    };

                    x.MouseLeave += (a_sender, a_args) =>
                    {
                        x.BackColor = color;
                    };

                }
            }
        }
    }
}
