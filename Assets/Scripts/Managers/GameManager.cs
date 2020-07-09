using System.Collections;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
    }
    
    [Header("Knife")]
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    private Knife currentKnife;
    
    [Header("Enemies")]
    private Enemy currentEnemy;
    private bool isCurrentEnemyBoss = false;
    
    [Header("Boss Battles")]
    public AudioClip bossFight;
    public AudioClip bossDefeated;
    
    [Header("On Enemy Objects")] 
    public GameObject knifeObject;
    public GameObject appleObject;
    
    private int spawnedKnives = 0;
    

    public void StartGame()
    {
        knifePrefab = ShopManager.instance.GetSelectedKnifePrefab();
        UIManager.instance.SwitchGamePanel(true);
        GameStatus.NewGame();
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        spawnedKnives = 0;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (GameStatus.Level % 5 != 0)
        {
            currentEnemy = EnemiesManager.instance.GetDefaultEnemy(GameStatus.Level);
            isCurrentEnemyBoss = false;
            StartCoroutine(SpawnKnife());
            KnivesCounter.instance.FillKnivesPanel(currentEnemy.Lives);
        }
        else
        {
            currentEnemy = EnemiesManager.instance.GetRandomBossEnemy();
            isCurrentEnemyBoss = true;
            StartCoroutine(OnBossSpawn());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentKnife && !currentKnife.IsThrown)
            {
                currentKnife.Throw();
                KnivesCounter.instance.DecreaseKnivesAmount();
                StartCoroutine(SpawnKnife());
            }
        }
    }

    private IEnumerator SpawnKnife()
    {
        yield return new WaitUntil(() => { return knifeSpawnPoint.childCount == 0;});
        if (currentEnemy.Lives > spawnedKnives && !GameStatus.IsGameOver ) 
        {
            spawnedKnives++;
            Vector3 spawnPos = new Vector3(knifeSpawnPoint.position.x, knifeSpawnPoint.position.y - 2.5f, knifeSpawnPoint.position.z);
            GameObject knifeObj = Instantiate(knifePrefab, spawnPos, Quaternion.identity, knifeSpawnPoint);
            LeanTween.moveLocalY (knifeObj, 0, 0.1f);
            knifeObj.name ="Knife_" + spawnedKnives; 
            currentKnife = knifeObj.GetComponent<Knife>();
        }
    }

    private IEnumerator OnBossSpawn()
    {
        SoundManager.instance.PlayOneShot(bossFight);
        UIManager.instance.SwitchBossOverlay(true);
        yield return new WaitForSeconds(2f);
        UIManager.instance.SwitchBossOverlay(false);
        StartCoroutine(SpawnKnife());
        KnivesCounter.instance.FillKnivesPanel(currentEnemy.Lives);
    }
    
    private IEnumerator OnBossDefeat()
    {
        SoundManager.instance.PlayOneShot(bossDefeated);
        GameStatus.Level++;
        UIManager.instance.SwitchBossDefeatedOverlay(true);
        yield return new WaitForSeconds(2f);
        UIManager.instance.SwitchBossDefeatedOverlay(false);
        InitializeLevel();
    }

    private IEnumerator OnEnemyDefeat()
    {
        GameStatus.Level++;
        yield return new WaitForSeconds(1f);
        InitializeLevel();
    }
    
    public void InitializeNextLevel()
    {
        if (isCurrentEnemyBoss)
            StartCoroutine(OnBossDefeat());
        else
            StartCoroutine(OnEnemyDefeat());
    }

    public void GameOver()
    {
        if(currentKnife != null)
            Destroy(currentKnife.gameObject);
        if (currentEnemy != null)
            currentEnemy.SafeDestroy(0f);
        GameStatus.IsGameOver = true;
        UIManager.instance.SwitchGamePanel(false);
        KnivesCounter.instance.Hide();
        MenuManager.instance.ShowEndgameScreen();
    }

    public void RestartGame()
    {
        GameStatus.Level = 0;
        GameStatus.Score = 0;
        StartGame();
    }
    
    
}
