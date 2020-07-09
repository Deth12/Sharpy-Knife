using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager instance;

    private void Awake()
    {
        if(instance != null)
            Destroy((gameObject));

        instance = this;
    }

    public GameObject lowDefaultEnemy;
    public GameObject mediumDefaultEnemy;
    
    public List<Boss> bossEnemies = new List<Boss>();
    
    public LevelTemplates levelTemplates;
    
    public Transform enemySpawnPoint;

    public int lowDifficultyBorder = 10;
    public int mediumDifficultyBorder = 30;
    public int highDifficultyBorder = 50;
    

    public Enemy GetDefaultEnemy(int level)
    {
        GameObject targetEnemy =
            (level <= lowDifficultyBorder
                ? lowDefaultEnemy
                : (level <= mediumDifficultyBorder ? mediumDefaultEnemy : mediumDefaultEnemy));
        GameObject enemyObj = Instantiate(targetEnemy, enemySpawnPoint.position, Quaternion.identity, enemySpawnPoint);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        LevelPattern lp = 
            (level <= lowDifficultyBorder ? 
                levelTemplates.easyLevelPatterns[Random.Range(0, levelTemplates.easyLevelPatterns.Count)] :
                (level <= mediumDifficultyBorder ? 
                    levelTemplates.mediumLevelPatterns[Random.Range(0, levelTemplates.mediumLevelPatterns.Count)] :
                    levelTemplates.hardLevelPatterns[Random.Range(0, levelTemplates.hardLevelPatterns.Count)]));
        RotationPattern rp = 
            (level <= lowDifficultyBorder ? 
                levelTemplates.easyRotationPatterns[Random.Range(0, levelTemplates.easyRotationPatterns.Count)] :
                (level <= mediumDifficultyBorder ? 
                    levelTemplates.mediumRotationPatterns[Random.Range(0, levelTemplates.mediumRotationPatterns.Count)] :
                    rp = levelTemplates.hardRotationPatterns[Random.Range(0, levelTemplates.hardRotationPatterns.Count)]));
        enemy.SetupEnemy(rp, lp);
        return enemy;
    }

    public Enemy GetRandomBossEnemy()
    {
        int rnd = Random.Range(0, bossEnemies.Count);
        GameObject enemyObj = Instantiate(bossEnemies[rnd].prefab,
            enemySpawnPoint.position, Quaternion.identity, enemySpawnPoint);
        Enemy enemy = enemyObj.GetComponent<Enemy>();    
        enemy.SetupEnemy(bossEnemies[rnd].rotationPattern,bossEnemies[rnd].levelPattern);
        UIManager.instance.UpdateBossName(bossEnemies[rnd].bossName);
        return enemy;
    }
}

[System.Serializable]
public class Boss
{
    public string bossName;
    public GameObject prefab;
    
    public RotationPattern rotationPattern;
    public LevelPattern levelPattern;
}