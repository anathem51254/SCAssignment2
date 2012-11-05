using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC2012_Assign
{
    class AssignAnswer
    {
        public const int lowValGene = 0;
        public const int highValGene = 3;
        public const int genomeLen = 30;
    }

    public class Genome
    {

        public int score;
        public bool mutant = false; // handy for mutation count set to true if the gene is mutated
        public bool foundEnd = false; // needed to early exit must be set when score is set


        public Genome()
        {
        }

        public string asText(int j)
        {
            return "";
        }

        public void mutate()
        {
            //mutant = true;
        }

        public static Genome breed(Genome mom, Genome dad, double mutationPercent)
        {
            Genome rc=new Genome(); // change this
            //if (G.rnd.NextDouble() * 100 < mutationPercent) rc.mutate();

            return rc;
        }

        public void init(int lengthZ)
        {
        }

        public void setToRnd(/* ???? */) // hiVal is exclusive
        {
            //for (int i = 0; i < length; i++)
            //{
            //    //;
            //}
        }

        public PathInMaze calcScore(Maze m)
        {
            PathInMaze p = new PathInMaze(m);
           
            //score = initial_score;
            
            for (int i = 0; i < AssignAnswer.genomeLen; i++)
            {
                int mi = -91450; //**warning you need to change this**
                                 // mi= movement instruction 
                                 // mi will not be -91450 
                                 // mi needs to be one of:
                                    // 0= Move Right(x+)
                                    // 1= Move Down(y+)
                                    // 2= Move Left(x+)
                                    // 3= Move Up(y-)
                                    // 4= move forward
                                    // 5= turn right
                                    // 6= turn left
                                    // 7= about face (turn right twice)
                                    // 
                
                //if (mi == -1) break; // perhaps use -1 to indicate at end
                int rc = p.movement(mi);

                if (rc == PathInMaze.resSucessEnd)
                {
                    //foundEnd = true;
                    //score = score -+*/ ??;
                    break;
                }

                if (rc == PathInMaze.resSucessTurn || rc == PathInMaze.resFailOverPath)
                {
                    //score = score -+*/ ??;
                    continue;
                }

                if (rc == PathInMaze.resSucess)
                {
                    //score = score -+*/ ??;
                    continue;
                }

                if (rc == PathInMaze.resFailOut || rc == PathInMaze.resFailWall)
                {
                    //score = score -+*/ ??;
                    continue;
                }

                if (rc == PathInMaze.resInvalid)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid Movement");
                    continue;
                }

            }

            // some usefull code to score distance 
            //int dx = Math.Abs(p.curX - p.mazz.endPosx);
            //int dy = Math.Abs(p.curY - p.mazz.endPosy);
            // note dx + dy is the manhattan distance 
            return p;
        }
    }

    public class Population
    {

        public Genome[] pp; // the population
        public int numInPop;
        public int highScore;
        public int highScoreIndex;

        public Population(int popNum)
        {
            numInPop = popNum;
            pp = new Genome[popNum];
        }

        public void setupPop()
        {
            for (int i=0; i<numInPop; i++)
            {
                pp[i] = new Genome();
                //pp[i].setToRnd(????);
            }
        }

        public void scorePop()
        {
            for (int i=0; i<numInPop; i++)
            {
                pp[i].calcScore(G.maze);
            }
        }

        public int findHighestScore()
        {
            int k = 0;
            int j = pp[k].score;
            for (int i = 1; i < numInPop; i++)
            {
                if (pp[i].score > j)
                {
                    k = i;
                    j = pp[k].score;
                }
            }
            highScore = j;
            highScoreIndex = k;
            return k;
        }

        public int findLowestScore()
        {
            //ignores scores of -1
            int k = -1;
            int j=9999999;
            for (int i = 0; i < numInPop; i++)
            {
                if (k == -1)
                {
                    if (pp[i].score != -1)
                    {
                        k = i;
                        j = pp[i].score;
                        continue;
                    }
                }
                if (pp[i].score < j)
                {
                    k = i;
                    j = pp[k].score;
                }
            }
            //highScore = j;
            //highScoreIndex = k;
            return k;
        }


        public int pickRandom()
        {
            //ignores scores of -1
            for (int i = 0; i < numInPop * 3; i++)
            {
                int k = G.rnd.Next(0, G.pop.numInPop);
                if (pp[k].score == -1) continue;
                {
                    return k;
                }

            }
            for (int i = 0; i < numInPop ; i++)
            {
                int k = i;
                if (pp[k].score == -1) continue;
                {
                    return k;
                }
            }
            System.Windows.Forms.MessageBox.Show("Invalid pickRandom");
            return 0;
        }
    }

}
