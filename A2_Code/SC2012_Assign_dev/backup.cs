using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SC2012_Assign
{
    class backup
    {
        /*
        // manhattan dis calc on move & end point
            if (G.fitFunc == "FitFunc1")
            {
                #region FitFunc1

                int newmanhattanDisPoi = calcDistance(p);

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
                        //manhattanDisMov = newmanhattanDisMov;
                        break;
                    }

                    if (rc == PathInMaze.resFailOverPath)
                    {
                        score = score - 50;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 2;
                        
                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resSucess)
                    {
                        score = score + 5;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 3;
                        
                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailWall)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 2;
                        
                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailOut)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 2;
                        
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

                if (newmanhattanDisPoi < manhattanDisPoi)
                    score += 40;
                else if (newmanhattanDisPoi > manhattanDisPoi)
                    score -= 40;
                else if (newmanhattanDisPoi == manhattanDisPoi && !firstrun2)
                    score -= 5;

                manhattanDisPoi = newmanhattanDisPoi;
                firstrun2 = false;

                #endregion
            }

            // manhattan dis calc on end point
            if (G.fitFunc == "FitFunc2")
            {
                #region FitFunc2

                int newmanhattanDisPoi = calcDistance(p);

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
                    }*//*
                    #endregion

                    if (rc == PathInMaze.resInvalid)
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Movement");
                        continue;
                    }
                    
                }

                if (newmanhattanDisPoi < manhattanDisPoi)
                    score += 40;
                else if (newmanhattanDisPoi > manhattanDisPoi)
                    score -= 40;
                else if (newmanhattanDisPoi == manhattanDisPoi && !firstrun2)
                    score -= 5;

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
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 2;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resSucess)
                    {
                        score = score + 5;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 3;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailWall)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 2;

                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailOut)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 20;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 2;

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
                    }*//*
                    #endregion

                    if (rc == PathInMaze.resInvalid)
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Movement");
                        continue;
                    }
                    firstrun = false;
                }

                #endregion
            }*/

        #region old fitfunc

        /*
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
                        score = score - 50;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score -= 10;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 10;
                        
                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resSucess)
                    {
                        score = score + 50;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score += 50;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 55;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 5;
                        
                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailWall)
                    {
                        score = score - 20;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score -= 30;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 70;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 70;
                        
                        manhattanDisMov = newmanhattanDisMov;
                        continue;
                    }

                    if (rc == PathInMaze.resFailOut)
                    {
                        score = score - 10;

                        if (newmanhattanDisMov < manhattanDisMov)
                            score -= 10;
                        else if (newmanhattanDisMov > manhattanDisMov)
                            score -= 20;
                        else if (newmanhattanDisMov == manhattanDisMov && !firstrun)
                            score -= 20;
                        
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
                    }*//*
                    #endregion

                    if (rc == PathInMaze.resInvalid)
                    {
                        System.Windows.Forms.MessageBox.Show("Invalid Movement");
                        continue;
                    }
                    firstrun = false;
                }*/

        #endregion
    }
}
