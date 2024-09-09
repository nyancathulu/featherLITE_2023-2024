using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast_Sound_Pooler : MonoBehaviour
{
    public static Blast_Sound_Pooler SoundPooler_instance;

    private List<AudioSource> pooledAudioSources = new List<AudioSource>();

    [SerializeField] private int AmountToPool;

    [SerializeField] private GameObject BlastAudioSource_Prefab;

    //[SerializeField] Game_Ender_Script game_ender;

    private void Awake()
    {
        if (SoundPooler_instance == null)
        {
            SoundPooler_instance = gameObject.GetComponent<Blast_Sound_Pooler>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            GameObject obj = Instantiate(BlastAudioSource_Prefab);
            //obj.GetComponent<Bullet_Script>().GameEnder = game_ender;
            pooledAudioSources.Add(obj.GetComponent<AudioSource>());
        }

    }

    public void PlayBlastSound()
    {
        for (int j = 0; j < pooledAudioSources.Count; j++)
        {
            if (!pooledAudioSources[j].isPlaying)
            {
                pooledAudioSources[j].GetComponent<AudioSource>().Play();
                break;
            }
        }

       
    }
}
