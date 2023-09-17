using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Ender_Script : MonoBehaviour
{

    // 0 = Red
    // 1 = Blue
    public delegate void Tiny_DeathEvent(int _loser, GameObject killing_bullet);
    public static event Tiny_DeathEvent OnTinyDeath;

    [SerializeField] GameObject Red_Player;
    [SerializeField] GameObject Blue_Player;

    GameObject Red_Instance;

    GameObject Blue_Instance;

    public List<Transform> Red_SpawnSpots;

    public List<Transform> Blue_SpawnSpots;

    bool GameEnded;

    float speed_factor;

    float afterFadeTime;

    [SerializeField] float little_delay;

    [SerializeField] GameObject red_WinScreen;

    [SerializeField] GameObject blue_WinScreen;

    [SerializeField] RoomBuildingScript MapBuilder;

    private void Start()
    {
        GameEnded = false;
        speed_factor = Red_Player.GetComponentInChildren<GUI_Fade>().speed;
        afterFadeTime = 1 / speed_factor + little_delay;
    }

    public void BuildMap()
    {
        StartCoroutine(MapBuildingCoroutine());
    }

    public IEnumerator MapBuildingCoroutine()
    {
        //send to map builder

        MapBuilder.BuildMaps();

        //add delay here if needed

        SpawnCharacters();

        yield break;
    }


    public void SpawnCharacters()
    {
        Transform RedSpawn = Red_SpawnSpots[Random.Range(0, Red_SpawnSpots.Count - 1)];
        Transform BlueSpawn = Blue_SpawnSpots[Random.Range(0, Blue_SpawnSpots.Count - 1)];

        Red_Instance = Instantiate(Red_Player, RedSpawn.position, Quaternion.identity);
        Blue_Instance = Instantiate(Blue_Player, BlueSpawn.position, Quaternion.identity);


    }











    public void EndGame(int loser, GameObject killingBullet)
    {
        if (!GameEnded)
        {
            GameEnded = true;
            if (OnTinyDeath != null) OnTinyDeath(loser, killingBullet);
            StartCoroutine(EndingCourutine(loser, killingBullet));
        }
    }

    public IEnumerator EndingCourutine(int _loser, GameObject _bullet)
    {
        float timer = afterFadeTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }

        _bullet.SetActive(false);

        if (_loser == 0) 
        {
            //Red_Instance.SetActive(false);
            //Debug.LogAssertion("Blue Wins!");
            blue_WinScreen.SetActive(true);
        }

        if (_loser == 1) 
        {
            //Blue_Instance.SetActive(false);
            red_WinScreen.SetActive(true);
            //Debug.LogAssertion("Red Wins!");
        }
    }












    /*public delegate void Tiny_DeathEvent(Vector2 p);
    public static event Tiny_DeathEvent OnTinyDeath;


    public void SendEvent()
    {
        if (OnTinyDeath != null) OnTinyDeath(Vector2.zero);
    }*/
}
