using UnityEngine;

//Хорошо, вот код для PlayerMovement.cs:

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerPhysics playerPhysics;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationStep = 15f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform rightWrist;
    [SerializeField] private Transform leftWrist;
    [SerializeField] private Vector3 crosshairWorldPositionWithOffsets;
    [SerializeField] private float zOffsetHeadArms = 250f;
    [SerializeField] private float xOffsetLeftArm = -100f;
    [SerializeField] private float xOffsetRightArm = 100f;


    private bool isAiming = false;

    public void Move(Vector2 input, bool isRunning)
    {
        if (playerPhysics.IsGrounded && playerPhysics.IsStanding)
        {
            playerAnimation.Move(input, isRunning);
            Vector3 runVector = playerTransform.TransformDirection(input) * moveSpeed;
            runVector.y = playerRigidbody.velocity.y;
            playerRigidbody.velocity = runVector;
            RotateToCrosshair();
        }
    }

    public void Jump()
    {
        if (playerPhysics.IsGrounded && playerPhysics.IsStanding)
        {
            playerPhysics.Jump(jumpForce);
            playerAnimation.Jump();
        }
    }

    public void GetUp()
    {
        if (playerPhysics.IsGrounded && !playerPhysics.IsStanding)
        {
            playerPhysics.GetUp();
            playerAnimation.GetUp();
        }
    }

    public void StartAiming()
    {
        isAiming = true;
        playerAnimation.StartAiming();
    }

    public void StopAiming()
    {
        isAiming = false;
        playerAnimation.StopAiming();

    }


    private void RotateToCrosshair()
    {
        float cameraYRotation = mainCamera.transform.rotation.eulerAngles.y;
        playerTransform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.Euler(0f, cameraYRotation, 0f),
                                                    rotationStep * Time.fixedDeltaTime);
    }


    private void UpdateArmPositions()
    {
        if (isAiming)
        {
            crosshairWorldPositionWithOffsets = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                                                            Input.mousePosition.y,
                                                                                            zOffsetHeadArms));

            Vector3 rightDirection = crosshairWorldPositionWithOffsets - rightWrist.position;
            Vector3 leftDirection = crosshairWorldPositionWithOffsets - leftWrist.position;

            rightWrist.LookAt(crosshairWorldPositionWithOffsets);
            leftWrist.LookAt(crosshairWorldPositionWithOffsets);
        }
    }
    private void LateUpdate()
    {
        UpdateArmPositions();
    }
}
//Этот класс теперь отвечает за движение и логику прыжка, оставляя анимацию и физику отдельным классам. Обратите внимание, как он вызывает методы из PlayerAnimation и PlayerPhysics. Теперь перейдём к PlayerAnimation.cs.