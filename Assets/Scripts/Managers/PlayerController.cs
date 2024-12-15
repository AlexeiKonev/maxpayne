using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

enum PlayerStates { Jump, SlowJump, Stand, Walk }

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    PlayerStates playerStates;

    [Header("Параметры для стрельбы")]
    public Shooting shooting;

    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isAiming = false;
    [SerializeField] private bool handsAreRaised = false;
    [SerializeField] public bool isGrounded = true;
    [SerializeField] private bool isStanding = true;
    [SerializeField] private bool isTryingToStand = false;

    private float currentTimeForRisingHands = 0f;
    private float currentArmsWeight = 0f;
    private float neededTimeForGroundCheck = 0f;

    private Vector3 runDirection;
    private Vector3 crosshairPosition;
    private Vector3 crosshairWorldPositionWithOffsets;
    private Vector3 rightArmAimPosition;
    private Vector3 leftArmAimPosition;

    private Camera mainCamera = null;
    private Animator playerAnimator = null;
    private Transform playerTransform = null;
    private Rigidbody playerRigidbody = null;
    [SerializeField] private bool timeSlowed = false;
    [Header("Параметры для движения")]
    public float moveSpeed = 5f;

    [Header("Параметры для поворота игрока к прицелу")]
    public float rotationStep = 15f;

    [Header("Параметры для поворота головы и рук к прицелу")]
    public float zOffsetHeadArms = 250f;

    [Header("Параметры для поднятия рук при прицеливании")]
    [Range(0, 1)]
    public float armsWeight = 1f;
    public float xOffsetLeftArm = -100f;
    public float xOffsetRightArm = 100f;
    public float timeForRaisingHands = 0.5f;
    public Transform rightWrist = null;
    public Transform leftWrist = null;

    [Header("Параметры для прыжка")]
    public float jumpForce = 100f;
    public GameObject mainPlayerCollider = null;
    public GameObject jumpingPlayerCollider = null;
    public LayerMask checkingLayers;
    public float checkDistance = 1f;
    public float delayBeforeCheck = 1f;

    [Header("Параметры для замедления времени при прыжке")]
    [Range(0, 1)]
    public float slowedTime;
    public PostProcessVolume mainPPVolume = null;
    public PostProcessProfile mainPPProfile = null;
    public PostProcessProfile slowtimePPProfile = null;



    /// <summary>
    /// Инициализация параметров.
    /// Блокируем курсор.
    /// </summary>
    private void Start()
    {
        // Блокируем курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Инициализация нужных переменных
        mainCamera = Camera.main;
        playerAnimator = GetComponent<Animator>();
        playerTransform = transform;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Обработка ввода данных.
    /// </summary>
    ///
    
    private void Update()
    {
        runDirection.x = Input.GetAxisRaw("Horizontal");
        runDirection.z = Input.GetAxisRaw("Vertical");
        runDirection = runDirection.normalized;

        isRunning = runDirection.magnitude == 0 ? false : true;

        // Координаты прицела + смещения из инспектора
        crosshairPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        crosshairWorldPositionWithOffsets = mainCamera.ScreenToWorldPoint(new Vector3(crosshairPosition.x,
                                                                                      crosshairPosition.y,
                                                                                      zOffsetHeadArms));

        leftArmAimPosition = mainCamera.ScreenToWorldPoint(new Vector3(crosshairPosition.x + xOffsetLeftArm,
                                                                       crosshairPosition.y, zOffsetHeadArms));

        rightArmAimPosition = mainCamera.ScreenToWorldPoint(new Vector3(crosshairPosition.x + xOffsetRightArm,
                                                                        crosshairPosition.y, zOffsetHeadArms));

        // Прицеливание
        if (Input.GetMouseButtonDown(1))
        {



            if (!isAiming)
            {
                CursorManager.Instance.ZoomIn();
            }
            isAiming = true;
        }

        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
        {


            if (isAiming)
            {
                CursorManager.Instance.ZoomOut();
            }
            isAiming = false;
        }

        // Стрельба
        if (Input.GetMouseButton(0))
        {
            isAiming = true;
            shooting.Shoot(/*crosshairWorldPositionWithOffsets*/);
        }

        if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {
            isAiming = false;
        }


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (!timeSlowed)
            {
                timeSlowed = true;
                SlowDownTime();
            }
            else
            {
                timeSlowed = false;
                NormalizeTime();
            }


        }


        // Прыжок или поднятие
        if (Input.GetKeyDown( KeyCode.LeftShift))
        {
            if (isGrounded && isStanding)
            {
                Jump();
            }
            //else if (isGrounded && !isStanding && !isTryingToStand)
            //{
            //    isTryingToStand = true;
            //    GetUp();
            //}
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && isStanding)
            {
                JumpSimple();

            }
            else if (isGrounded && !isStanding && !isTryingToStand)
            {
                isTryingToStand = true;
                GetUp();
            }


            if (!isGrounded  )
            {
                isGrounded = true;
                timeSlowed = false;
                NormalizeTime();

            }
        }

    }

    private void FixedUpdate()
    {
        if (isGrounded && !isStanding && !isTryingToStand)
        {
            isTryingToStand = true;
            playerRigidbody.isKinematic = true;
            GetUp();
            playerRigidbody.isKinematic = false;
        }
        CorectAnimation();
    }
    /// <summary>
    /// Замена анимации. Обязательно происходит в LateUpdate.
    /// </summary>

    private void CorectAnimation()
    {

        // Если игрок в полете - передает в его Animator параметры для
        // корректного отоброжения положения тела в полете - 
        // анимация разворачивается в ту же сторону, в которую игрок целится.
        // --
        // Если игрок на земле - разворачиваем его в сторону прицела и
        // передаем параметры для корректного отображения анимации бега.
        if (!isStanding && !isTryingToStand)
        {
            // Получаем направление взгляда камеры.
            Vector3 aimDirection = mainCamera.transform.forward;
            aimDirection.y = 0f;
            aimDirection = aimDirection.normalized;
            aimDirection = playerTransform.InverseTransformDirection(aimDirection);

            playerAnimator.SetFloat("aimHorizontal", aimDirection.x);
            playerAnimator.SetFloat("aimVertical", aimDirection.z);

            CheckForGrounded();
        }
        else if (isGrounded && isStanding)
        {
            RotateToCrosshair();

            playerAnimator.SetFloat("moveHorizontal", runDirection.x);
            playerAnimator.SetFloat("moveVertical", runDirection.z);

            Vector3 runVector = playerTransform.TransformDirection(runDirection) * moveSpeed;
            runVector.y = playerRigidbody.velocity.y;

            playerRigidbody.velocity = runVector;
        }
    }

    private void LateUpdate()
    {
        // Если игрок целится - ориентируем объекты запястьев по направлению к прицелу.
        // Вместе с запястьями будут вращаться и оружия.
        if ((isAiming || !isStanding) && !isTryingToStand)
        {
            Vector3 rightDirection = crosshairWorldPositionWithOffsets - rightWrist.position;
            Vector3 leftDirection = crosshairWorldPositionWithOffsets - leftWrist.position;

            leftWrist.LookAt(leftDirection);
            rightWrist.LookAt(rightDirection);
        }
    }

    /// <summary>
    /// Inverse Kinematic.
    /// </summary>
    private void OnAnimatorIK()
    {
        playerAnimator.SetLookAtPosition(crosshairWorldPositionWithOffsets);
        playerAnimator.SetLookAtWeight(1.0f, 0.5f, 1.0f, 1.0f, 0.7f);

        // Вытягивание рук при прицеливании
        if ((isAiming || !isStanding) && !isTryingToStand)
        {
            // Плавное поднятие рук
            handsAreRaised = true;

            if (currentTimeForRisingHands < timeForRaisingHands)
            {
                currentTimeForRisingHands += Time.deltaTime;
                currentArmsWeight = Mathf.Lerp(0, armsWeight, currentTimeForRisingHands / timeForRaisingHands);
            }
            else
            {
                currentTimeForRisingHands = timeForRaisingHands;
                currentArmsWeight = armsWeight;
            }
        }
        else
        {
            // Плавное опускание рук
            if (!handsAreRaised)
            {
                return;
            }

            if (currentTimeForRisingHands > 0)
            {
                currentTimeForRisingHands -= Time.deltaTime;
                currentArmsWeight = Mathf.Lerp(0, armsWeight, currentTimeForRisingHands / timeForRaisingHands);
            }
            else
            {
                currentTimeForRisingHands = 0;
                currentArmsWeight = 0;
                handsAreRaised = false;
            }
        }

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, currentArmsWeight);
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, currentArmsWeight);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightArmAimPosition);
        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftArmAimPosition);
    }

    /// <summary>
    /// Функция, вращения игрока к прицелу.
    /// </summary>
    private void RotateToCrosshair()
    {
        // Получаем текущий угол вращения камеры.
        float cameraYRotation = mainCamera.transform.rotation.eulerAngles.y;

        // Меняем угол поворота только по оси У.
        playerTransform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.Euler(0f, cameraYRotation, 0f),
                                                    rotationStep * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Функций, отвечающая за прыжок игрока.
    /// </summary>
    private void Jump()
    {
        // Обнуляем скорость игрока, чтобы он не пролетал больше нужного
        playerRigidbody.velocity = Vector3.zero;

        // Изменяем статус игрока
        isGrounded = false;
        isStanding = false;

        // Активируем коллайдер игрока в полете и деактивируем коллайдер для перемещения
        mainPlayerCollider.SetActive(false);
        jumpingPlayerCollider.SetActive(true);

        // Запуска анимации
        playerAnimator.SetTrigger("Jump");

        // Физика прыжка
        Vector3 jumpDirection;
        if (runDirection.magnitude == 0)
        {
            jumpDirection = playerTransform.forward;
        }
        else
        {
            jumpDirection = playerTransform.TransformDirection(runDirection);
        }
        jumpDirection = jumpDirection.normalized;

        playerRigidbody.AddForce((jumpDirection + Vector3.up) * playerRigidbody.mass * jumpForce);

        // Разворот игрока по направлению прыжка
        playerTransform.rotation = Quaternion.LookRotation(jumpDirection);

        // Замедляем время.
        SlowDownTime();

        // Время, после которого будет осуществляться проверка на приземление
        neededTimeForGroundCheck = Time.time + delayBeforeCheck;
    }
    private void JumpSimple()
    {
        // Обнуляем скорость игрока, чтобы он не пролетал больше нужного
        playerRigidbody.velocity = Vector3.zero;

        // Изменяем статус игрока
        isGrounded = false;
        isStanding = false;

        // Активируем коллайдер игрока в полете и деактивируем коллайдер для перемещения
        mainPlayerCollider.SetActive(false);
        jumpingPlayerCollider.SetActive(true);

        // Запуска анимации
        //playerAnimator.SetTrigger("Jump");

        // Физика прыжка
        Vector3 jumpDirection;
        if (runDirection.magnitude == 0)
        {
            jumpDirection = playerTransform.forward;
        }
        else
        {
            jumpDirection = playerTransform.TransformDirection(runDirection);
        }
        jumpDirection = jumpDirection.normalized;

        playerRigidbody.AddForce((jumpDirection + Vector3.up) * playerRigidbody.mass * jumpForce);

        // Разворот игрока по направлению прыжка
        playerTransform.rotation = Quaternion.LookRotation(jumpDirection);

        // Замедляем время.
        //SlowDownTime();

        // Время, после которого будет осуществляться проверка на приземление
        //neededTimeForGroundCheck = Time.time + delayBeforeCheck;
        CheckForGroundedSimple();
    }

    /// <summary>
    /// Находится ли игрок на земле.
    /// </summary>
    private void CheckForGrounded()
    {
        if (Time.time < neededTimeForGroundCheck) return;

        CapsuleCollider playerCapsCollider = jumpingPlayerCollider.GetComponent<CapsuleCollider>();
        Bounds playerCapsColliderBounds = playerCapsCollider.bounds;
        Vector3 offsetVector = new Vector3(0f, 0f, playerCapsCollider.height / 2);

        Ray[] checkingRays = {
                                new Ray(playerCapsColliderBounds.center, Vector3.down),
                                new Ray(playerCapsColliderBounds.center + offsetVector, Vector3.down),
                                new Ray(playerCapsColliderBounds.center - offsetVector, Vector3.down),
                             };

        foreach (Ray currentRay in checkingRays)
        {
            isGrounded |= Physics.Raycast(currentRay, checkDistance, checkingLayers);
        }

        if (isGrounded)
        {
            NormalizeTime();

        }
    }
    private void CheckForGroundedSimple()
    {
        if (Time.time < neededTimeForGroundCheck) return;

        CapsuleCollider playerCapsCollider = jumpingPlayerCollider.GetComponent<CapsuleCollider>();
        Bounds playerCapsColliderBounds = playerCapsCollider.bounds;
        Vector3 offsetVector = new Vector3(0f, 0f, playerCapsCollider.height / 2);

        Ray[] checkingRays = {
                                new Ray(playerCapsColliderBounds.center, Vector3.down),
                                new Ray(playerCapsColliderBounds.center + offsetVector, Vector3.down),
                                new Ray(playerCapsColliderBounds.center - offsetVector, Vector3.down),
                             };

        foreach (Ray currentRay in checkingRays)
        {
            isGrounded |= Physics.Raycast(currentRay, checkDistance, checkingLayers);
        }

        if (isGrounded)
        {
            isStanding = true;
            NormalizeTime();

        }
    }

    /// <summary>
    /// Включает замедление времени.
    /// </summary>
    private void SlowDownTime()
    {
        mainPPVolume.profile = slowtimePPProfile;
        Time.timeScale = slowedTime;
    }

    /// <summary>
    /// Нормализирует время.
    /// </summary>
    private void NormalizeTime()
    {
        mainPPVolume.profile = mainPPProfile;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Функция, вызывающияся при вставании игрока.
    /// </summary>
    private void GetUp()
    {
        shooting.canShoot = false;
        isAiming = false;

        // Запуска анимации
        playerAnimator.SetTrigger("GetUp");

        // Активируем коллайдер игрока в полете и деактивируем коллайдер для перемещения
        mainPlayerCollider.SetActive(true);
        jumpingPlayerCollider.SetActive(false);
    }
    private void GetUpSimple()
    {
        shooting.canShoot = false;
        isAiming = false;

        // Запуска анимации
        //playerAnimator.SetTrigger("GetUp");

        // Активируем коллайдер игрока в полете и деактивируем коллайдер для перемещения
        mainPlayerCollider.SetActive(true);
        jumpingPlayerCollider.SetActive(false);
    }

    /// <summary>
    /// Ивент для анимаций вставания.
    /// </summary>
    public void ChangeIsStandingFromAnimator()
    {
        isTryingToStand = false;
        isStanding = true;
        shooting.canShoot = true;
    }
}
