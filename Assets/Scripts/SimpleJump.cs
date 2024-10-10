using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleJump : MonoBehaviour
{
  public  PlayerController playerController;
    void Start()
    {
        playerController =GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerController != null)
        {
            if(Input.GetKeyUp(KeyCode.LeftAlt)) 
            {
                Debug.Log("simple jump");
            }
        }

    }
}
