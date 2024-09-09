using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
public class Blue_Shooting : MonoBehaviour
{
    public InputActionReference firingInput;
    public AudioSource ShootSound;
    public PlayerMovement player;

    [Range(-1, 0)]
    public float vert_CrouchShootOffset;
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
            if (player.isCrouching) bullet.transform.position = gameObject.transform.position + new Vector3(0, vert_CrouchShootOffset, 0);
            else bullet.transform.position = gameObject.transform.position + new Vector3(0, 0, 0);
            bullet.transform.rotation = gameObject.transform.rotation;
            bullet.SetActive(true);
            ShootSound.Play();
        }

    }
}
