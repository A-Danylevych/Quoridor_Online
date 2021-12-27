using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Model;

namespace Quoridor
{

    public partial class GameBoard : Form, IViewer
    {
        private readonly Controller.Controller _controller;
        private readonly UserInterface _userInterface;


        public GameBoard(UserInterface userInterface, string password)  
        {
            _userInterface = userInterface;
            _controller = new Controller.Controller(password);
            InitializeComponent();

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            Hide();
            _userInterface.Restart();
            _userInterface.Show();
        }

        private void MainGameTimer(object sender, EventArgs e)
        {
            foreach (Control x in Controls)
            {
                if ((string) x.Tag == "Wall" && !TouchingOther(x.Top, x.Left))
                {
                    x.MouseClick += (aSender, aArgs) =>
                    {
                        _controller.SetWall(x.Top, x.Left, IsVertical(x.Top, x.Left));
                    };
                    x.MouseHover += (aSender, aArgs) =>
                    {
                        if (x.BackColor != System.Drawing.Color.LightSlateGray && !TouchingOther(x.Top, x.Left))
                        {
                            RenderWall(x.Top, x.Left, System.Drawing.Color.LightSteelBlue);
                        }

                    };
                    x.MouseLeave += (aSender, aArgs) =>
                    {
                        if (x.BackColor != System.Drawing.Color.LightSlateGray)
                        {
                            RenderWall(x.Top, x.Left, System.Drawing.Color.LightSkyBlue);
                            x.SendToBack();
                        }
                    };
                }

                if ((string) x.Tag == "Cell")
                {
                    x.MouseClick += (aSender, aArgs) => { _controller.SetCell(x.Top, x.Left); };

                }
            }
        }

        private bool TouchingOther(int top, int left)
        {
            var isVertical = IsVertical(top, left);
            foreach (var w in WallsList())
            {
                if (isVertical)
                {
                    if (w.Left == left && (w.Top == top-75 || w.Top == top+75))
                    {
                        if (w.BackColor == System.Drawing.Color.LightSlateGray)
                        {
                            return true;
                        }
                    }

                    if (w.Top != top + 50 || w.Left != left - 50) continue;
                    if (w.BackColor == System.Drawing.Color.LightSlateGray)
                    {
                        return true;
                    }
                }
                else
                {
                    if (w.Top == top && (w.Left == left-75 || w.Left == left+75))
                    {
                        if (w.BackColor == System.Drawing.Color.LightSlateGray)
                        {
                            return true;
                        }
                    }

                    if (w.Top != top - 50 || w.Left != left + 50) continue;
                    if (w.BackColor == System.Drawing.Color.LightSlateGray)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ResetGame(Color color)
        {
            gameTimer.Start();

            label1.Text = @"Залишилось стін: 10" + @"      you're " + color;
            label2.Text = @"Залишилось стін: 10";
            
            
            
            ClearWalls();
            RenderBottomPlayer(625, 325);
            RenderUpperPlayer(25, 325);

            label1.Left = 22;
            label2.Left = 383;
        }

        private void ClearWalls()
        {
            foreach (var wall in WallsList())
            {
                wall.BackColor = System.Drawing.Color.LightSkyBlue;
                wall.SendToBack();
            }
        }

        private void GameOver(string message)
        {

            gameTimer.Stop();

            label1.Left = 250;

            label1.Text = @"     " + message + @"   won!    Press enter to restart";

            label2.Text = null;
        }

        public bool IsVertical(int top, int left)
        {
            var horizontal = false;
            foreach (var unused in WallsList().Where(v => v.Height == 25 && v.Top == top && v.Left == left))
            {
                horizontal = true;
            }
            return !horizontal;
        }

        public void RenderEnding(string message)
        {
            GameOver(message);
        }

        public void RenderUpperPlayer(int top, int left)
        {
            GreenDot.Top = top + 6;
            GreenDot.Left = left +6;
            GreenDot.BringToFront();
        }

        public void RenderBottomPlayer(int top, int left)
        {
            RedDot.Top = top + 8;
            RedDot.Left = left + 8;
            RedDot.BringToFront();
        }
        
        public void RenderWall(int top, int left)
        {
            RenderWall(top, left, System.Drawing.Color.LightSlateGray);
        }

        private List<PictureBox> WallsList()
        {
            var pic = new List<PictureBox>
            {
                pictureBox1,
                pictureBox3,
                pictureBox4,
                pictureBox5,
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9,
                pictureBox10,
                pictureBox11,
                pictureBox12,
                pictureBox13,
                pictureBox14,
                pictureBox15,
                pictureBox16,
                pictureBox17,
                pictureBox18,
                pictureBox19,
                pictureBox20,
                pictureBox21,
                pictureBox22,
                pictureBox23,
                pictureBox24,
                pictureBox25,
                pictureBox26,
                pictureBox27,
                pictureBox28,
                pictureBox29,
                pictureBox30,
                pictureBox31,
                pictureBox32,
                pictureBox33,
                pictureBox34,
                pictureBox35,
                pictureBox36,
                pictureBox37,
                pictureBox38,
                pictureBox39,
                pictureBox40,
                pictureBox41,
                pictureBox42,
                pictureBox43,
                pictureBox44,
                pictureBox45,
                pictureBox46,
                pictureBox47,
                pictureBox48,
                pictureBox49,
                pictureBox50,
                pictureBox51,
                pictureBox52,
                pictureBox53,
                pictureBox54,
                pictureBox55,
                pictureBox56,
                pictureBox57,
                pictureBox58,
                pictureBox59,
                pictureBox60,
                pictureBox61,
                pictureBox62,
                pictureBox63,
                pictureBox64,
                pictureBox65,
                pictureBox66,
                pictureBox67,
                pictureBox68,
                pictureBox69,
                pictureBox70,
                pictureBox71,
                pictureBox72,
                pictureBox73,
                pictureBox74,
                pictureBox75,
                pictureBox76,
                pictureBox77,
                pictureBox78,
                pictureBox79,
                pictureBox80,
                pictureBox81,
                pictureBox82,
                pictureBox83,
                pictureBox84,
                pictureBox85,
                pictureBox86,
                pictureBox87,
                pictureBox88,
                pictureBox89,
                pictureBox90,
                pictureBox91,
                pictureBox92,
                pictureBox93,
                pictureBox94,
                pictureBox95,
                pictureBox96,
                pictureBox97,
                pictureBox98,
                pictureBox99,
                pictureBox100,
                pictureBox101,
                pictureBox102,
                pictureBox103,
                pictureBox104,
                pictureBox105,
                pictureBox106,
                pictureBox107,
                pictureBox108,
                pictureBox109,
                pictureBox110,
                pictureBox111,
                pictureBox112,
                pictureBox113,
                pictureBox114,
                pictureBox115,
                pictureBox116,
                pictureBox117,
                pictureBox118,
                pictureBox119,
                pictureBox120,
                pictureBox121,
                pictureBox122,
                pictureBox123,
                pictureBox124,
                pictureBox125,
                pictureBox126,
                pictureBox127,
                pictureBox128,
                pictureBox129,
                pictureBox130,
                pictureBox131,
                pictureBox132,
                pictureBox133,
                pictureBox134,
                pictureBox135,
                pictureBox136,
                pictureBox137,
                pictureBox138,
                pictureBox139,
                pictureBox140,
                pictureBox141,
                pictureBox142,
                pictureBox143,
                pictureBox144,
                pictureBox145,
                pictureBox146,
                pictureBox147,
                pictureBox148,
                pictureBox149,
                pictureBox150,
                pictureBox151,
                pictureBox152,
                pictureBox153,
                pictureBox154,
                pictureBox155,
                pictureBox156,
                pictureBox157,
                pictureBox158,
                pictureBox159,
                pictureBox160,
                pictureBox161,
                pictureBox162,
                pictureBox163,
                pictureBox164,
                pictureBox165,
                pictureBox166,
                pictureBox167,
                pictureBox168,
                pictureBox169,
                pictureBox170,
                pictureBox171,
                pictureBox172,
                pictureBox173,
                pictureBox174,
                pictureBox175,
                pictureBox176,
                pictureBox177,
                pictureBox178,
                pictureBox179,
                pictureBox180,
                pictureBox181,
                pictureBox182,
                pictureBox183,
                pictureBox184,
                pictureBox185,
                pictureBox186,
                pictureBox187,
                pictureBox188,
                pictureBox189,
                pictureBox190,
                pictureBox191,
                pictureBox192,
                pictureBox193,
                pictureBox194,
                pictureBox195,
                pictureBox196,
                pictureBox197,
                pictureBox198,
                pictureBox199,
                pictureBox200,
                pictureBox201,
                pictureBox202,
                pictureBox203,
                pictureBox204,
                pictureBox205,
                pictureBox206,
                pictureBox207,
                pictureBox208,
                pictureBox209,
                pictureBox210,
                pictureBox211,
                pictureBox212,
                pictureBox213,
                pictureBox214,
                pictureBox215,
                pictureBox216,
                pictureBox217,
                pictureBox218,
                pictureBox219,
                pictureBox220,
                pictureBox221,
                pictureBox222,
                pictureBox223,
                pictureBox224,
                pictureBox225,
                pictureBox226,
                pictureBox227,
                pictureBox228,
                pictureBox229
            };
            
            return pic.Where(p => (string) p.Tag == "Wall").ToList();
        }

        private void RenderWall(int top, int left, System.Drawing.Color color)
        {
            foreach (var v in WallsList().Where(v => v.Top == top && v.Left == left))
            {
                v.BackColor = color;
                v.BringToFront();
            }
        }

        public void RenderRemainingWalls(int topCount, int bottomCount)
        {
            label1.Text = @"Залишилось стін: " + bottomCount;
            label2.Text = @"Залишилось стін: " + topCount;
        }
    }

}
