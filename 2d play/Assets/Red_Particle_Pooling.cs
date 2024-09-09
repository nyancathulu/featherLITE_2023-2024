using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Particle_Pooling : MonoBehaviour
{
    public static Red_Particle_Pooling Red_instance;

    private List<GameObject> pooledRedParticles = new List<GameObject>();

    [SerializeField] private int Red_amountToPool;

    [SerializeField] private GameObject Red_particlePrefab;

    //[SerializeField] Game_Ender_Script game_ender;

    private void Awake()
    {
        if (Red_instance == null)
        {
            Red_instance = gameObject.GetComponent<Red_Particle_Pooling>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Red_amountToPool; i++)
        {
            GameObject obj = Instantiate(Red_particlePrefab);
            //obj.GetComponent<Bullet_Script>().GameEnder = game_ender;
            obj.SetActive(false);
            pooledRedParticles.Add(obj);
        }

    }
    
    public void SendParticle(Vector3 position)
    {
        for (int j = 0; j < pooledRedParticles.Count; j++)
        {
            if (!pooledRedParticles[j].activeInHierarchy)
            {
                GameObject obj = pooledRedParticles[j];
                obj.transform.position = position;
                obj.SetActive(true);
                Blast_Sound_Pooler.SoundPooler_instance.PlayBlastSound();
                break;
            }
        }
    }
}
