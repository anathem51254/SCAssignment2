using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SC2012_Assign
{
    public partial class Form1 : Form
    {
        bool initPb = true;
        Bitmap bm;
        int printInfo = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // create and draw maze
            if (initPb) 
            {
                createAndDrawMaze();
            }
            else drawMaze(G.maze);
        }

        public void createAndDrawMaze()
        {
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            G.maze = new Maze(comboBox1.Text);
            pictureBox1.Image = bm;
            G.maze.computeDrawSize(pictureBox1.Width, pictureBox1.Height);
            drawMaze(G.maze);
        }

        public void drawMaze(Maze maz)
        {
            System.Drawing.Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(pictureBox1.Image);

            // first clear the picture box
            Brush myBrush = new SolidBrush(System.Drawing.Color.Black);
            Rectangle myRectangle = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            graphicsObj.FillRectangle(myBrush, myRectangle);

            // now draw the maze            
            Brush myBrush2 = new SolidBrush(System.Drawing.Color.Black);
            Rectangle myRectangle2 = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            for (int y=0; y<maz.ySize; y++)
                for (int x = 0; x < maz.xSize; x++)
                {
                    System.Drawing.Color c = maz.getColor(x, y);
                    ((SolidBrush)myBrush2).Color = c;
                    myRectangle2.X = x * maz.drawSizeX;
                    myRectangle2.Y = y * maz.drawSizeY;
                    myRectangle2.Width = maz.drawSizeX;
                    myRectangle2.Height = maz.drawSizeY;
                    graphicsObj.FillRectangle(myBrush2, myRectangle2);
                }
            pictureBox1.Refresh();
        }

        public void drawPathInMaze(PathInMaze path) // quick way to draw lines 
        {
            drawPathInMaze(path, 2 , 2, 2, true);
        }

        public void drawPathInMaze(PathInMaze path, int offsetX, int offsetY, int lineWidth, bool showCount)
        {
            if (path.lengthOfpath == 0) return;

            System.Drawing.Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(pictureBox1.Image);

            Pen myPen = new Pen(Color.DarkBlue, lineWidth);

            for (int i = 0; i < path.lengthOfpath-1; i++)
            {
                Point p1 = new Point(path.pathx[i] * path.mazz.drawSizeX + path.mazz.drawSizeX / path.divi, path.pathy[i] * path.mazz.drawSizeY + path.mazz.drawSizeY / path.divi);
                Point p2 = new Point(path.pathx[i + 1] * path.mazz.drawSizeX + path.mazz.drawSizeX / path.divi, path.pathy[i + 1] * path.mazz.drawSizeY + path.mazz.drawSizeY / path.divi);
                p1.X = p1.X + offsetX;
                p2.X = p2.X + offsetX;
                p1.Y = p1.Y + offsetY;
                p2.Y = p2.Y + offsetY;
                // draw one line
                graphicsObj.DrawLine(myPen,p1,p2);
                if (showCount)
                {
                    // now draw overcount
                    int x = path.pathx[i + 1];
                    int y = path.pathy[i + 1];
                    int cnt = path.countOnPath(x, y);
                    if (cnt > 0)
                    {
                        string s = cnt.ToString();
                        Point p3 = new Point();
                        p3.X = p2.X - 5;
                        p3.Y = p2.Y - 5;
                        graphicsObj.DrawString(s, new Font("Courier New", 9), Brushes.DarkGoldenrod, p2);

                    }
                }
                
            }
            //draw final place
            Point p1a = new Point();
            Point p2a = new Point(path.pathx[path.lengthOfpath - 1] * path.mazz.drawSizeX + path.mazz.drawSizeX / path.divi, path.pathy[path.lengthOfpath-1] * path.mazz.drawSizeY + path.mazz.drawSizeY / path.divi);
            p2a.X = p2a.X + 3;
            p2a.Y = p2a.Y + 3;
            p1a.X = p2a.X - 6;
            p1a.Y = p2a.Y - 6;
            Pen zPen = new Pen(Color.Purple, 6);
            graphicsObj.DrawLine(zPen, p1a, p2a);

            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PathInMaze p = new PathInMaze(G.maze);
            G.path = p;
            p.movement(0);
            p.movement(1);
            p.movement(0);
            p.movement(1);
            p.movement(0);
            p.movement(1);
            p.movement(0);
            p.movement(1);
            p.movement(0);
            p.movement(1);
            p.movement(0);
            p.movement(1);
            p.movement(0);
            p.movement(1);
            p.movement(0);
            drawPathInMaze(G.path);
 //          System.Windows.Forms.MessageBox.Show("Score is "+G.path
   //             );
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Genome g = new Genome();
            //g.init(30);
            //
            // ...
            //
            
            G.path = g.calcScore(G.maze);
            drawMaze(G.maze);
            drawPathInMaze(G.path);
            label1.Text = String.Format("{0:00000}", g.score);
        }

        public void drawHighScore()
        {
            drawMaze(G.maze);
            PathInMaze p = new PathInMaze(G.maze);
            Genome g = G.pop.pp[G.pop.highScoreIndex];
            G.path = g.calcScore(G.maze);
            drawPathInMaze(G.path);
        }

        public void drawVars()
        {
            label2.Text = String.Format("Mutation Percent {0:00.00}", G.mutationPercent);
            label3.Text = String.Format("Mutations {0:D}", G.mutations);
            label9.Text = String.Format("Duplicates {0:D}", G.dupNum);
            label10.Text = String.Format("Weaklings {0:D}", G.weaklingNum);
            label4.Text = String.Format("Generation {0:D}", G.generation);
            label5.Text = String.Format("Best Score {0:D}", G.bestScore);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            G.rnd = new Random();
            createAndDrawMaze();
            G.pop = new Population(int.Parse(textBox1.Text));
            G.pop.setupPop();
            G.pop.scorePop();
            G.pop.findHighestScore();
            G.pop.findLowestScore();
            drawHighScore();
            G.mutationPercent = double.Parse(textBox3.Text);
            G.generation=0;
            G.generations=int.Parse(textBox2.Text);
            G.mutations=0;
            G.weaklingNum = 0;
            G.dupNum = 0;
            drawVars();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            run1Gen();
        }

        public void run1Gen()
        {
            int scoreDif = G.pop.findLowestScore() - G.pop.findHighestScore();
            G.weaklingNum += G.pop.RemoveWeaklings( (G.pop.highScore - (-scoreDif / 2)) );

            // assumes that population has been scored
            for (int i = 0; i < G.pop.numInPop / 2; i++)
            {
                //G.dupNum += G.pop.CheckDuplicates();
                G.pop.findLowestScore();
                int l = G.pop.lowScoreIndex;
                int mom = G.pop.pickRandom();
                int dad = G.pop.pickRandom();
                G.pop.pp[l] = Genome.breed(G.pop.pp[mom], G.pop.pp[dad], G.mutationPercent);
                
                if (G.pop.pp[l].mutant) G.mutations++;
            }

            G.generation++;
            if (G.generation >= G.generations) G.run = false;
            G.pop.scorePop();
            G.pop.findHighestScore();
            G.bestScore = G.pop.highScore;
            if (G.pop.pp[G.pop.highScoreIndex].foundEnd && G.stopWhenPathFound) G.run = false;
            drawHighScore();
            drawVars();
            Application.DoEvents();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string t = "";
            for (int i = 0; i < G.pop.numInPop; i++)
            {
                t = t + G.pop.pp[i].asText(i)+"\n";
            }
            debugInfo.Text += t;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            G.run = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            G.run=true;
            G.stopWhenPathFound = checkBox1.Checked;
            while (G.run)
            {
                run1Gen();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
