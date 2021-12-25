using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Security.Cryptography;

namespace Quoridor
{
    public partial class UserInterface : Form
    {
        private GameBoard Form;
        private Client _client;
        private string password;
        
        public UserInterface()
        {
            InitializeComponent();
            password = RandomString(10);
            Form = new GameBoard(this, password);
            _client = new Client(this, Form);
        }

        public UserInterface(string password)
        {
            this.password = password;
            InitializeComponent();
            Form = new GameBoard(this, password);
            _client = new Client(this, Form);
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void Close(Color color)
        {
            Hide();

            Form.resetGame(color);
            
            Form.Show();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var message = new CWrapperMessage { LogIn = new CLogIn { Password = password } };
            _client.SendMessage(message);
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
