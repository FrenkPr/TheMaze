using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class MazeGraphicGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mazeCell;
    [SerializeField] private int numRows;
    [SerializeField] private int numCols;
    [SerializeField] private float nextCellOffset;

    public void GenerateMaze()
    {
        //WallSide[,] mazeWallSides = MazeGeneratorAlgorithm.GenerateRandomMazeBitmask(numRows, numCols);

        for (int z = 0; z < numRows; z++)
        {
            for (int x = 0; x < numCols; x++)
            {
                GameObject cellSpawnedRoot = Instantiate(mazeCell, transform, false);
                Vector3 newCellLocalPosition = cellSpawnedRoot.transform.localPosition;

                newCellLocalPosition.x += nextCellOffset * x;
                newCellLocalPosition.z += nextCellOffset * z;
                
                cellSpawnedRoot.transform.localPosition = newCellLocalPosition;

                //for (int i = cellSpawnedRoot.transform.childCount - 1; i >= 0; i--)
                //{
                //    Debug.Log("removed from cell root, child: " + cellSpawnedRoot.transform.GetChild(i).name +
                //        "\nindex: " + i);
                //    cellSpawnedRoot.transform.GetChild(i).parent = transform;
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
