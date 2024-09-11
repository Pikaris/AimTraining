using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 3.0f;
    [SerializeField] float mouseSensitivityY = 2.0f;
    [SerializeField] float limitMinY = -30.0f;
    [SerializeField] float limitMaxY = 40.0f;


    float rotationX = 0;
    float rotationY = 0;

    public float moveSpeed = 5.0f;

    PlayerInputAction inputAction;
    Rigidbody rigid;

    CinemachineVirtualCamera headVcam;

    Vector2 mousePos;
    Vector3 movePosition;
    Vector3 screenCenter;

    float mousePosX;
    float mousePosY;



    private void Awake()
    {
        inputAction = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        Transform child = transform.GetChild(0);
        headVcam = child.GetComponent<CinemachineVirtualCamera>();

        Cursor.lockState = CursorLockMode.Locked;
        rigid.freezeRotation = true;

        screenCenter = new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
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
        rigid.Move(rigid.position + Time.fixedDeltaTime * moveSpeed * movePosition, Quaternion.Euler(0, rotationY, 0));
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        rotationX = 0;
        rotationY = 0;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        LookAim();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 200.0f))
        {
            if(hit.collider.CompareTag("Target"))
            {
                Debug.Log("Hit");
            }
        }
        //ray.GetPoint
    }

    private void DoShake()
    {

    }

    void LookAim()
    {
        mousePosX = mousePos.x * mouseSensitivityX * Time.deltaTime;
        mousePosY = mousePos.y * mouseSensitivityY * Time.deltaTime;

        rotationY = mousePosX;
        rotationX = mousePosY;

        rotationY = Mathf.Clamp(rotationY, limitMinY, limitMaxY);

        headVcam.transform.rotation *= Quaternion.Euler(-rotationX, rotationY, 0);
    }
}
