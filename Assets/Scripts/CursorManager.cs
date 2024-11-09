using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    public Image cursor;

    public Transform zoomScope;
    public Transform defaultScope;

    public GameObject cursorDead;

    public static CursorManager Instance;

    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        zoomScope = GameObject.Find("Zoom").GetComponent<Transform>();  
        defaultScope = GameObject.Find("Player_1").GetComponent<Transform>();  

        cinemachineFreeLook = GameObject.Find("CM FreeLook").GetComponent<CinemachineFreeLook>();
        cursor = GameObject.Find("Crosshair Image").GetComponent<Image>();
        cursor.color = Color.white;
    }
    public void ShowDeadCursor(float delayseconds)
    {

        StartCoroutine(ShowDeadDelay(delayseconds));
    }
    public IEnumerator ShowDeadDelay(float delay)
    {
        cursorDead.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        cursorDead.gameObject.SetActive(false);
    }
    public void ZoomIn()
    {
        cinemachineFreeLook.Follow = zoomScope;
    }
    public void ZoomOut()
    {
        cinemachineFreeLook.Follow = defaultScope;
    }
    private void Update()
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
