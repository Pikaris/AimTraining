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

    float limitMinX = -60.0f;
    float limitMaxX = 60.0f;

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
        Transform child = transform.GetChild(2);
        vcam = child.GetComponent<CinemachineVirtualCamera>();
        //movePosition = GetComponent<Vector3>();

        //direction = Vector3.zero;
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
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
        
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        mousePosition.x += Mouse.current.position.x.ReadValue() * mouseSensitivityX;
        mousePosition.y -= Mouse.current.position.y.ReadValue() * mouseSensitivityY;
        mousePosition.x = Mathf.Clamp(mousePosition.x, limitMinX, limitMaxX);
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
    }

    private void DoShake()
    {

    }
}
