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

    private const float threshHold = 0.05f;

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

        screenCenter = new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);
    }

    private void Start()
    {
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
        inputAction.Player.Aim.performed += OnAim;
        inputAction.Player.Aim.canceled += OnAim;
    }

    private void OnDisable()
    {
        inputAction.Player.Aim.canceled -= OnAim;
        inputAction.Player.Aim.performed -= OnAim;
        inputAction.Player.Click.performed -= OnFire;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
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
        if(Physics.Raycast(ray, out hit, 300.0f))
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

    /// <summary>
    /// 마우스 에임 함수
    /// </summary>
    void LookAim()
    {
        if (mousePos.sqrMagnitude >= threshHold)
        {
            CalculateAim();
        }
        else
        {
            mousePos = new Vector2(0, 0);
            CalculateAim();
        }
    }

    /// <summary>
    /// 마우스 에임에 필요한 계산을 하는 함수
    /// </summary>
    private void CalculateAim()
    {
        float fixedDeltaTimeMultiplier = IsInputDevice ? 1.0f : Time.fixedDeltaTime;

        rotationX += mousePos.y * mouseSensitivityY * fixedDeltaTimeMultiplier;
        rotationY = mousePos.x * mouseSensitivityX * fixedDeltaTimeMultiplier;

        rotationX = ClampAngle(rotationX, -90.0f, 90.0f);

        headVcam.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * rotationY);
    }

    private void Move()
    {
        Vector3 direction = new Vector3(rigid.position.x, 0.0f, rigid.position.y);

        direction = rigid.transform.right * movePosition.x + rigid.transform.forward * movePosition.z;

        rigid.Move(rigid.position + Time.fixedDeltaTime * moveSpeed * direction, rigid.rotation * Quaternion.Euler(0, rotationY, 0));
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
