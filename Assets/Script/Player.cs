using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class Player : MonoBehaviour
{
    [SerializeField] float mouseSensitivityX = 3.0f;
    [SerializeField] float mouseSensitivityY = 2.0f;
    [SerializeField] float limitMinY = -80.0f;
    [SerializeField] float limitMaxY = 80.0f;

    public float moveSpeed = 5.0f;

    protected Vector3 screenCenter;

    float rotationX = 0;
    float rotationY = 0;

    float maxSensX = 100.0f;
    float maxSensY = 100.0f;

    float mousePosX;
    float mousePosY;

    int hitCount;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput playerInput;
#endif

    PlayerInputAction inputAction;
    Rigidbody rigid;

    CinemachineVirtualCamera headVcam;

    Image hitMarker;

    ParticleSystem muzzleFlash;

    TargetManager target;

    Transform armTrans;

    Coroutine coroutineFire;
    Coroutine coroutineHit;

    Vector2 mousePos;
    Vector3 movePosition;


    Coroutine AutoFire;

    private const float threshHold = 0.005f;

    bool isAutoFire = true;
    bool isCanFire = true;
    bool isFire = false;


    public Action onHitChange;

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

    public float SensitivityX
    {
        get => mouseSensitivityX;
        set
        {
            mouseSensitivityX = value;
        }
    }

    public float SensitivityY
    { 
        get => mouseSensitivityY;
        set
        {
            mouseSensitivityY = value;
        }
    }

    public float MaxSensitivityX => maxSensX
;
    public float MaxSensitivityY => maxSensY;

    public int HitCount
    {
        get => hitCount;
        set
        {
            if (hitCount != value)
            {
                hitCount = value;
                onHitChange?.Invoke();
            }
        }
    }


    private void Awake()
    {
        inputAction = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();

        Transform child = transform.GetChild(0).GetChild(0);
        if (child != null)
        {
            headVcam = child.GetComponent<CinemachineVirtualCamera>();
        }

        child = transform.GetChild(1).GetChild(3);
        if (child != null)
        {
            muzzleFlash = child.GetComponent<ParticleSystem>();
        }

        child = transform.GetChild(1);
        if (child != null)
        {
            armTrans = child.GetComponent<Transform>();
        }


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
        GameObject canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        Transform canvasChild = canvas.transform.GetChild(1);
        hitMarker = canvasChild.GetComponent<Image>();
        hitMarker.enabled = false;

        target = FindAnyObjectByType<TargetManager>();

        muzzleFlash.Stop();

        StartCoroutine(GunFire());
        StartCoroutine(HitMarker());
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.Click.performed += OnFire;
        inputAction.Player.Click.canceled += OnFire;
        inputAction.Player.Aim.performed += OnAim;
        inputAction.Player.Aim.canceled += OnAim;
        inputAction.Player.ChangeFireMode.performed += OnChangeFireMode;
    }


    private void OnDisable()
    {
        inputAction.Player.ChangeFireMode.performed -= OnChangeFireMode;
        inputAction.Player.Aim.canceled -= OnAim;
        inputAction.Player.Aim.performed -= OnAim;
        inputAction.Player.Click.canceled -= OnFire;
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
        OnFiring();
    }

    private void LateUpdate()
    {
        LookAim();
    }

    private void AimRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, 300.0f))
        {
            if (rayHit.collider.CompareTag("Target"))
            {
                hitMarker.enabled = true;

                target.SetHittedTarget(rayHit.collider.gameObject);

                HitCount++;

                Debug.Log("Hit");
            }
            else
            {
                hitMarker.enabled = false;
            }
        }
    }

    public void OnOption(bool isDisplayOption)
    {
        if (isDisplayOption)
        {
            Debug.Log("on");
            inputAction.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.Log("off");
            inputAction.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        movePosition = context.ReadValue<Vector3>();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (isAutoFire)
        {
            isFire = inputAction.Player.Click.IsPressed();
        }
        else
        {
            AimRayCast();
            //isFire = false;
            //if (isCanFire)
            //{
            //    isFire = true;
            //    isCanFire = false;
            //}
            //else
            //{
            //    Debug.Log("isFire false");

            //    isFire = false;
            //    if (!inputAction.Player.Click.IsPressed())
            //    {
            //        Debug.Log("CanFire");
            //        isCanFire = true;
            //    }
            //}
        }

        headVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1.5f;
        headVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1.5f;
    }

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

    private void OnFiring()
    {
        if (isFire)
        {
            muzzleFlash.Play();
        }
        else
        {
            muzzleFlash.Stop();
            hitMarker.enabled = false;

            headVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
            headVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.0f;
        }
    }

    private void OnChangeFireMode(InputAction.CallbackContext context)
    {
        isAutoFire ^= true;
        if(isAutoFire)
        {
            Debug.Log("Auto");
            //isAutoFire = false;
        }
        else
        {
            Debug.Log("Single");
            //isAutoFire = true;
        }
    }

    private void CalculateAim()
    {
        float fixedDeltaTimeMultiplier = IsInputDevice ? 1.0f : Time.fixedDeltaTime;

        rotationX += mousePos.y * mouseSensitivityY * fixedDeltaTimeMultiplier;
        rotationY = mousePos.x * mouseSensitivityX * fixedDeltaTimeMultiplier;

        rotationX = ClampAngle(rotationX, limitMinY, limitMaxY);

        headVcam.transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * rotationY);
        armTrans.localRotation = Quaternion.Euler(rotationX * 0.5f, 0.0f, 0.0f);
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

    /// <summary>
    /// ���� ���� �� ����Ǵ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator GunFire()
    {
        while (true)
        {
            if (isFire)
            {
                AimRayCast();

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// ���� ǥ���� �¾��� �� ��Ŀ�� ǥ�� ��Ű�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator HitMarker()
    {
        while (true)
        {
            if (hitMarker.enabled)
            {
                Debug.Log("hitMarkerDisable");
                yield return new WaitForSeconds(0.05f);
                hitMarker.enabled = false;
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator MuzzleFlash()
    {
        while(true)
        {
            //if()
        }
    }
}
