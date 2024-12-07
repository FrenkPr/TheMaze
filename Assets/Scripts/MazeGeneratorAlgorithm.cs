using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wall side bitmask to check what type of wall to remove
[System.Flags]
public enum WallSide
{
    UP = 1,    //0b0001
    DOWN = 2,  //0b0010
    LEFT = 4,  //0b0100
    RIGHT = 8, //0b1000
    ALL_WALLS = 15,  //0b1111
}

public static class MazeGeneratorAlgorithm
{
    //declaring static variables
    private static int mazeNumRows = 10;
    private static int mazeNumCols = 10;
    private static WallSide[,] cellWallSides;

    //tuple values:
    //- deltaX: move along the x of the cells grid
    //- deltaY: move along the y of the cells grid
    //- currentBitmask: the bitmask that will be assigned to the current cell to remove one of its walls
    //- neighbourBitmask: the bitmask that will be assigned to the neighbour of the current cell to remove one of its walls
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goUp;
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goDown;
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goToLeft;
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goToRight;

    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask)[] directions;


    //here we use the DFS algorithm with Iterative implementation (with stack) to generate the maze

    //Choose the initial cell, mark it as visited and push it to the stack
    //While the stack is not empty
    //Pop a cell from the stack and make it a current cell
    //If the current cell has any neighbours which have not been visited
    //Push the current cell to the stack
    //Choose one of the unvisited neighbours
    //Remove the wall between the current cell and the chosen cell
    //Mark the chosen cell as visited and push it to the stack
    private static void RemoveMazeWalls()
    {
        //Choose the initial cell, mark it as visited and push it to the stack
        int startX = Random.Range(0, mazeNumCols);
        int startY = Random.Range(0, mazeNumRows);

        Stack<(int x, int y)> cellsToVisit = new Stack<(int x, int y)>();

        cellsToVisit.Push((startX, startY));

        //While the stack is not empty
        while (cellsToVisit.Count > 0)
        {
            //Pop a cell from the stack and make it a current cell
            (int x, int y) currentCell = cellsToVisit.Pop();

            //here we shuffle the neighbour directions to choose
            //a random unvisited neighbour while iterating each one of them
            directions.Shuffle();

            foreach (var dir in directions)
            {
                int nextX = currentCell.x + dir.deltaX;
                int nextY = currentCell.y + dir.deltaY;

                //If the current cell has any neighbours which have not been visited
                //Choose one of the unvisited neighbours

                //if we find at least one maze cell that has not been visited and that is not out of bounds
                if (!IsCellOutOfBounds(nextX, nextY) && !HasCellBeenVisited(nextX, nextY))
                {
                    //Push the current cell to the stack
                    cellsToVisit.Push(currentCell);

                    //Remove the wall between the current cell and the chosen cell
                    cellWallSides[currentCell.x, currentCell.y] &= ~dir.currentBitmask;
                    cellWallSides[nextX, nextY] &= ~dir.neighbourBitmask;

                    //Mark the chosen cell as visited and push it to the stack
                    cellsToVisit.Push((nextX, nextY));

                    break;
                }
            }
        }
    }

    //checking if a cell is out of bounds
    private static bool IsCellOutOfBounds(int x, int y)
    {
        if (x < 0 || x >= mazeNumCols)
        {
            return true;
        }

        if (y < 0 || y >= mazeNumRows)
        {
            return true;
        }

        return false;
    }

    //checking if a cell has been visited or not via the WallSide enum
    private static bool HasCellBeenVisited(int x, int y)
    {
        return !cellWallSides[x, y].HasFlag(WallSide.ALL_WALLS);
    }

    //set the maze size on main menu scene
    public static void SetMazeSize(int numRows, int numCols)
    {
        mazeNumRows = numRows;
        mazeNumCols = numCols;
    }

    //inits and generates a random maze bitmask
    public static WallSide[,] GenerateRandomMazeBitmask()
    {
        //read declaration variables section to understand better the usage
        //of the tuple variables
        goUp = (0, -1, WallSide.UP, WallSide.DOWN);
        goDown = (0, 1, WallSide.DOWN, WallSide.UP);
        goToLeft = (-1, 0, WallSide.LEFT, WallSide.RIGHT);
        goToRight = (1, 0, WallSide.RIGHT, WallSide.LEFT);

        directions = new (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask)[4];

        directions[0] = goUp;
        directions[1] = goDown;
        directions[2] = goToLeft;
        directions[3] = goToRight;

        //mazeNumCols = 10;
        //mazeNumRows = 50;

        //initializing maze cells as a bidimensional array
        cellWallSides = new WallSide[mazeNumCols, mazeNumRows];

        //inits each cell with all walls
        for (int y = 0; y < mazeNumRows; y++)
        {
            for (int x = 0; x < mazeNumCols; x++)
            {
                cellWallSides[x, y] = WallSide.ALL_WALLS;
            }
        }

        RemoveMazeWalls();

        return cellWallSides;
    }
}
