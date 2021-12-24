
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    public partial class UserInterface : Form
    {
        private GameBoard Form;
        public UserInterface()
        {
            InitializeComponent();
            Form = new GameBoard(this);
        }

        private void Close()
        {
            Hide();

            Form.resetGame();
            
            Form.Show();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
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
