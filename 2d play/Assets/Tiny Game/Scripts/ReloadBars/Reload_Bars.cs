using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload_Bars : MonoBehaviour
{
    public GameObject RedBar;
    public GameObject BlueBar;
    public Image RedFill;
    public Image BlueFill;
    private Vector3 OG_Scale;
    public Game_Ender_Script gameEnder;
    private void OnEnable()
    {
        OG_Scale = RedFill.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        RedFill.gameObject.transform.localScale = new Vector3(OG_Scale.x, OG_Scale.y * Mathf.Clamp((gameEnder.redShooter.firingPotential/gameEnder.redShooter.totalFirePotential), 0, 1), OG_Scale.z);
        BlueFill.gameObject.transform.localScale = new Vector3(OG_Scale.x, OG_Scale.y * Mathf.Clamp((gameEnder.blueShooter.firingPotential / gameEnder.blueShooter.totalFirePotential), 0, 1), OG_Scale.z);
    }
}
