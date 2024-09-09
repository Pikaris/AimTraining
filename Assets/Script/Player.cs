using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 3.0f;
    [SerializeField] float mouseSensitivityY = 2.0f;
    [SerializeField] float limitMinX = 300.0f;
    [SerializeField] float limitMaxX = 1000.0f;

    int monitor_Width;
    int monitor_Height;

    float rotationX = 0;
    float rotationY = 0;

    public float moveSpeed = 5.0f;

    PlayerInputAction inputAction;
    Rigidbody rigid;

    CinemachineVirtualCamera vcam;

    Vector3 mousePosition;

    Vector3 worldPosition;
    Vector3 movePosition;
    float mouseX;
    float mouseY;
    float mousePosX;
    float mousePosY;



    private void Awake()
    {
        inputAction = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        Transform child = transform.GetChild(2);
        vcam = child.GetComponent<CinemachineVirtualCamera>();

        monitor_Width = UnityEngine.Screen.width;
        monitor_Height = UnityEngine.Screen.height;
        //rigid.freezeRotation = true;
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.Click.performed += OnFire;
        inputAction.Player.Click.canceled += OnFire;
        inputAction.Player.Aim.performed += OnAim;
    }

    private void OnDisable()
    {
        inputAction.Player.Aim.performed -= OnAim;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Click.performed -= OnFire;
        inputAction.Player.Click.canceled -= OnFire;
        inputAction.Player.Disable();
    }

    private void FixedUpdate()
    {
        rigid.Move(rigid.position + Time.fixedDeltaTime * moveSpeed * movePosition, vcam.transform.rotation);

        mousePosX = mouseX * mouseSensitivityX * Time.fixedDeltaTime;// + (monitor_Width * 0.5f);
        mousePosY = mouseY * mouseSensitivityY * Time.fixedDeltaTime;// + (monitor_Height * 0.5f);

        rotationY = mousePosX;
        rotationX = mousePosY;
        vcam.transform.rotation = Quaternion.Euler(-rotationX, rotationY, 0);
    }

    private void Update()
    {
        //worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void LateUpdate()
    {
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        //mousePosition = Mouse.current.position.ReadValue();
        //worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseX = Mouse.current.position.x.ReadValue();
        mouseY = Mouse.current.position.y.ReadValue();
        //rotationX = Mathf.Clamp(rotationX, limitMinX, limitMaxX);
        Debug.Log($"X :{mousePosX}");
        Debug.Log($"Y :{mousePosY}");
        Debug.Log($"transX :{transform.rotation.x}");
        Debug.Log($"transY :{transform.rotation.y}");
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    }

    private void DoShake()
    {

    }
}
