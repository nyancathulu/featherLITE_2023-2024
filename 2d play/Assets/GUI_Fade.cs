using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Fade : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> SpriteElements;

    [SerializeField] int Player_Number;

    public float speed;

    public AnimationCurve fadeCurve;

    void OnEnable()
    {
        Game_Ender_Script.OnTinyDeath += FadeAway;
    }

    public void FadeAway(int loser, GameObject bullet)
    {
        if (Player_Number == loser) StartCoroutine(FadeCoroutine(bullet.GetComponent<SpriteRenderer>()));
    }

    public IEnumerator FadeCoroutine(SpriteRenderer _bullet)
    {
        float t = 0;
        while (true)
        {

            if (t > 1)
            {
                t = 1;
            }

            else
            {
                t += speed * Time.deltaTime;
            }

            float fadeValue = fadeCurve.Evaluate(Mathf.Clamp(t, 0, 1));

            for (int i = 0; i < SpriteElements.Count; i++)
            {
                SpriteElements[i].color = new Color(SpriteElements[i].color.r, SpriteElements[i].color.g, SpriteElements[i].color.b, fadeValue);
            }

            _bullet.color = new Color(_bullet.color.r, _bullet.color.g, _bullet.color.b, fadeValue);

            if (t == 1)
            {
                yield break;
            }

            yield return 0;
        }
    }


}
