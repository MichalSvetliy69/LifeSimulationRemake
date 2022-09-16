using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LS.Remake
{
    public partial class Form1 : Form

    {
        private int currentGenerations = 0;
        
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;
        public Form1()
        {
            InitializeComponent();
        }
        private int CountNeighbours(int x , int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                   var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var IsSelfChecking = col == x && row == y;
                    var hasLife = field[col,row];
                    if (hasLife && !IsSelfChecking)
                    {
                        count++;
                    }

                }

            }
            return count;
        }                                                       //CountNeighbors
        private void StopGame() {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            Resolution.Enabled = true;
            Dencity.Enabled = true;
        }                                                                         //Stop.Descride
        private void StartGame()
        {
            currentGenerations = 0;
            Text = $"Generation {currentGenerations}";
            if (timer1.Enabled)
                return;
            Resolution.Enabled = false;
            Dencity.Enabled = false;

                resolution = (int)Resolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];
            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)Dencity.Value ) == 0;
                }

            }
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graphics = Graphics.FromImage(pictureBox1.Image);       
            timer1.Start();
            
        }                                                                         //Start.Descride

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);
            var newField =new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var NeighborsCount = CountNeighbours(x,y);
                    var haslife = field[x, y];
                    if (!haslife && NeighborsCount == 3)
                    {
                        newField[x, y] = true;
                    }else if (haslife && (NeighborsCount <2 || NeighborsCount >3)) {
                        newField[x, y] = false;
                    }else
                    {
                        newField[x, y] = field[x, y];
                    }
                    if (haslife)
                    {
                        graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution, resolution);
                    }
                }

            }
            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation {++currentGenerations}";

        }                                                                    //NextGeneration
        

        private void BStart_Click(object sender, EventArgs e)
        {
            StartGame();


        }                                            //Start

        private void BStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }                                             //Stop

        private void timer1_Tick(object sender, EventArgs e)                                                //Timer
        {
            NextGeneration();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)                                 //Mouse
        {
            if (!timer1.Enabled)
                return;
            if (e.Button == MouseButtons.Left)
            {
                
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = VolidateMousPosition(x,y);
                if (validationPassed)
                {
                    field[x, y] = true;

                }
                field[x, y] = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = VolidateMousPosition(x, y);
                if (validationPassed)
                {
                    field[x, y] = false;

                }
                field[x, y] = false;
            }

        }
        private bool VolidateMousPosition (int x , int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {currentGenerations}";
        }                                             //CountGeneration
    }   
}
