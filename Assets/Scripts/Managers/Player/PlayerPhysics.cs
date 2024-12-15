using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private GameObject mainPlayerCollider;
    [SerializeField] private GameObject jumpingPlayerCollider;
    [SerializeField] private LayerMask checkingLayers;
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private float delayBeforeCheck = 1f;

    private bool isGrounded = true;
    private bool isStanding = true;
    private bool isTryingToStand = false;
    private float neededTimeForGroundCheck = 0f;


    public bool IsGrounded => isGrounded;
    public bool IsStanding => isStanding;

    public void Jump(float jumpForce)
    {
        playerRigidbody.velocity = Vector3.zero;
        isGrounded = false;
        isStanding = false;
        mainPlayerCollider.SetActive(false);
        jumpingPlayerCollider.SetActive(true);
        Vector3 jumpDirection = transform.forward; //  Simplified jump direction
        playerRigidbody.AddForce((jumpDirection + Vector3.up) * jumpForce);
        neededTimeForGroundCheck = Time.time + delayBeforeCheck;
    }

    public void GetUp()
    {
        isTryingToStand = true;
        playerRigidbody.isKinematic = true;
        isStanding = true;
        mainPlayerCollider.SetActive(true);
        jumpingPlayerCollider.SetActive(false);
        isTryingToStand = false;


    }


    private void FixedUpdate()
    {
        if (isTryingToStand)
        {
            GetUp();
            playerRigidbody.isKinematic = false;
        }
        CheckForGrounded();
    }

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
            isStanding = true;
        }
    }
}
//Этот класс отвечает за физику игрока, включая прыжок, проверку приземления, и управление коллайдерами. Обратите внимание, что метод GetUp теперь проще, так как анимация вызывается из другого класса. Остался только TimeManager.cs.