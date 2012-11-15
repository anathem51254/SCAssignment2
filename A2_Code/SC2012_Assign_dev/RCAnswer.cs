using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC2012_Assign
{
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
        public int score;
        public int manhattanDisMov;
        public int manhattanDisPoi;
        public bool mutant = false; // handy for mutation count set to true if the gene is mutated
        public bool foundEnd = false; // needed to early exit must be set when score is set

        public Gene[] dna;

        public Genome()
        {
        }

        public void BlankGenome()
        {
            dna = new Gene[G.genomeLen];
            for (int i = 0; i < G.genomeLen; i++)
            {
                dna[i] = new Gene();
            }
            score = -999999;
        }

        public void RandomGenome()
        {
            dna = new Gene[G.genomeLen];
            for (int i = 0; i < G.genomeLen; i++)
            {
                Gene newGene = new Gene();
                newGene.RandomGenes();
                dna[i] = newGene;
            }
            score = -999999;
        }

        public static Genome breed(Genome mum, Genome dad, double mutationPercent)
        {
            Genome child = new Genome();
            child.BlankGenome();

            int coPoint = G.rnd.Next(0, G.genomeLen);

            for (int i = 0; i < coPoint; i++)
            {
                child.dna[i] = mum.GetGene(i);
            }

            for (int i = coPoint; i < G.genomeLen; i++)
            {
                child.dna[i] = dad.GetGene(i);
            }

            if (G.rnd.NextDouble() * 100 < mutationPercent) 
                child = child.Mutate(child);

            return child;
        }

        public Gene GetGene(int i)
        {
            return dna[i];
        }

        public Genome Mutate(Genome child)
        {
            child.mutant = true;

            int mutstrat = G.rnd.Next(1, 3);

            switch(mutstrat)
            {
                case 1:
                    MutStrat1();
                    break;
                case 2:
                    MutStrat2();
                    break;
            }

            return child;
        }

        private void MutStrat1()
        {
            int i = G.rnd.Next((G.rnd.Next(6) + G.rnd.Next(6)));

            for (int x = 0; x < i; x++)
            {
                Gene newGene = new Gene();
                newGene.RandomGenes();
                int r = G.rnd.Next(0, G.genomeLen);
                dna[r] = newGene;
            }
        }

        private void MutStrat2()
        {
            int lpoint = G.rnd.Next(1, 15);
            if (lpoint == 1)
                lpoint += 3;
            if (lpoint == 2)
                lpoint += 2;
            if (lpoint == 3)
                lpoint += 1;

            int movDec = G.rnd.Next(4);

            for (int fpoint = lpoint - 2; fpoint < lpoint; fpoint++)
            {
                if (movDec == 0)
                    dna[fpoint].gene = 0;
                if (movDec == 1)
                    dna[fpoint].gene = 1;
                if (movDec == 2)
                    dna[fpoint].gene = 2;
                if (movDec == 3)
                    dna[fpoint].gene = 3;
            }
        }

        public PathInMaze calcScore(Maze m)
        {
            PathInMaze p = new PathInMaze(m);
           
            score = 5000;

            int _manhattanDisPoi = calcDistance(p);

            int prevmi = 0;

            for (int i = 0; i < G.genomeLen; i++)
            {
                int mi = dna[i].gene;
                prevmi = mi;
                // 0= Move Right(x+)
                // 1= Move Down(y+)
                // 2= Move Left(x+)
                // 3= Move Up(y-)
                // 4= move forward
                // 5= turn right
                // 6= turn left
                // 7= about face (turn right twice)
                ////
                //if (mi == 0 && prevmi == 0 ||
                //    mi == 1 && prevmi == 1 ||
                //    mi == 2 && prevmi == 2 ||
                //    mi == 0 && prevmi == 0)
                //    score += 5;
                //else
                //    score -= 5;

                    
                //if (mi == -1) break; // perhaps use -1 to indicate at end
                int rc = p.movement(mi);

                // resSucessEnd = Success got to end
                // resSucessTurn  = move was successfull but did not move it was a turn command
                // resSucess = successfull and moved 1 square
                // resFailWall = fail hit a wall (did not move)
                // resFailOut = fail moved out of maze (did not move)
                // resFailOverPath = fail moved over previous path (but it did move)
                // resInvalid = not a valid action - fail

                int _manhattanDisMov = calcDistance(p);
                

                if (rc == PathInMaze.resSucessEnd)
                {
                    foundEnd = true;
                    score = score * 20;
                    break;
                }

                if (rc == PathInMaze.resFailOverPath)
                {
                    score = score - 30;

                    if (_manhattanDisMov < manhattanDisMov)
                        score += 20;
                    else if (_manhattanDisMov > manhattanDisMov)
                        score -= 20;
                    else if (_manhattanDisMov == manhattanDisMov)
                        score -= 2;

                    continue;
                }

                if (rc == PathInMaze.resSucess)
                {
                    score = score + 2;

                    if (_manhattanDisMov < manhattanDisMov)
                        score += 20;
                    else if (_manhattanDisMov > manhattanDisMov)
                        score -= 20;
                    else if (_manhattanDisMov == manhattanDisMov)
                        score -= 3;

                    continue;
                }

                if (rc == PathInMaze.resFailWall)
                {
                    score = score - 3;

                    if (_manhattanDisMov < manhattanDisMov)
                        score += 20;
                    else if (_manhattanDisMov > manhattanDisMov)
                        score -= 20;
                    else if (_manhattanDisMov == manhattanDisMov)
                        score -= 2;

                    continue;
                }

                if (rc == PathInMaze.resFailOut)
                {
                    score = score - 2;

                    if (_manhattanDisMov < manhattanDisMov)
                        score += 20;
                    else if (_manhattanDisMov > manhattanDisMov)
                        score -= 20;
                    else if (_manhattanDisMov == manhattanDisMov)
                        score -= 2;

                    continue;
                }

                if (rc == PathInMaze.resInvalid)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid Movement");
                    continue;
                }

                if (rc == PathInMaze.resSucessTurn)
                {
                    score = score + 3;

                    if (_manhattanDisMov < manhattanDisMov)
                        score += 20;
                    else if (_manhattanDisMov > manhattanDisMov || _manhattanDisMov == manhattanDisMov)
                        score -= 20;

                    continue;
                }
            }

            //if (_manhattanDisPoi < manhattanDisPoi)
            //   score += 15;
            //else if (_manhattanDisPoi > manhattanDisPoi)
            //   score -= 15;
            //else if (_manhattanDisPoi == manhattanDisPoi)
            //   score -= 5;

            return p;
        }

        private int calcDistance(PathInMaze p)
        {
            int dx = Math.Abs(p.curX - p.mazz.endPosx);
            int dy = Math.Abs(p.curY - p.mazz.endPosy);
            return (dx + dy);
        }
    } 

    public class Population
    {
        public Genome[] pp; // the population
        public int numInPop;
        public int highScore;
        public int highScoreIndex;
        public int midScore;
        public int scoreDif;
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

        public int RemoveWeaklings()
        {
            int cnt = 0;
            for (int index = 0; index < numInPop; index++)
            {
                if (pp[index].score < (midScore + (scoreDif / 4)) )
                {
                    System.Diagnostics.Debug.WriteLine(pp[index].score.ToString());
                    pp[index].RandomGenome();
                    cnt++;
                }
            }
            return cnt;
        }

        // not used
        public int CheckDuplicates()
        {
            int cnt = 0;

            for (int i = 0; i < numInPop; i++)
                for (int x = 0; x < numInPop; x++)
                {
                    int gg = 1;
                    for (int f = 0; f < G.genomeLen; f++)
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
            int k = -999999;
            int j = 9999999;
            for (int i = 0; i < numInPop; i++)
            {
                if (k == -1)
                {
                    if (pp[i].score != -999999)
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

        public int findMidScore()
        {
            scoreDif = (-findLowestScore() - -findHighestScore());

            midScore = highScore - scoreDif / 2; 

            return midScore;
        }


        public int pickRandom()
        {
            //ignores scores of -1
            for (int i = 0; i < numInPop * 3; i++)
            {
                int k = G.rnd.Next(0, G.pop.numInPop);
                if (pp[k].score == -999999) continue;
                {
                    return k;
                }

            }
            for (int i = 0; i < numInPop ; i++)
            {
                int k = i;
                if (pp[k].score == -999999) continue;
                {
                    return k;
                }
            }
            System.Windows.Forms.MessageBox.Show("Invalid pickRandom");
            return 0;
        }
    }

}
