using UnityEngine;
using System.Collections;
using System;

public class MazeGenerator
{
	static int mazewidth = 0;
	static int mazeheight = 0;
	static string[,] maze;
	static string MAZEWALL = "#";
	static string MAZEPATH = " ";
	static string MAZESTART = "S";
	static string MAZEFINISH = "F";
	static double FINISHDISTANCE = 0.4;
	static double COMPLEXITY = 0.2;
	static int GAP = 3;
	static int[] bigobjectwidth;
	static int[] bigobjectheight;
	static int totalbigobject;

	static System.Random rand = new System.Random();

	#region maze generator - recursive division
	static void makeMaze()
	{
		makeMaze(0, mazewidth - 1, 0, mazeheight - 1);
	}


	//behind the scences actual mazemaking
	static void makeMaze(int left, int right, int top, int bottom)
	{
		int width = right - left;
		int height = bottom - top;

		//makes sure there is still room to divide, then picks the best
		//direction to divide into
		if (width > 2 && height > 2)
		{

			if (width > height)
				divideVertical(left, right, top, bottom);

			else if (height > width)
				divideHorizontal(left, right, top, bottom);

			else if (height == width)
			{
				int pickOne = rand.Next(0, 1);

				if (pickOne == 0)
					divideVertical(left, right, top, bottom);
				else
					divideHorizontal(left, right, top, bottom);
			}
		}
		else if (width > 2 && height <= 2)
		{
			divideVertical(left, right, top, bottom);
		}
		else if (width <= 2 && height > 2)
		{
			divideHorizontal(left, right, top, bottom);
		}
	}


	static void divideVertical(int left, int right, int top, int bottom)
	{
		//find a random point to divide at
		//must be even to draw a wall there
		int divide = left + 2 + rand.Next((right - left - 1) / 2) * 2;

		//draw a line at the halfway point
		for (int i = top; i < bottom; i++)
		{
			maze[i, divide] = MAZEWALL;
		}

		//get a random odd integer between top and bottom and clear it
		int clearSpace = top + rand.Next((bottom - top) / 2) * 2 + 1;

		maze[clearSpace, divide] = MAZEPATH;

		makeMaze(left, divide, top, bottom);
		makeMaze(divide, right, top, bottom);
	}

	static void divideHorizontal(int left, int right, int top, int bottom)
	{
		//find a random point to divide at
		//must be even to draw a wall there
		int divide = top + 2 + rand.Next((bottom - top - 1) / 2) * 2;
		if (divide % 2 == 1)
			divide++;

		//draw a line at the halfway point
		for (int i = left; i < right; i++)
		{
			maze[divide, i] = MAZEWALL;
		}

		//get a random odd integer between left and right and clear it
		int clearSpace = left + rand.Next((right - left) / 2) * 2 + 1;

		maze[divide, clearSpace] = MAZEPATH;

		//recur for both parts of the newly split section
		makeMaze(left, right, top, divide);
		makeMaze(left, right, divide, bottom);
	}
	#endregion

	#region insert big object
	static void InsertBigObject(string marker, int width, int height, int gap)
	{
		int startx = 0;
		int starty = 0;
		bool valid = false;
		while (!valid)
		{
			valid = true;
			startx = rand.Next(1, mazewidth - 1 - width);
			starty = rand.Next(1, mazeheight - 1 - height);

			for (int y = starty - gap; y < starty + height + gap; y++)
			{
				for (int x = startx - gap; x < startx + width + gap; x++)
				{
					if (!((maze[y, x] == MAZEPATH) || (maze[y, x] == MAZEWALL)))
					{
						valid = false;
					}
				}
			}
		}

		for (int y = starty; y < starty + height; y++)
		{
			for (int x = startx; x < startx + width; x++)
			{
				maze[y, x] = marker;
			}
		}
	}
	#endregion

	public static string[,] CreateMaze(int p_mazesize, float p_complexity, int p_totalbigobject, bool p_hasstartpoint, bool p_hasfinishpoint) //this.exe <mazesize> <complexity> <totalbigobject>
	{
		int mazesize = 0;
		bool debug = false;
		if (debug)
		{

		}
		else
		{
			mazesize = p_mazesize;

		}

		mazewidth = mazesize;
		mazeheight = mazesize;
		mazewidth = mazewidth * 2 + 1;
		mazeheight = mazeheight * 2 + 1;

		//Console.Write("Maze Complexity (Easy=0.3 to Difficult=0) = ");
		COMPLEXITY = p_complexity;
		//totalbigobject = Convert.ToInt32(Console.ReadLine());
		try
		{
			totalbigobject = p_totalbigobject;
		}
		catch
		{
			totalbigobject = 0;
		}

		bigobjectwidth = new int[totalbigobject];
		bigobjectheight = new int[totalbigobject];
		for (int a = 0; a < totalbigobject; a++)
		{
			string inputtemp = Console.ReadLine();
			bigobjectwidth[a] = Convert.ToInt32(inputtemp.Split(' ')[0]);
			bigobjectheight[a] = Convert.ToInt32(inputtemp.Split(' ')[1]);
		}

		maze = new string[mazeheight, mazewidth];

		int homewidth = 9;
		int homeheight = 5;

		#region maze initialization
		//initialize maze
		for (int y = 0; y < mazeheight; y++)
		{
			for (int x = 0; x < mazewidth; x++)
			{
				maze[y, x] = " ";
				if ((y % 2 == 0) && (x % 2 == 0))
				{
					maze[y, x] = "o";
				}
			}
		}

		//draw border horizontal wall
		for (int x = 0; x < mazewidth; x++)
		{
			maze[0, x] = "#";
			maze[mazeheight - 1, x] = "#";
		}

		//draw border vertical wall
		for (int y = 0; y < mazeheight; y++)
		{
			maze[y, 0] = "#";
			maze[y, mazewidth - 1] = "#";
		}
		#endregion

		makeMaze();

		#region complexity calculations
		//make hole based on complexity
		int nWALL = (mazewidth - 1) * (mazeheight - 1) - (homewidth * homeheight);
		int nHOLE = 0;
		while (nHOLE < nWALL * COMPLEXITY)
		{
			int holeX = rand.Next(2, mazewidth - 3);
			int holeY = rand.Next(2, mazeheight - 3);

			if (maze[holeY, holeX] == MAZEWALL)
			{
				int nNEIGHBOURS = 0;
				if (maze[holeY - 1, holeX] == MAZEPATH)
				{
					if (maze[holeY + 1, holeX] == MAZEPATH)
					{
						nNEIGHBOURS++;
					}
					nNEIGHBOURS++;
					if (maze[holeY, holeX - 1] == MAZEPATH)
					{
						continue;
					}
					if (maze[holeY, holeX + 1] == MAZEPATH)
					{
						continue;
					}
				}
				else if (maze[holeY, holeX - 1] == MAZEPATH)
				{
					if (maze[holeY, holeX + 1] == MAZEPATH)
					{
						nNEIGHBOURS++;
					}
					nNEIGHBOURS++;
					if (maze[holeY + 1, holeX] == MAZEPATH)
					{
						continue;
					}
					if (maze[holeY - 1, holeX] == MAZEPATH)
					{
						continue;
					}
				}
				if (nNEIGHBOURS == 2)
				{
					maze[holeY, holeX] = MAZEPATH;
					nHOLE++;
				}
			}
		}
		#endregion

		if (p_hasstartpoint)
		{
			#region start and finish position
			//randomize start position
			int homex = rand.Next(3, mazewidth - 4 - homewidth);
			//homex = (mazewidth-homewidth) / 2;
			homex = homex + (homex + 1) % 2;
			int homey = rand.Next(3, mazeheight - 4 - homeheight);
			//homey = (mazeheight -homeheight)/ 2;
			homey = homey + (homey + 1) % 2;

			for (int y = 0; y < homeheight; y++)
			{
				for (int x = 0; x < homewidth; x++)
				{
					maze[homey + y, homex + x] = MAZESTART;
				}
			}

			int homexc = (2 * homex + homewidth) / 2;
			int homeyc = (2 * homey + homeheight) / 2;
		}
		//set finish line
		bool finishlinefound = false;
		double distanceMaze = (Math.Sqrt(mazewidth * mazewidth + mazeheight * mazeheight)) * FINISHDISTANCE;
		//Console.WriteLine(distanceMaze.ToString());
		while (!finishlinefound)
		{
			int flx = rand.Next(1, mazewidth - 2);
			int fly = rand.Next(1, mazeheight - 2);

			//double distanceF = Math.Sqrt(Math.Pow(flx - homexc, 2) + Math.Pow(fly - homeyc, 2)) + 2;

			//Console.Write((flx - homexc).ToString()+" "+ (fly - homeyc).ToString() + " ");
			//Console.WriteLine(flx.ToString()+" " + fly.ToString()+" "+distanceF.ToString());

			//if (distanceF > distanceMaze)
			//{
			if (maze[fly, flx] == MAZEPATH)
			{
				maze[fly, flx] = MAZEFINISH;
				break;
			}
			//}
		}
		#endregion

		#region Big Object Insertion

		for (int a = 0; a < totalbigobject; a++)
		{
			InsertBigObject(a.ToString(), bigobjectwidth[a], bigobjectheight[a], GAP);
		}

		#endregion


		return maze;
		//Console.ReadLine();
		//Console.Clear();
	}
}