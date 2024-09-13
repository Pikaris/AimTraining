using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class Player : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 3.0f;
    [SerializeField] float mouseSensitivityY = 2.0f;
    [SerializeField] float limitMinY = -30.0f;
    [SerializeField] float limitMaxY = 40.0f;


    float rotationX = 0;
    float rotationY = 0;

    public float moveSpeed = 5.0f;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif

    PlayerInputAction inputAction;
    Rigidbody rigid;

    CinemachineVirtualCamera headVcam;

    Vector2 mousePos;
    Vector3 movePosition;
    Vector3 screenCenter;

    float mousePosX;
    float mousePosY;

    private bool IsInputDevice
    {
        get
        {
            #if ENABLE_INPUT_SYSTEM
            return playerInput.currentControlScheme == "KM";
            #else
            return false;
            #endif
        }
    }
    

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        Transform child = transform.GetChild(0).GetChild(0);
        headVcam = child.GetComponent<CinemachineVirtualCamera>();

        Cursor.lockState = CursorLockMode.Locked;
        rigid.freezeRotation = true;

        screenCenter = new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);

        headVcam.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Start()
    {
        //inputAction = GetComponent<PlayerInputAction>();
#if ENABLE_INPUT_SYSTEM
        playerInput = GetComponent<PlayerInput>();
#else
        Debug.LogError("KeyboardMouse Error");
#endif
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
        //rigid.Move(rigid.position + Time.fixedDeltaTime * moveSpeed * movePosition, Quaternion.Euler(0, rotationY, 0));
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        LookAim();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        Debug.Log(mousePos);
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
        //if (inputAction.Player.Aim.WasPressedThisFrame)
        //float camRotX = headVcam.transform.rotation.x;
        //float camRotY = headVcam.transform.rotation.y;

        float deltaTimeMultiplier = IsInputDevice ? 1.0f : Time.deltaTime;

        rotationY = mousePos.x * mouseSensitivityX * deltaTimeMultiplier;
        rotationX += mousePos.y * mouseSensitivityY * deltaTimeMultiplier;

        //camRotX = Mathf.Clamp(headVcam.transform.rotation.x, limitMinY, limitMaxY);
        //camRotY = Mathf.Clamp(headVcam.transform.rotation.y, limitMinY, limitMaxY);

        //Debug.Log(rotationX);
        //Debug.Log(rotationY);

        headVcam.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * rotationY);
    }
}
