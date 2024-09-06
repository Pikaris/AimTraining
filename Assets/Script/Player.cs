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
    [SerializeField] float limitMinX = -60.0f;
    [SerializeField] float limitMaxX = 60.0f;

    int monitor_Width;
    int monitor_Height;

    float rotationX;
    float rotationY;

    public float moveSpeed = 5.0f;

    PlayerInputAction inputAction;
    Rigidbody rigid;

    CinemachineVirtualCamera vcam;

    Vector2 mousePosition;

    Vector3 worldPosition;
    Vector3 movePosition;

    
    

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        Transform child = transform.GetChild(0);
        vcam = child.GetComponent<CinemachineVirtualCamera>();

        monitor_Width = UnityEngine.Screen.width;
        monitor_Height = UnityEngine.Screen.height;
        rigid.freezeRotation = true;
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
    }

    private void Update()
    {
        //worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //float mousePosX = Mouse.current.position.x.ReadValue() * mouseSensitivityX - monitor_Width;
        //float mousePosY = Mouse.current.position.y.ReadValue() * mouseSensitivityY - (monitor_Height * 0.5f);
        rotationY = worldPosition.x;
        rotationX = worldPosition.y;
        //rotationX = Mathf.Clamp(rotationX, limitMinX, limitMaxX);
        vcam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Debug.Log($"X :{worldPosition.x}");
        Debug.Log($"Y :{worldPosition.y}");
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
