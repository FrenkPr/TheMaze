using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class MazeGraphicGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mazeCell;
    [SerializeField] private float nextCellOffset;

    public void GenerateMaze()
    {
        WallSide[,] mazeWallSides = MazeGeneratorAlgorithm.GenerateRandomMazeBitmask();
        Dictionary<string, WallSide> wallSides = new Dictionary<string, WallSide>();

        wallSides["UP"] = WallSide.UP;
        wallSides["DOWN"] = WallSide.DOWN;
        wallSides["LEFT"] = WallSide.LEFT;
        wallSides["RIGHT"] = WallSide.RIGHT;

        Debug.Log(mazeWallSides.GetLength(1) + "x" + mazeWallSides.GetLength(0));

        //we use a nested loop to iterate each maze cell.
        //mazeWallSides.GetLength(0): maze cells per row
        //mazeWallSides.GetLength(1): maze cells per column
        for (int z = 0; z < mazeWallSides.GetLength(0); z++)
        {
            for (int x = 0; x < mazeWallSides.GetLength(1); x++)
            {
                GameObject cellSpawnedRoot = Instantiate(mazeCell, transform, false);
                Vector3 newCellLocalPosition = cellSpawnedRoot.transform.localPosition;

                newCellLocalPosition.x += nextCellOffset * x;
                newCellLocalPosition.z += nextCellOffset * z;
                
                cellSpawnedRoot.transform.localPosition = newCellLocalPosition;

                //for (int i = 0; i < cellSpawnedRoot.transform.childCount; i++)
                //{
                //    WallSide cellSpawnedWallSide = wallSides[cellSpawnedRoot.transform.GetChild(i).tag];

                //    //we check if the current cell wall is to destroy or not
                //    //comparing the mazeWallSides and the cellSpawnedWallSide bits
                //    if (((uint)mazeWallSides[z, x] & (uint)cellSpawnedWallSide) != 0)
                //    {
                //        //Debug.Log("current row: " + z +
                //        //    "\ncurrent col: " + x +
                //        //    "\ncell wall side values: " + (uint)mazeWallSides[z, x] +
                //        //    "\ncell child wall side value: " + cellSpawnedWallSide.ToString() + ": " + (uint)cellSpawnedWallSide);

                //        GameObject cellWall = cellSpawnedRoot.transform.GetChild(i).gameObject;

                //        cellWall.transform.SetParent(null, true);
                //        Destroy(cellWall);

                //        i = 0;
                //    }
                //}
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
        GenerateMaze();
    }
}
