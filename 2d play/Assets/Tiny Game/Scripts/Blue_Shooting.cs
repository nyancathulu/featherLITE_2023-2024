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
    public float firingPotential;
    public float reloadSpeed;
    public float totalFirePotential;
    [Range(-1, 0)]
    public float vert_CrouchShootOffset;

    private void OnEnable()
    {
        firingPotential = totalFirePotential;
    }
    // Update is called once per frame
    void Update()
    {
        if (firingInput.action.triggered)
        {
            FireBullet();
        }
        firingPotential += Time.deltaTime * reloadSpeed;
        firingPotential = Mathf.Clamp(firingPotential, -20, totalFirePotential);
    }

    void FireBullet()
    {
        GameObject bullet = Blue_Bullet_Pooling.Blue_instance.GetBlueBullet();

        if (bullet != null)
        {
            if (firingPotential <= 0) return;
            if (player.isCrouching) bullet.transform.position = gameObject.transform.position + new Vector3(0, vert_CrouchShootOffset, 0);
            else bullet.transform.position = gameObject.transform.position + new Vector3(0, 0, 0);
            bullet.transform.rotation = gameObject.transform.rotation;
            bullet.SetActive(true);
            ShootSound.Play();
            if (firingPotential >= 8.5) firingPotential -= 10;
            else firingPotential = -20;
        }

    }
}
