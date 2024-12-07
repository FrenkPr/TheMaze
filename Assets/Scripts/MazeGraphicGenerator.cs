using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class MazeGraphicGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject mazeCell;
    [SerializeField] private float nextCellOffset;

    public void DrawMaze()
    {
        WallSide[,] mazeWallSides = MazeGeneratorAlgorithm.GenerateRandomMazeBitmask();
        Dictionary<string, WallSide> wallSides = new Dictionary<string, WallSide>();

        wallSides["UP"] = WallSide.UP;
        wallSides["DOWN"] = WallSide.DOWN;
        wallSides["LEFT"] = WallSide.LEFT;
        wallSides["RIGHT"] = WallSide.RIGHT;

        //we tick the empty object maze generator as static
        gameObject.isStatic = true;

        //drawing floor based on maze position and size
        GameObject spawnedFloor = Instantiate(floor, transform);
        
        spawnedFloor.transform.localPosition = new Vector3(nextCellOffset * mazeWallSides.GetLength(0) + nextCellOffset, -4, -nextCellOffset * mazeWallSides.GetLength(1));
        //spawnedFloor.transform.localScale = new Vector3(nextCellOffset * (mazeWallSides.GetLength(0) * 0.5f) + 10, 1, nextCellOffset * (mazeWallSides.GetLength(1) * 0.5f) + 10);
        //spawnedFloor.transform.localScale = new Vector3(10, 1, nextCellOffset * (mazeWallSides.GetLength(1) * 0.5f) + 10);

        spawnedFloor.transform.localScale = new Vector3(nextCellOffset * 250, 1, nextCellOffset * 250);

        spawnedFloor.isStatic = true;

        //we use a nested loop to iterate each maze cell.
        //mazeWallSides.GetLength(0): maze cells per column
        //mazeWallSides.GetLength(1): maze cells per row
        for (int z = 0; z < mazeWallSides.GetLength(1); z++)
        {
            for (int x = 0; x < mazeWallSides.GetLength(0); x++)
            {
                GameObject cellSpawnedRoot = Instantiate(mazeCell, transform, false);
                Vector3 newCellLocalPosition = cellSpawnedRoot.transform.localPosition;

                newCellLocalPosition.x += nextCellOffset * x;
                newCellLocalPosition.z += -nextCellOffset * z;
                cellSpawnedRoot.transform.localPosition = newCellLocalPosition;

                cellSpawnedRoot.isStatic = true;

                for (int i = 0; i < cellSpawnedRoot.transform.childCount; i++)
                {
                    GameObject cellWall = cellSpawnedRoot.transform.GetChild(i).gameObject;
                    WallSide cellSpawnedWallSide = wallSides[cellSpawnedRoot.transform.GetChild(i).tag];

                    cellWall.isStatic = true;

                    //we check if the current cell wall is to destroy or not
                    if (!mazeWallSides[x, z].HasFlag(cellSpawnedWallSide))
                    {
                        cellWall.SetActive(false);
                    }
                }
            }
        }

        //we call this function to combine children meshes of the maze container all in one
        //and make in this way just one draw call (GPU optimization performance),
        //instead of thousands
        StaticBatchingUtility.Combine(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawMaze();
    }
}
