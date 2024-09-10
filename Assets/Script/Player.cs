using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 3.0f;
    [SerializeField] float mouseSensitivityY = 2.0f;
    [SerializeField] float limitMinY = -30.0f;
    [SerializeField] float limitMaxY = 40.0f;

    Vector3 screenCenter;

    float rotationX = 0;
    float rotationY = 0;

    public float moveSpeed = 5.0f;

    PlayerInputAction inputAction;
    Rigidbody rigid;

    CinemachineVirtualCamera headVcam;

    Vector2 mousePos;

    Vector2 mouseWorldPos;
    Vector3 movePosition;
    float mouseX;
    float mouseY;
    float mousePosX;
    float mousePosY;



    private void Awake()
    {
        inputAction = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        Transform child = transform.GetChild(0);
        headVcam = child.GetComponent<CinemachineVirtualCamera>();
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
        // rigid.Move(rigid.position + Time.fixedDeltaTime * moveSpeed * movePosition, vcam.transform.rotation);
        rigid.Move(rigid.position + Time.fixedDeltaTime * moveSpeed * movePosition, Quaternion.Euler(0, rotationY, 0));
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        headVcam.transform.rotation *= Quaternion.Euler(-rotationX, rotationY, 0);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        //mousePos.z = movePosition.z + multiplyMouseSens;
        //mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Debug.Log(mousePos);

        screenCenter = new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f);

        Debug.Log(screenCenter);

        ////mouseX = Mouse.current.position.x.ReadValue();
        ////mouseY = Mouse.current.position.y.ReadValue();

        //mousePosX = mouseX * mouseSensitivityX * Time.fixedDeltaTime;// + (monitor_Width * 0.5f);
        //mousePosY = mouseY * mouseSensitivityY * Time.fixedDeltaTime;// + (monitor_Height * 0.5f);

        mousePosX = (mousePos.x - screenCenter.x) * mouseSensitivityX;// * Time.deltaTime;
        mousePosY = (mousePos.y - screenCenter.y) * mouseSensitivityY;// * Time.deltaTime;

        //Debug.Log($"mouseCenterX : {mousePos.x - screenCenter.x}");
        //Debug.Log($"mouseCenterY : {mousePos.y - screenCenter.y}");
        //Debug.Log($"mouseX : {mousePos.x}");
        //Debug.Log($"mouseY : {mousePos.y}");

        rotationY = mousePosX;
        rotationX = mousePosY;
        //rotationX = Mathf.Clamp(rotationX, limitMinY, limitMaxY);
        //Debug.Log($"X :{mousePosX}");
        //Debug.Log($"Y :{mousePosY}");
        //Debug.Log($"transX :{transform.rotation.x}");
        //Debug.Log($"transY :{transform.rotation.y}");
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
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
}
