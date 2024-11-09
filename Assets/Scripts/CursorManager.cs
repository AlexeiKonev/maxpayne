using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Image cursor;


    void Start()
    {


        cursor = GameObject.Find("Crosshair Image").GetComponent<Image>();
        cursor.color = Color.white;
    }

    private void FixedUpdate()
    {


        // Calculate direction using Raycast
        RaycastHit hit;
        Vector3 direction = Shooting.Instance.mainCameraTransform.forward; // Направление взгляда камеры

        // Check if we hit something within range
        if (Physics.Raycast(Shooting.Instance.mainCameraTransform.position, direction, out hit, Shooting.Instance.range, Shooting.Instance.layerMask))
        {
            // If we hit something, set the direction to that point
            direction = (hit.point - Shooting.Instance.gunTransform.position).normalized; // Направление к точке попадания



            if (hit.transform.gameObject.tag == "Enemy")
            {
                cursor.color = Color.red;
            }
            if (hit.transform.gameObject.tag == "Untagged")
            {
                cursor.color = Color.white;
            }






        }



    }

}
