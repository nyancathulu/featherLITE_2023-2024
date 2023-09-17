using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ready_Up_Screen : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] GameObject Canvas;
    [SerializeField] Game_Ender_Script GameSequencer;

    private void Start()
    {
        StartCoroutine(ReadyUpCoroutine());
    }

    public IEnumerator ReadyUpCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Canvas.SetActive(false);
                GameSequencer.BuildMap();
                yield break;
            }
            yield return 0;
        }
    }
}
