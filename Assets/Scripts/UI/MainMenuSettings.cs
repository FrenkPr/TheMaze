using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuSettings : MonoBehaviour
{
    //declaring instance variables
    [SerializeField] private TMP_Text mazeNumRowsTextObject;
    [SerializeField] private TMP_Text mazeNumColsTextObject;
    private int mazeNumRows = 10;
    private int mazeNumCols = 10;

    //this function is called in the MainMenuScene S_MazeNumRows element on slider value changed
    public void SetMazeRows(float numRows)
    {
        //we get the text info using Substring method: "Number of maze cells per row: "
        string numRowsTextInfo = mazeNumRowsTextObject.text.Substring(0, mazeNumRowsTextObject.text.LastIndexOf(':') + 2);
        
        mazeNumRowsTextObject.text = numRowsTextInfo + ((int)numRows).ToString();
        mazeNumRows = (int)numRows;
    }

    //this function is called in the MainMenuScene S_MazeNumCols element on slider value changed
    public void SetMazeCols(float numCols)
    {
        //we get the text info using Substring method: "Number of maze cells per column: "
        string numColsTextInfo = mazeNumColsTextObject.text.Substring(0, mazeNumColsTextObject.text.LastIndexOf(':') + 2);
        
        mazeNumColsTextObject.text = numColsTextInfo + ((int)numCols).ToString();
        mazeNumCols = (int)numCols;
    }

    //this function is called in the MainMenuScene when BTN_GenerateMaze button is clicked
    public void Quit()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }

    //this function is called in the MainMenuScene when BTN_GenerateMaze button is clicked
    public void OnGenerateMazeButtonClicked()
    {
        MazeGeneratorAlgorithm.SetMazeSize(mazeNumRows, mazeNumCols);
        SceneManager.LoadScene("TheMazeScene");
    }
}
