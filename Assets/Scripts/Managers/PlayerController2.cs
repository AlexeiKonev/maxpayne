using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerPhysics playerPhysics;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private Shooting shooting;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool isRunning = input.magnitude > 0;
        playerMovement.Move(input, isRunning);

        // Прицеливание
        if (Input.GetMouseButtonDown(1))
        {
            playerMovement.StartAiming();
        }
        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
        {
            playerMovement.StopAiming();
        }

        // Стрельба
        if (Input.GetMouseButton(0))
        {
            shooting.Shoot();
        }


        // Замедление времени
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            timeManager.ToggleSlowMotion();
        }

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
        }

        // Встать
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerMovement.GetUp();
        }


    }

    // Вспомогательные функции (если необходимы)
    // ...
}