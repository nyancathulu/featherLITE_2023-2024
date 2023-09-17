using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Red_Shooting : MonoBehaviour
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
        GameObject bullet = Red_Bullet_Pooling.Red_instance.GetRedBullet();

        if (bullet != null)
        {
            bullet.transform.position = gameObject.transform.position;
            bullet.transform.rotation = gameObject.transform.rotation;
            bullet.SetActive(true);
        }

    }
}
