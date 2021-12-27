using System;
using System.Windows.Forms;
using System.Linq;

namespace Quoridor
{
    public partial class UserInterface : Form
    {
        private readonly GameBoard _gameBoard;
        private readonly Client _client;
        private readonly string _password;
        
        public UserInterface()
        {
            InitializeComponent();
            _password = RandomString(10);
            _gameBoard = new GameBoard(this, _password);
            _client = Client.GetInstance();
            _client.SetInterface(this);
            _client.SetView(_gameBoard);
            _client.Receive();
        }

        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void Close(Color color)
        {
            Hide();

            _gameBoard.ResetGame(color);
            
            _gameBoard.Show();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var message = new CWrapperMessage { LogIn = new CLogIn { Password = _password } };
            _client.SendMessage(message);
            label1.Text = @"Conecting....";
        }

        private void MainGameTimer(object sender, EventArgs e)
        {
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "button")
                {
                    var color = x.BackColor;
                    x.MouseHover += (aSender, aArgs) =>
                    {
                        x.BackColor = System.Drawing.Color.Lavender;
                    };

                    x.MouseLeave += (aSender, aArgs) =>
                    {
                        x.BackColor = color;
                    };

                }
            }
        }
    }
}
