using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager
{
    private int[,] maze;
    private int[] playerPosition;
    private int[] goalPosition;
    private List<int[]> playerPath;
    private Random rand;

    public GameManager()
    {
        // Initialize random.
        rand = new Random();

        // Initialize maze.
        maze = new int[,] {
            { -1, -1, -1, -1, -1 },
            { -1,  0,  0,  0, -1 },
            { -1,  0, -1,  0, -1 },
            { -1,  0,  0,  0,  2 },
            { -1, -1, -1, -1, -1 }
        };

        // Initialize player position.
        playerPosition = GetInitialPosition();

        // Initialize player path.
        playerPath = new List<int[]>();
        playerPath.Add(new int[] { playerPosition[0], playerPosition[1] });


        // Initialize goal position.
        goalPosition = new int[2] { 3, 4 };
    }

    // Implement four actions: Up (0), Down (1), Left (2), Right (3).
    public bool Action(int action)
    {
        // Check the current position.
        int[] newPosition = new int[2] { playerPosition[0], playerPosition[1] };

        switch (action)
        {
            case 0: // Up
                newPosition[0]--;
                break;
            case 1: // Down
                newPosition[0]++;
                break;
            case 2: // Left
                newPosition[1]--;
                break;
            case 3: // Right
                newPosition[1]++;
                break;
        }

        if (maze[newPosition[0], newPosition[1]] == -1)
        {
            // It's a wall.
            return false;
        }

        // It's a valid move, update player position.
        playerPosition = newPosition;

        // It's a valid move, update player position and path.
        playerPosition = newPosition;
        playerPath.Add(new int[] { playerPosition[0], playerPosition[1] });

        return true;
    }

    // Check if the player has reached the goal.
    public bool IsGoal()
    {
        if (playerPosition[0] == goalPosition[0] && playerPosition[1] == goalPosition[1])
        {
            return true;
        }

        return false;
    }


    private int[] GetInitialPosition()
    {
        return new int[] { 1, 1 };
    }

    private int[] GetRandomStartPosition()
    {
        List<int[]> emptySpaces = new List<int[]>();

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 0)
                {
                    emptySpaces.Add(new int[] { i, j });
                }
            }
        }

        return emptySpaces[rand.Next(emptySpaces.Count)];
    }

    public void Reset()
    {
        // Reset player position.
        playerPosition = GetInitialPosition();

        // Reset player path.
        playerPath.Clear();
        playerPath.Add(new int[] { playerPosition[0], playerPosition[1] });
    }

    public int[] GetState()
    {
        int[] flattenedMaze = new int[maze.Length];

        int index = 0;
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                flattenedMaze[index++] = maze[i, j];
            }
        }

        return flattenedMaze;
    }


    public void PrintMaze()
    {
        //Console.Clear(); // Clear the console.

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (i == playerPosition[0] && j == playerPosition[1])
                    Console.Write("P "); // Player's current position.
                else if (playerPath.Any(p => p[0] == i && p[1] == j))
                    Console.Write(". "); // Player path.
                else if (maze[i, j] == -1)
                    Console.Write("# "); // Wall.
                else if (maze[i, j] == 0)
                    Console.Write("  "); // Open space.
                else if (maze[i, j] == 2)
                    Console.Write("G "); // Goal.
            }
            Console.WriteLine();
        }
    }

}
