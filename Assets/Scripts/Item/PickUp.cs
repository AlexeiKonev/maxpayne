using UnityEngine;
using UnityEngine.ProBuilder;

public class PickUp : MonoBehaviour
{
    Shooting shooting;
    private void Start()
    {
        shooting = GetComponent<Shooting>();
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Pistol")
        {
            shooting.ammo += 3f;

            GameObject.Destroy(collision.gameObject);
            Debug.Log("PicUp Pistol ammo");

        }
        if (collision.gameObject.tag == "Uzi")
        {
            shooting.ammoUziAll += 8f;

            GameObject.Destroy(collision.gameObject);
            Debug.Log("PicUp Uzi ammo");

        }
    }
}
