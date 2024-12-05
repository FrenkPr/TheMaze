using UnityEngine;

//wall side bitmask to check what type of wall to remove
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
    private static int mazeNumRows = 0;
    private static int mazeNumCols = 0;
    private static WallSide[,] cellWallSides;

    //tuple values:
    //- deltaX: move along the x of the cells grid
    //- deltaY: move along the y of the cells grid
    //- currentBitmask: the bitmask that will be assigned to the current cell
    //- neighbourBitmask: the bitmask that will be assigned to the neighbour of the current cell
    //private static Tuple<int, int, WallSide, WallSide> goUp;
    //private static Tuple<int, int, WallSide, WallSide> goDown;
    //private static Tuple<int, int, WallSide, WallSide> goToLeft;
    //private static Tuple<int, int, WallSide, WallSide> goToRight;

    //private static Tuple<int, int, WallSide, WallSide>[] directions;

    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goUp;
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goDown;
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goToLeft;
    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask) goToRight;

    private static (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask)[] directions;

    private static void RemoveMazeWalls(int? x = null, int? y = null)
    {
        if (x == null)
        {
            x = Random.Range(0, mazeNumCols);
        }

        if (y == null)
        {
            y = Random.Range(0, mazeNumRows);
        }

        ArrayUtilities.Shuffle(directions);

        foreach (var dir in directions)
        {
            int nextX = (int)x + dir.deltaX;
            int nextY = (int)y + dir.deltaY;

            if (!IsCellOutOfBounds(nextX, nextY) && !HasCellBeenVisited(nextX, nextY))
            {
                cellWallSides[(int)y, (int)x] &= dir.currentBitmask;
                cellWallSides[nextY, nextX] &= dir.neighbourBitmask;

                RemoveMazeWalls(nextX, nextY);
            }
        }
    }

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

    private static bool HasCellBeenVisited(int x, int y)
    {
        return cellWallSides[y, x] != WallSide.ALL_WALLS;
    }

    public static WallSide[,] GenerateRandomMazeBitmask(int numRows, int numCols)
    {
        mazeNumRows = numRows;
        mazeNumCols = numCols;

        //read declaration variables section to understand better the usage
        //of the tuple variables
        //goUp = new Tuple<int, int, WallSide, WallSide>(0, -1, WallSide.UP, WallSide.DOWN);
        //goDown = new Tuple<int, int, WallSide, WallSide>(0, 1, WallSide.DOWN, WallSide.UP);
        //goToLeft = new Tuple<int, int, WallSide, WallSide>(-1, 0, WallSide.LEFT, WallSide.RIGHT);
        //goToRight = new Tuple<int, int, WallSide, WallSide>(1, 0, WallSide.RIGHT, WallSide.LEFT);

        goUp = (0, -1, WallSide.UP, WallSide.DOWN);
        goDown = (0, 1, WallSide.DOWN, WallSide.UP);
        goToLeft = (-1, 0, WallSide.LEFT, WallSide.RIGHT);
        goToRight = (1, 0, WallSide.RIGHT, WallSide.LEFT);

        directions = new (int deltaX, int deltaY, WallSide currentBitmask, WallSide neighbourBitmask)[4];

        directions[0] = goUp;
        directions[1] = goDown;
        directions[2] = goToLeft;
        directions[3] = goToRight;

        //initializing maze cells as a bidimensional array
        cellWallSides = new WallSide[mazeNumRows, mazeNumCols];

        //inits each cell with all walls
        for (int y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numCols; x++)
            {
                cellWallSides[y, x] = WallSide.ALL_WALLS;
            }
        }

        RemoveMazeWalls();

        return cellWallSides;
    }
}
