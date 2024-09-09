using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Particle_Pooling : MonoBehaviour
{
    public static Blue_Particle_Pooling Blue_instance;

    private List<GameObject> pooledBlueParticles = new List<GameObject>();

    [SerializeField] private int Blue_amountToPool;

    [SerializeField] private GameObject Blue_particlePrefab;

    //[SerializeField] Game_Ender_Script game_ender;

    private void Awake()
    {
        if (Blue_instance == null)
        {
            Blue_instance = gameObject.GetComponent<Blue_Particle_Pooling>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Blue_amountToPool; i++)
        {
            GameObject obj = Instantiate(Blue_particlePrefab);
            //obj.GetComponent<Bullet_Script>().GameEnder = game_ender;
            obj.SetActive(false);
            pooledBlueParticles.Add(obj);
        }

    }

    public void SendParticle(Vector3 position)
    {
        for (int j = 0; j < pooledBlueParticles.Count; j++)
        {
            if (!pooledBlueParticles[j].activeInHierarchy)
            {
                GameObject obj = pooledBlueParticles[j];
                obj.transform.position = position;
                obj.SetActive(true);
                Blast_Sound_Pooler.SoundPooler_instance.PlayBlastSound();
                break;
            }
        }
    }
}
