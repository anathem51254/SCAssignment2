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
        public const int genomeLen = 95;
    }

    public class Gene
    {
        private const int GENEVARIETY = 4;

        public int gene;

        public Gene() 
        {
            gene = 0;
        }

        public void RandomGenes()
        {
            gene = G.rnd.Next(GENEVARIETY);
        }
    }

    public class Genome
    {
        public int dnaSize;
        public int score;
        public int manhattanDis;
        public bool mutant = false; // handy for mutation count set to true if the gene is mutated
        public bool foundEnd = false; // needed to early exit must be set when score is set

        public List<Gene> dna;

        public Genome()
        {
        }

        public void BlankGenome()
        {
            dna = new List<Gene>();
            dnaSize = AssignAnswer.genomeLen;
            score = -1;
        }

        public void RandomGenome()
        {
            dna = new List<Gene>();

            for (int i = 0; i < AssignAnswer.genomeLen; i++)
            {
                Gene newGene = new Gene();
                newGene.RandomGenes();
                dna.Add(newGene);
            }

            dnaSize = AssignAnswer.genomeLen;
            score = -1;
        }

        public static Genome breed(Genome mum, Genome dad, double mutationPercent)
        {
            Genome child = new Genome();
            child.BlankGenome();

            int coPoint = G.rnd.Next(0, mum.dnaSize);

            for (int i = 0; i < coPoint; i++)
            {
                child.dna.Add(mum.GetGene(i));
            }

            for( int i = coPoint; i < dad.dnaSize; i++)
            {
                child.dna.Add(dad.GetGene(i));
            }

            if (G.rnd.NextDouble() * 100 < mutationPercent) 
                child = child.Mutate(child);

            return child;
        }

        public string asText(int j)
        {
            return G.pop.pp[j].score.ToString();
        }

        public Genome Mutate(Genome child)
        {
            child.mutant = true;

            int mutstrat = 1;//G.rnd.Next(3);

            switch(mutstrat)
            {
                case 1:
                    int i = G.rnd.Next(G.rnd.Next(10), child.dnaSize);
                    
                    for (int x = 0; x < i; x++)
                    {
                        Gene newGene = new Gene();
                        newGene.RandomGenes();
                        int r = G.rnd.Next(0, child.dnaSize);
                        child.dna[r] = newGene;
                    }
                    break;
                case 2:

                    int prevy;
                    int y;

                    for (int z = 0; z < 3; z++)
                    {
                        y = G.rnd.Next(0, child.dnaSize);
                        prevy = y;

                        if (prevy == y)
                            continue;
                        else
                        {
                            //for(int hh = 0; hh < 2; hh++)
                            //{
                                Gene newGene = new Gene();

                                if (/*child.dna[y].gene == 0 ||*/ child.dna[y].gene == 2)
                                {
                                    //int temp = G.rnd.Next(0, 1);

                                    //if (temp == 0)
                                    //{
                                        newGene.gene = 0;
                                        child.dna[y] = newGene;
                                    //}
                                    //else if (temp == 1)
                                    //{
                                       // newGene.gene = 0;
                                       // child.dna[y] = newGene;
                                    //}
                                }

                            //}
                        }

                    }
                    break;
            }

            return child;
        }

        public Gene GetGene(int i)
        {
            return dna[i];
        }

        public PathInMaze calcScore(Maze m)
        {
            PathInMaze p = new PathInMaze(m);
           
            score = 100000;
            int path = 0;
            int wall = 0;
            for (int i = 0; i < AssignAnswer.genomeLen; i++)
            {
                int mi = dna.ElementAt(i).gene;

                // 0= Move Right(x+)
                // 1= Move Down(y+)
                // 2= Move Left(x+)
                // 3= Move Up(y-)
                // 4= move forward
                // 5= turn right
                // 6= turn left
                // 7= about face (turn right twice)
                    
                //if (mi == -1) break; // perhaps use -1 to indicate at end
                int rc = p.movement(mi);

                // resSucessEnd = Success got to end
                // resSucessTurn  = move was successfull but did not move it was a turn command
                // resSucess = successfull and moved 1 square
                // resFailWall = fail hit a wall (did not move)
                // resFailOut = fail moved out of maze (did not move)
                // resFailOverPath = fail moved over previous path (but it did move)
                // resInvalid = not a valid action - fail

                int dx = Math.Abs(p.curX - p.mazz.endPosx);
                int dy = Math.Abs(p.curY - p.mazz.endPosy);

                int _manhattanDis = dx + dy;

                if (rc == PathInMaze.resSucessEnd)
                {
                    foundEnd = true;
                    score = score + 100000;
                    break;
                }

                if (rc == PathInMaze.resSucessTurn)
                {
                    score = score + 800;

                    if (_manhattanDis < manhattanDis)
                        score += 1200;
                    else if (_manhattanDis > manhattanDis/* || _manhattanDis == manhattanDis*/)
                        score -= 800;
                    
                    continue;
                }

                if (rc == PathInMaze.resFailOverPath)
                {
                    score = score - 2000;
                    //path++;
                    //if ( (path % 4) == 0)
                    //    score = score - 5000;

                    if (_manhattanDis < manhattanDis)
                        score += 1200;
                    else if (_manhattanDis > manhattanDis/* || _manhattanDis == manhattanDis*/)
                        score -= 800;

                    continue;
                }

                if (rc == PathInMaze.resSucess)
                {
                    score = score + 800;

                    if (_manhattanDis < manhattanDis)
                        score += 1200;
                    else if (_manhattanDis > manhattanDis/* || _manhattanDis == manhattanDis*/)
                        score -= 800;

                    continue;
                }

                if (rc == PathInMaze.resFailOut || rc == PathInMaze.resFailWall)
                {
                    score = score - 800;
                    //wall++;
                    //if ((wall % 4) == 0)
                    //    score = score - 1500;

                    if (_manhattanDis < manhattanDis)
                        score += 1200;
                    else if (_manhattanDis > manhattanDis/* || _manhattanDis == manhattanDis*/)
                        score -= 800;

                    continue;
                }

                if (rc == PathInMaze.resInvalid)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid Movement");
                    continue;
                }
                manhattanDis = _manhattanDis;
            }

            return p;
        }
    }

    public class Population
    {
        public Genome[] pp; // the population
        public int numInPop;
        public int highScore;
        public int highScoreIndex;
        public int lowScore;
        public int lowScoreIndex;

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
                pp[i].RandomGenome();
            }
        }

        public void scorePop()
        {
            for (int i=0; i<numInPop; i++)
            {
                pp[i].calcScore(G.maze);
            }
        }

        public int RemoveWeaklings(int midscore)
        {
            int cnt = 0;
            for (int index = 0; index < numInPop; index++)
            {
                if (pp[index].score < (midscore * 1) )
                {
                    pp[index].RandomGenome();
                    cnt++;
                }
            }
            return cnt;
        }

        public int CheckDuplicates()
        {
            int cnt = 0;

            for (int i = 0; i < numInPop; i++)
                for (int x = 0; x < numInPop; x++)
                {
                    int gg = 1;
                    for (int f = 0; f < AssignAnswer.genomeLen; f++)
                    {

                        Gene temp1 = pp[i].GetGene(f);
                        Gene temp2 = pp[x].GetGene(f);

                        //for (int hh = 0; hh < 4; hh++)
                        //{
                        if (temp1.gene == temp2.gene)
                        {
                            gg++;
                        }

                        if (gg == 30)
                        {
                            pp[i].RandomGenome();
                            cnt++;
                        }
                        //}
                    }
                }
            return cnt;
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
            return j;
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
            lowScore = j;
            lowScoreIndex = k;
            return j;
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
