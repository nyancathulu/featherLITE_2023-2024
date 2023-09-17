using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Bullet_Pooling : MonoBehaviour
{

    public static Blue_Bullet_Pooling Blue_instance;

    private List<GameObject> pooledBlueBullets = new List<GameObject>();

    [SerializeField] private int Blue_amountToPool;

    [SerializeField] private GameObject Blue_bulletPrefab;

    [SerializeField] Game_Ender_Script game_ender;


    private void Awake()
    {
        if (Blue_instance == null)
        {
            Blue_instance = gameObject.GetComponent<Blue_Bullet_Pooling>();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Blue_amountToPool; i++)
        {
            GameObject obj = Instantiate(Blue_bulletPrefab);
            obj.GetComponent<Bullet_Script>().GameEnder = game_ender;
            obj.SetActive(false);
            pooledBlueBullets.Add(obj);
        }
    }

    public GameObject GetBlueBullet()
    {
        for (int j = 0; j < pooledBlueBullets.Count; j++)
        {
            if (!pooledBlueBullets[j].activeInHierarchy)
            {
                return pooledBlueBullets[j];
            }
        }

        return null;
    }
}
