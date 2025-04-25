using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] Red_Bullet_Pooling RED_bullet_pooler;

    [SerializeField] Blue_Bullet_Pooling BLUE_bullet_pooler;

    [SerializeField] List<GameObject> TilemapsToDestroy = new List<GameObject>();

    [SerializeField] AudioSource WinSound;

    [SerializeField] private string StartMenu;

    [SerializeField] float waitTime_BeforeLoadingStartMenu;

    [SerializeField] GameObject SlimePads;

    [SerializeField] bool isDemo;

    [HideInInspector] public Red_Shooting redShooter;
    [HideInInspector] public Blue_Shooting blueShooter;

    public Reload_Bars ReloadBars_UI;
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
        SlimePads.SetActive(true);

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

        redShooter = Red_Instance.GetComponentInChildren<Red_Shooting>();
        blueShooter = Blue_Instance.GetComponentInChildren<Blue_Shooting>();

        ReloadBars_UI.gameObject.SetActive(true);
    }











    public void EndGame(int loser, GameObject killingBullet)
    {
        if (!GameEnded)
        {
            GameEnded = true;
            if (OnTinyDeath != null) OnTinyDeath(loser, killingBullet);
            StartCoroutine(EndingCourutine(loser, killingBullet));
            ReloadBars_UI.gameObject.SetActive(false);
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

        if (_loser == 0) 
        {
            //Red_Instance.SetActive(false);
            //Debug.LogAssertion("Blue Wins!");
            BLUE_bullet_pooler.DisableBullets();
            blue_WinScreen.SetActive(true);
        }

        if (_loser == 1) 
        {
            //Blue_Instance.SetActive(false);
            RED_bullet_pooler.DisableBullets();
            red_WinScreen.SetActive(true);
            //Debug.LogAssertion("Red Wins!");
        }
        WinSound.Play();
        float timer2 = waitTime_BeforeLoadingStartMenu;

        while (timer2 > 0)
        {
            timer2 -= Time.deltaTime;
            yield return 0;
        }
        if (!isDemo) SceneManager.LoadScene(StartMenu);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
        //DestroyTilemaps();
        
    }

    void DestroyTilemaps()
    {
        for (int i = 0; i < TilemapsToDestroy.Count; i++) TilemapsToDestroy[i].SetActive(false);
    }











    /*public delegate void Tiny_DeathEvent(Vector2 p);
    public static event Tiny_DeathEvent OnTinyDeath;


    public void SendEvent()
    {
        if (OnTinyDeath != null) OnTinyDeath(Vector2.zero);
    }*/
}
