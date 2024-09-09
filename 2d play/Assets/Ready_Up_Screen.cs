using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class Ready_Up_Screen : MonoBehaviour
{
    [SerializeField] int ReadyUpSeconds;
    [SerializeField] GameObject Canvas;
    [SerializeField] Game_Ender_Script GameSequencer;

    [SerializeField] List<InputActionReference>  Player1Inputs = new List<InputActionReference>();
    [SerializeField] List<InputActionReference> Player2Inputs = new List<InputActionReference>();

    [SerializeField] GameObject RedX;
    [SerializeField] GameObject BlueX;
    [SerializeField] GameObject RedC;
    [SerializeField] GameObject BlueC;

    [SerializeField] TextMeshProUGUI timeText;

    private bool Player1_ready;
    private bool Player2_ready;

    [SerializeField] AudioSource CountdownSound;

    [SerializeField] bool hasMusic;

    bool musicMuted;

    [Header("Only Put This if hasMusic = true and this is the display scene")]
    [SerializeField] AudioSource music;
    [SerializeField] float fadeOutSpeed;

    

    private void Start()
    {
        timeText.text = "";
        Player1_ready = false;
        Player2_ready = false;
        StartCoroutine(ReadyUpCoroutine());

        if (hasMusic)
        {
            musicMuted = false;
        }
    }

    public IEnumerator ReadyUpCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < Player1Inputs.Count; i++)
            {
                if (Player1Inputs[i].action.triggered)
                {
                    RedX.SetActive(false);
                    RedC.SetActive(true);
                    Player1_ready = true;

                    if (hasMusic)
                    {
                        if (!musicMuted)
                        {
                            StartCoroutine(StopMusicCoroutine());
                        }
                    }
                }
            }

            for (int i = 0; i < Player2Inputs.Count; i++)
            {
                if (Player2Inputs[i].action.triggered)
                {
                    BlueX.SetActive(false);
                    BlueC.SetActive(true);
                    Player2_ready = true;

                    if (hasMusic)
                    {
                        if (!musicMuted)
                        {
                            StartCoroutine(StopMusicCoroutine());
                        }
                    }
                }
            }

            if (Player1_ready && Player2_ready)
            {
                break;
            }
            yield return 0;
        }
        float timer = (float)ReadyUpSeconds;
        CountdownSound.Play();
        while (timer > 0)
        {
            DisplayTime(timer+1);
            timer -= Time.deltaTime;
            yield return 0;
        }
        GameSequencer.BuildMap();
        Canvas.SetActive(false);
        yield break;

    }

    void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = seconds.ToString();
    }



    public IEnumerator StopMusicCoroutine()
    {

        if (musicMuted) yield break;

        float volume = music.volume;

        while (volume > 0)
        {
            volume -= Time.deltaTime;

            music.volume = volume;

            yield return null;
        }

        music.volume = 0;

        yield break;
    }
}
