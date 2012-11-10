using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace SC2012_Assign
{
    /// <summary>
    /// Global class - lots of global static variables 
    /// </summary>
    public class G
    {
        public static Maze maze;
        public static PathInMaze path; // this variable is for debugging and example you wont need it in final code
        public static Random rnd; // a global random number
        public static Population pop;

        public static double mutationPercent;
        public static int generation;
        public static int generations;
        public static int mutations;
        public static int weaklingNum;
        public static int dupNum;
        public static int bestScore;

        public static bool run = false;
        public static bool stopWhenPathFound;

        public static void initGlobals()
        {
            rnd = new Random();
        }
    }

    public class PathInMaze
    {
        public int curX; // current position
        public int curY;
        public int curDirection; // 0=Right(x+)  1=Down(y+)  2=Left(x+)  3=Up(y-)

        public Maze mazz;
        
        public int[] pathx;
        public int[] pathy;       
        public int lengthOfpath;
        public int maxLengthOfpath;

        public int divi = 2; // a divisor to help find middle of block

        public PathInMaze(Maze m)
        {
            mazz = m;
            maxLengthOfpath = mazz.xSize * mazz.ySize; // that should be enough room
            pathx = new int[maxLengthOfpath]; // that should be enough room
            pathy = new int[maxLengthOfpath]; // that should be enough room
            curDirection = 0;
            curX = mazz.startPosx;
            curY = mazz.startPosy;
            lengthOfpath = 1;
            pathx[0] = curX;
            pathy[0] = curY;
        }

        public const int resSucessEnd = 0;
        public const int resSucessTurn = 1;
        public const int resSucess = 2;
        public const int resFailWall = 3;
        public const int resFailOut = 4;
        public const int resFailOverPath = 5;
        public const int resInvalid = 6;

        /// 0 = Success got to end
        /// 1 = move was successfull but did not move it was a turn command
        /// 2 = successfull and moved 1 square
        /// 3 = fail hit a wall (did not move)
        /// 4 = fail moved out of maze (did not move)
        /// 5 = fail moved over previous path (but it did move)
        /// 6 = not a valid action - fail

        /// <summary>
        /// Moves or turns a point in the maze it returns the result of the action
        /// actions are:
        /// 0= Move Right(x+)
        /// 1= Move Down(y+)
        /// 2= Move Left(x+)
        /// 3= Move Up(y-)
        /// 4= move forward
        /// 5= turn right
        /// 6= turn left
        /// 7= about face (turn right twice)
        /// 
        /// return values:
        /// 0 = Success got to end
        /// 1 = move was successfull but did not move it was a turn command
        /// 2 = successfull and moved 1 square
        /// 3 = fail hit a wall (did not move)
        /// 4 = fail moved out of maze (did not move)
        /// 5 = fail moved over previous path
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public int movement(int action)
        {
            if (action == 0) // move right
            {
                return tryMove(curX + 1, curY);
            }

            if (action == 1) // move down
            {
                return tryMove(curX, curY + 1);
            }

            if (action == 2) // move left
            {
                return tryMove(curX-1, curY);
            }

            if (action == 3) // move up
            {
                return tryMove(curX, curY - 1);
            }

            if (action == 4) // move forward
            {
                if (curDirection == 0) return tryMove(curX + 1, curY); // right
                if (curDirection == 1) return tryMove(curX, curY+1); // down
                if (curDirection == 2) return tryMove(curX - 1, curY); // left
                if (curDirection == 3) return tryMove(curX , curY-1); // up
            }

            if (action == 5) // turn right
            {
                curDirection++;
                if (curDirection > 3) curDirection = 0;
                return resSucessTurn;
            }
            
            if (action == 6) // turn left
            {                
                curDirection--;
                if (curDirection < 0) curDirection = 3;
                return resSucessTurn;
            }

            if (action == 7) // about face
            {
                curDirection++;
                if (curDirection > 3) curDirection = 0;
                curDirection++;
                if (curDirection > 3) curDirection = 0;
                return resSucessTurn;
            }

            return resInvalid;
        }

        protected int tryMove(int x, int y)
        {
            int ter = mazz.getTerrain(x, y);
            if (ter == Maze.terOutsideMaze) return resFailOut;
            if (ter == Maze.terImpassable) return resFailWall;
            if (ter == Maze.terEndPos)
            {
                doMove(x, y);
                return resSucessEnd;
            }
            if (ter == Maze.terPassable || ter == Maze.terStartPos)
            {
                if (onPath(x,y)) 
                    {
                    doMove(x, y);
                    return resFailOverPath;
                    }
                doMove(x, y);
                return resSucess;
            }
            return resInvalid;
        }

        protected bool onPath(int x, int y)
        {
            if (lengthOfpath == 0) return false;
            for (int i = 0; i < lengthOfpath; i++)
            {
                if (x == pathx[i] && y == pathy[i]) return true;
            }
            return false;
        }

        /// <summary>
        /// Counts the number of times a place appears on a path 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int countOnPath(int x, int y)
        {
            int rc = 0;
            if (lengthOfpath == 0) return 0;
            for (int i = 0; i < lengthOfpath; i++)
            {
                if (x == pathx[i] && y == pathy[i]) rc++;
            }
            return rc;
        }

        protected void doMove(int x, int y)
        {
            pathx[lengthOfpath] = x;
            pathy[lengthOfpath] = y;
            lengthOfpath++;
            curX = x;
            curY = y;
        }

    }

    public class Maze
    {
        /// <summary>
        /// Terrain values
        /// 0 = passable
        /// 1 = impassable
        /// 2 = start pos
        /// 3 = end pos
        /// 4 = outside maze (not used but returned from getTerrain)
        /// </summary>
        /// 

        public const int terPassable = 0;
        public const int terImpassable = 1;
        public const int terStartPos = 2;
        public const int terEndPos = 3;
        public const int terOutsideMaze = 4;

        public int[,] blocks;
        public int xSize;
        public int ySize;

        public int startPosx;
        public int startPosy;
        public int endPosx;
        public int endPosy;

        public int drawSizeX; // size of a block on the screen
        public int drawSizeY; // size of a block on the screen


        public Maze()
        {
            //set defaults
            setMaze0();
        }

        public Maze(string mazeStyle)
        {
            if (mazeStyle == "Maze0") setMaze0();
            if (mazeStyle == "Maze1") setMaze1();
            if (mazeStyle == "Maze2") setMaze2();
            if (mazeStyle == "Maze3") setMaze3();
        }

        public void setMaze0() // the default maze
        {
            allocateMaze(7, 7);
            for (int y = 0; y < 4; y++)
            {
                blocks[1, y] = terImpassable;
                blocks[3, y + 3] = terImpassable;
                blocks[5, y + 2] = terImpassable;
            }
            setStart(0, 3);
            setEnd(6, 3);

        }

        public void setMaze1() // the default maze
        {
            allocateMaze(9, 7);
            for (int y = 0; y < 4; y++)
            {
                blocks[1, y] = terImpassable;
                blocks[3, y + 3] = terImpassable;
                blocks[5, y + 2] = terImpassable;
                blocks[7, y] = terImpassable;
            }
            setStart(0, 3);
            setEnd(8, 3);

        }

        public void setMaze2() // the default maze
        {
            allocateMaze(10, 8);
            for (int y = 0; y < 4; y++)
            {
                blocks[1, y] = terImpassable;
                blocks[3, y + 3] = terImpassable;
                blocks[5, y + 2] = terImpassable;
                blocks[7, y] = terImpassable;
                blocks[9, y + 1] = terImpassable;
            }
            blocks[6, 4] = terImpassable;
            blocks[4, 5] = terImpassable;
            setStart(0, 3);
            setEnd(9, 3);

        }

        public void setMaze3() // the default maze
        {
            allocateMaze(20, 15);
            for (int y = 0; y < 6; y++)
            {
                blocks[1, y] = terImpassable;
                blocks[3, y + 3] = terImpassable;
                blocks[9, y + 3] = terImpassable;
                blocks[7, y] = terImpassable;
                blocks[9, y + 1] = terImpassable;
                blocks[18, y + 9] = terImpassable;
                blocks[y + 10, 4] = terImpassable;
                blocks[y + 8, 6] = terImpassable;
            }
            blocks[7, 6] = terImpassable;
            blocks[4, 5] = terImpassable;
            setStart(0, 3);
            setEnd(9, 3);

        }



        public void allocateMaze(int Xsize, int Ysize)
        {
            blocks = new int[Xsize,Ysize];
            xSize = Xsize;
            ySize = Ysize;
            for (int y=0; y<ySize; y++)
                for (int x = 0; x < xSize; x++)
                {
                    blocks[x, y] = terPassable;
                }
        }

        /// <summary>
        /// This computes the size of a cell in screen pixels - given the screen area
        /// for good reasons it rounds down
        /// </summary>
        /// <param name="xSz"></param>
        /// <param name="ySz"></param>
        public void computeDrawSize(int xSz, int ySz)
        {
         drawSizeX = xSz/xSize;
         drawSizeY = ySz/ySize;
        }

        
        public System.Drawing.Color getColor(int x, int y)
        {
            int t = getTerrain(x, y);
            if (t == terImpassable) return System.Drawing.Color.DarkGray;
            if (t == terPassable) return System.Drawing.Color.Wheat;
            if (t == terStartPos) return System.Drawing.Color.Green;
            if (t == terEndPos) return System.Drawing.Color.Red;
            if (t == terOutsideMaze) return System.Drawing.Color.Yellow;
            return System.Drawing.Color.Teal;
        }
  
        /// <summary>
        /// gets the terrain for a specific location with bounds checking
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int getTerrain(int x, int y)
        {
            if (x < 0) return terOutsideMaze;
            if (y < 0) return terOutsideMaze;
            if (x >= xSize) return terOutsideMaze;
            if (y >= ySize) return terOutsideMaze;
            return blocks[x, y];
        }

        public void setTerrain(int x, int y, int terrainVal)
        {
            blocks[x, y] = terrainVal;
        }

        public void setStart(int x, int y)
        {
            blocks[x, y] = terStartPos;
            startPosx = x;
            startPosy = y;
        }

        public void setEnd(int x, int y)
        {            
            blocks[x, y] = terEndPos;
            endPosx = x;
            endPosy = y;
        }


    }
}
