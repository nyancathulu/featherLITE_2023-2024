using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blue_Shooting : MonoBehaviour
{
    public InputActionReference firingInput;

    // Update is called once per frame
    void Update()
    {
        if (firingInput.action.triggered)
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        GameObject bullet = Blue_Bullet_Pooling.Blue_instance.GetBlueBullet();

        if (bullet != null)
        {
            bullet.transform.position = gameObject.transform.position;
            bullet.transform.rotation = gameObject.transform.rotation;
            bullet.SetActive(true);
        }

    }
}
