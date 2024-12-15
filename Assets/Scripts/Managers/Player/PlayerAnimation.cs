using UnityEngine;

//Okay, hereТs the code for PlayerAnimation.cs:

using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private float armsWeight = 1f;
    [SerializeField] private float timeForRaisingHands = 0.5f;

    private float currentTimeForRisingHands = 0f;
    private float currentArmsWeight = 0f;
    private bool handsAreRaised = false;


    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void Move(Vector2 input, bool isRunning)
    {
        playerAnimator.SetFloat("moveHorizontal", input.x);
        playerAnimator.SetFloat("moveVertical", input.y);
    }

    public void Jump()
    {
        playerAnimator.SetTrigger("Jump");
    }

    public void GetUp()
    {
        playerAnimator.SetTrigger("GetUp");
    }

    public void StartAiming()
    {
        playerAnimator.SetBool("isAiming", true);
    }

    public void StopAiming()
    {
        playerAnimator.SetBool("isAiming", false);

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (playerAnimator.GetBool("isAiming"))
        {
            // ѕлавное подн€тие рук
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
            // ѕлавное опускание рук
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
        // «десь должна быть логика установки позиции рук, если необходимо.


    }
}
//Ётот класс теперь содержит только анимационную логику, использу€ Animator дл€ управлени€ анимаци€ми. —ледующий будет PlayerPhysics.cs.