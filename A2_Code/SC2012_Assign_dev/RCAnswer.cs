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
        public bool firstrun = true;
        public bool firstrun2 = true;

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

            int mutstrat = 1;

            if (G.mutStrat == "MutStrat1")
                mutstrat = 1;
            
            if(G.mutStrat == "MutStrat2")
                mutstrat = 2;

            if (G.mutStrat == "MutStrat3")
                mutstrat = 3;

            if (G.mutStrat == "MutStrat4")
                mutstrat = 4;

            if (G.mutStrat == "MutStrat1&2")
                mutstrat = G.rnd.Next(1, 3);

            if (G.mutStrat == "MutStrat1&3")
            {
                mutstrat = G.rnd.Next(1, 3);
                if (mutstrat == 2)
                    mutstrat = 3;
            }

            if (G.mutStrat == "MutStrat2&3")
                mutstrat = G.rnd.Next(2, 3);

            if (G.mutStrat == "MutStrat1&2&3")
                mutstrat = G.rnd.Next(1, 4);

            if (G.mutStrat == "All")
                mutstrat = G.rnd.Next(1, 5);

            switch(mutstrat)
            {
                case 1:
                    MutStrat1();
                    break;
                case 2:
                    MutStrat2();
                    break;
                case 3:
                    MutStart3();
                    break;
                case 4:
                    MutStrat4();
                    break;
            }

            return child;
        }

        // select random genes and randomly change them
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

        // select 1 gene randomly and swap with another random gene unless equal then random gene - good middle ground
        private void MutStrat2()
        {
            int fpoint = G.rnd.Next(30);
            int lpoint = G.rnd.Next(30);

            if (lpoint == fpoint)
            {
                dna[lpoint].gene = G.rnd.Next(4);
            }
            else
            {
                int temp = dna[lpoint].gene;
                dna[lpoint].gene = dna[fpoint].gene;
                dna[fpoint].gene = temp;
            }
        }


        // swap gene order
        private void MutStart3()
        {
            for (int i = 0; i < G.genomeLen / 2; i++)
            {
                int temp = dna[i].gene;
                dna[i].gene = dna[G.genomeLen - i - 1].gene;
                dna[G.genomeLen - i - 1].gene = temp;
            }
        }

        // select 1 point randomly and change the operation randomly - too random
        private void MutStrat4()
        {
            int lpoint = G.rnd.Next(1, 15);
            if (lpoint == 1)
                lpoint += 3;
            if (lpoint == 2)
                lpoint += 2;
            if (lpoint == 3)
                lpoint += 1;

            int movDec = G.rnd.Next(4);

            for (int fpoint = lpoint - 1; fpoint < lpoint; fpoint++)
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

        private int calcDistance(PathInMaze p)
        {
            int dx = Math.Abs(p.curX - p.mazz.endPosx);
            int dy = Math.Abs(p.curY - p.mazz.endPosy);
            return (dx + dy);
        }

        public PathInMaze calcScore(Maze m)
        {
            PathInMaze p = new PathInMaze(m);
           
            score = 500;

            // manhattan dis calc on move & end point
            if (G.fitFunc == "FitFunc1")
            {
                #region FitFunc1

                if(firstrun2)
                    manhattanDisPoi = calcDistance(p);

                manhattanDisMov = calcDistance(p);

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

                    //if (mi == -1) break; // perhaps use -1 to indicate at end
                    int rc = p.movement(mi);

                    // resSucessEnd = Success got to end
                    // resSucessTurn  = move was successfull but did not move it was a turn command
                    // resSucess = successfull and moved 1 square
                    // resFailWall = fail hit a wall (did not move)
                    // resFailOut = fail moved out of maze (did not move)
                    // resFailOverPath = fail moved over previous path (but it did move)
                    // resInvalid = not a valid action - fail


                    int newmanhattanDisMov = calcDistance(p);

                    if (rc == PathInMaze.resSucessEnd)
                    {
                        foundEnd = true;
                        score = score * 50;
                        manhattanDisMov = newmanhattanDisMov;
                        break;
                    }

                    if (rc == PathInMaze.resFailOverPath)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score -= 1;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 2;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 1;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resSucess)
                    {
                        score = score + 2;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 4;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 5;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 1;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailWall)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score -= 2;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 2;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 5;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailOut)
                    {
                        score = score - 2;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score -= 2;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 2;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 1;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    #region not used
                    /*if (rc == PathInMaze.resSucessTurn)
                    {
                        score = score + 3;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov || newmanhattanDisMov == manhattanDisMov)
                            score -= 20;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    #endregion

                    if (rc == PathInMaze.resInvalid)
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Movement");
                        continue;
                    }
                    firstrun = false;
                }

            // manhattan dis calc on end point
            if (G.fitFunc == "FitFunc2")
            {
                #region FitFunc2

                if (firstrun2)
                    manhattanDisPoi = calcDistance(p);

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

                    //if (mi == -1) break; // perhaps use -1 to indicate at end
                    int rc = p.movement(mi);

                    // resSucessEnd = Success got to end
                    // resSucessTurn  = move was successfull but did not move it was a turn command
                    // resSucess = successfull and moved 1 square
                    // resFailWall = fail hit a wall (did not move)
                    // resFailOut = fail moved out of maze (did not move)
                    // resFailOverPath = fail moved over previous path (but it did move)
                    // resInvalid = not a valid action - fail

                    if (rc == PathInMaze.resSucessEnd)
                    {
                        foundEnd = true;
                        score = score * 50;
                        break;
                    }

                    if (rc == PathInMaze.resFailOverPath)
                    {
                        score = score - 50;
                        continue;
                    }

                    if (rc == PathInMaze.resSucess)
                    {
                        score = score + 5;
                        continue;
                    }

                    if (rc == PathInMaze.resFailWall)
                    {
                        score = score - 10;
                        continue;
                    }

                    if (rc == PathInMaze.resFailOut)
                    {
                        score = score - 10;
                        continue;
                    }

                    #region not used
                    /*if (rc == PathInMaze.resSucessTurn)
                    {
                        score = score + 3;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov || newmanhattanDisMov == manhattanDisMov)
                            score -= 20;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }*/
                    #endregion

                    if (rc == PathInMaze.resInvalid)
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Movement");
                        continue;
                    }
                    
                }

                int newmanhattanDisPoi = calcDistance(p);

                if (newmanhattanDisPoi < manhattanDisPoi)
                    score += 30;
                else if (newmanhattanDisPoi > manhattanDisPoi)
                    score -= 30;
                else if (newmanhattanDisPoi == manhattanDisPoi && !firstrun2)
                    score -= 10;

                //System.Diagnostics.Debug.WriteLine("Prev: " + manhattanDisPoi.ToString() + "\n Cur: " + newmanhattanDisPoi.ToString() + "\n");
                
                manhattanDisPoi = newmanhattanDisPoi;
                firstrun2 = false;

                #endregion
            }

            // manhattan dis calc on end point
            if (G.fitFunc == "FitFunc2")
            {
                #region FitFunc2

                if (firstrun2)
                    manhattanDisPoi = calcDistance(p);

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

                    //if (mi == -1) break; // perhaps use -1 to indicate at end
                    int rc = p.movement(mi);

                    // resSucessEnd = Success got to end
                    // resSucessTurn  = move was successfull but did not move it was a turn command
                    // resSucess = successfull and moved 1 square
                    // resFailWall = fail hit a wall (did not move)
                    // resFailOut = fail moved out of maze (did not move)
                    // resFailOverPath = fail moved over previous path (but it did move)
                    // resInvalid = not a valid action - fail

                    if (rc == PathInMaze.resSucessEnd)
                    {
                        foundEnd = true;
                        score = score * 50;
                        break;
                    }

                    if (rc == PathInMaze.resFailOverPath)
                    {
                        score = score - 5;
                        continue;
                    }

                    if (rc == PathInMaze.resSucess)
                    {
                        score = score + 2;
                        continue;
                    }

                    if (rc == PathInMaze.resFailWall)
                    {
                        score = score - 5;
                        continue;
                    }

                    if (rc == PathInMaze.resFailOut)
                    {
                        score = score - 2;
                        continue;
                    }

                    #region not used
                    /*if (rc == PathInMaze.resSucessTurn)
                    {
                        score = score + 3;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov || newmanhattanDisMov == manhattanDisMov)
                            score -= 20;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }*/
                    
                 #endregion

                 if (rc == PathInMaze.resInvalid)
                 {
                     System.Windows.Forms.MessageBox.Show("Invalid Movement");
                     continue;
                 }
                    
             }

                int newmanhattanDisPoi = calcDistance(p);

                if (newmanhattanDisPoi < manhattanDisPoi)
                {



                    score += 70;
                }
                else if (newmanhattanDisPoi > manhattanDisPoi)
                    score -= 40;
                else if (newmanhattanDisPoi == manhattanDisPoi && !firstrun2)
                    score -= 15;

             //System.Diagnostics.Debug.WriteLine("Prev: " + manhattanDisPoi.ToString() + "\n Cur: " + newmanhattanDisPoi.ToString() + "\n");

             manhattanDisPoi = newmanhattanDisPoi;
             firstrun2 = false;

             #endregion
            }

            // manhattan dis calc on move
            if (G.fitFunc == "FitFunc3")
            {
            #region FitFunc3

             manhattanDisMov = calcDistance(p);

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

                 //if (mi == -1) break; // perhaps use -1 to indicate at end
                 int rc = p.movement(mi);

                 // resSucessEnd = Success got to end
                 // resSucessTurn  = move was successfull but did not move it was a turn command
                 // resSucess = successfull and moved 1 square
                 // resFailWall = fail hit a wall (did not move)
                 // resFailOut = fail moved out of maze (did not move)
                 // resFailOverPath = fail moved over previous path (but it did move)
                 // resInvalid = not a valid action - fail


                 int newmanhattanDisMov = calcDistance(p);


                 if (rc == PathInMaze.resSucessEnd)
                 {
                     foundEnd = true;
                     score = score * 50;
                     //manhattanDisMov = newmanhattanDisMov;
                     break;
                 }

                 if (rc == PathInMaze.resFailOverPath)
                 {
                     score = score - 50;

                     if (newmanhattanDisMov < manhattanDisMov)
                         score += 10;
                     else if (newmanhattanDisMov > manhattanDisMov)
                         score -= 20;
                     else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                         score -= 10;

                     manhattanDisMov = newmanhattanDisMov;
                     continue;
                 }

                 if (rc == PathInMaze.resSucess)
                 {
                     score = score + 30;

                     if (newmanhattanDisMov < manhattanDisMov)
                         score += 50;
                     else if (newmanhattanDisMov > manhattanDisMov)
                         score -= 50;
                     else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                         score -= 15;

                     manhattanDisMov = newmanhattanDisMov;
                     continue;
                 }

                 if (rc == PathInMaze.resFailWall)
                 {
                     score = score - 20;

                     if (newmanhattanDisMov < manhattanDisMov)
                         score += 5;
                     else if (newmanhattanDisMov > manhattanDisMov)
                         score -= 20;
                     else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                         score -= 30;

                     manhattanDisMov = newmanhattanDisMov;
                     continue;
                 }

                 if (rc == PathInMaze.resFailOut)
                 {
                     score = score - 15;

                     if (newmanhattanDisMov < manhattanDisMov)
                         score -= 10;
                     else if (newmanhattanDisMov > manhattanDisMov)
                         score -= 20;
                     else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                         score -= 15;

                     manhattanDisMov = newmanhattanDisMov;
                     continue;
                 }

                 #region not used
                 /*if (rc == PathInMaze.resSucessTurn)
                 {
                     score = score + 3;

                     if (newmanhattanDisMov < manhattanDisMov)
                         score += 20;
                     else if (newmanhattanDisMov > manhattanDisMov || newmanhattanDisMov == manhattanDisMov)
                         score -= 20;

                     manhattanDisMov = newmanhattanDisMov;
                     continue;
                 }*/
                    #endregion

                    if (rc == PathInMaze.resInvalid)
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Movement");
                        continue;
                    }
                    firstrun = false;
                }

                #endregion
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
                if (pp[index].score < (midScore + (scoreDif / 3)) )
                {
                    //System.Diagnostics.Debug.WriteLine(pp[index].score.ToString());
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

        public int pickRandomFromHighScores()
        {
            for (int i = 0; i < numInPop * 3; i++)
            {
                int k = G.rnd.Next(0, G.pop.numInPop);
                if (pp[k].score == -999999 || pp[k].score < (midScore + (scoreDif / 2)) ) continue;
                {
                    return k;
                }

            }
            for (int i = 0; i < numInPop; i++)
            {
                int k = i;
                if (pp[k].score == -999999 || pp[k].score < (midScore  + (scoreDif / 2)) ) continue;
                {
                    return k;
                }
            }
            System.Windows.Forms.MessageBox.Show("Invalid pickRandom");
            return 0;
        }

        public int pickRandom()
        {
            //ignores scores of -999999
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
