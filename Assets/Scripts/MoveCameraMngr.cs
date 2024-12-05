using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMngr : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 moveVelocity;
    private bool showMouseCursor;

    // Start is called before the first frame update
    void Start()
    {
        showMouseCursor = true;
    }

    public void MoveCamera()
    {
        UFO_Movement();

        transform.position += moveVelocity * Time.deltaTime;
    }

    private void UFO_Movement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("HorizontalX"), Input.GetAxis("Vertical"), Input.GetAxis("HorizontalZ"));

        moveDir.x = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) ? 0 : moveDir.x;
        moveDir.y = Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E) ? 0 : moveDir.y;
        moveDir.z = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? 0 : moveDir.z;

        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        float ufoForwardRadAngles = Mathf.Atan2(transform.right.z, transform.right.x);
        ufoForwardRadAngles += Mathf.PI * 0.5f;
        Vector3 ufoForward = new Vector3(Mathf.Cos(ufoForwardRadAngles), 0, Mathf.Sin(ufoForwardRadAngles));

        moveVelocity = transform.right * moveDir.x * moveSpeed;
        moveVelocity.y += moveDir.y * moveSpeed;
        moveVelocity += ufoForward * moveDir.z * moveSpeed;
        moveVelocity *= 0.25f;

        //UI_Mngr.Instance.TextSprites["TextInfo"].text = ufoForward.ToString();
    }

    public void ToggleMouseCursorVisibility()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.F1))
        {
            showMouseCursor = !showMouseCursor;

            Cursor.visible = showMouseCursor;
            Cursor.lockState = showMouseCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

        ToggleMouseCursorVisibility();
        MoveCamera();

        float fps = 1 / Time.deltaTime;

        //UI_Mngr.Instance.TextSprites["TextInfo"].text = fps.ToString();
        //UI_Mngr.Instance.TextSprites["TextInfo"].text = transform.right.ToString();
    }
}
