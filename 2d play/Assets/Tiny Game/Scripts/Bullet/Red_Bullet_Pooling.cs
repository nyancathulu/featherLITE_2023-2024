using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Bullet_Pooling : MonoBehaviour
{

    public static Red_Bullet_Pooling Red_instance;

    private List<GameObject> pooledRedBullets = new List<GameObject>();

    [SerializeField] private int Red_amountToPool;

    [SerializeField] private GameObject Red_bulletPrefab;

    [SerializeField] Game_Ender_Script game_ender;

    private void Awake()
    {
        if (Red_instance == null)
        {
            Red_instance = gameObject.GetComponent<Red_Bullet_Pooling>();
        }
       
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Red_amountToPool; i++)
        {
            GameObject obj = Instantiate(Red_bulletPrefab);
            obj.GetComponent<Bullet_Script>().GameEnder = game_ender;
            obj.SetActive(false);
            pooledRedBullets.Add(obj);
        }
        Debug.Log(Red_instance);
    }

    public GameObject GetRedBullet()
    {
        for (int j = 0; j < pooledRedBullets.Count; j++)
        {
            if (!pooledRedBullets[j].activeInHierarchy)
            {
                return pooledRedBullets[j];
            }
        }

        return null;
    }
}
