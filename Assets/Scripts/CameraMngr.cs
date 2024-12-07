using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MoveCameraMngr : MonoBehaviour
{
    //position update instance variables
    [SerializeField] private float moveSpeed;
    private Vector3 moveVelocity;
    
    //toggle show mouse cursor debug
    private bool showMouseCursor;

    //pause menu instance variables
    [SerializeField] GameObject pauseMenuCanvas;
    private bool isInPause;

    //Input system controller
    public InputSysController InputsController { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //inits the class to manage the new input system
        InputsController = new InputSysController();

        //toggle show mouse cursor debug
        showMouseCursor = false;
    }

    //toggle pause menu when the relative button is triggered
    private void TogglePauseMenuTrigger()
    {
        if (!InputsController.OnInputTrigger("PauseMenu"))
        {
            return;
        }

        TogglePauseMenu();
    }

    //toggle pause menu function
    public void TogglePauseMenu()
    {
        isInPause = !isInPause;

        Time.timeScale = Convert.ToInt32(!isInPause);
        pauseMenuCanvas.SetActive(isInPause);

        //here we show/hide and lock/unlock mouse cursor
        Cursor.visible = isInPause;
        Cursor.lockState = isInPause ? CursorLockMode.None : CursorLockMode.Locked;
    }

    //a camera move system where you can move laterally and zoom in/out
    public void MoveCamera()
    {
        Vector2 moveLaterally = InputsController.GetInputValue<Vector2>("MoveDir");
        float zoomDir = InputsController.GetInputValue<float>("ZoomDir");
        Vector3 moveDir = new Vector3();

        moveDir.x = moveLaterally.x;
        moveDir.z = moveLaterally.y;
        
        moveDir.y = zoomDir;

        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        float forwardRadAngles = Mathf.Atan2(transform.right.z, transform.right.x);
        forwardRadAngles += Mathf.PI * 0.5f;
        Vector3 ufoForward = new Vector3(Mathf.Cos(forwardRadAngles), 0, Mathf.Sin(forwardRadAngles));

        moveVelocity = transform.right * moveDir.x * moveSpeed;
        moveVelocity.y += moveDir.y * moveSpeed;
        moveVelocity += ufoForward * moveDir.z * moveSpeed;
        moveVelocity *= 0.25f;

        transform.position += moveVelocity * Time.deltaTime;
    }

    //used for debugging
    public void ToggleMouseCursorVisibility()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.F1))
        {
            showMouseCursor = !showMouseCursor;

            Cursor.visible = showMouseCursor;
            Cursor.lockState = showMouseCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    //this function is called in the "TheMazeScene" when BTN_RegenerateMaze button is clicked
    public void RegenerateMaze()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TheMazeScene");
    }

    //this function is called in the "TheMazeScene" when BTN_MainMenu button is clicked
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }



    // Update is called once per frame
    void Update()
    {
        //debug function
        //ToggleMouseCursorVisibility();

        TogglePauseMenuTrigger();
        MoveCamera();
    }
}
