using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PlayerPrefsSettings")]
public class PlayerPrefsSettings : ScriptableObject
{
    public int GetHighscore()
    {
        return PlayerPrefs.GetInt("Highscore", 0);
    }

    public int GetMaxLevel()
    {
        return PlayerPrefs.GetInt("MaxLevel", 0);
    }
    
    public int GetApples()
    {
        return PlayerPrefs.GetInt ("ApplesAmount", 0);
    }
    
    public void AddApples(int amount)
    {
        PlayerPrefs.SetInt("ApplesAmount", PlayerPrefs.GetInt("ApplesAmount") + amount);
    }
 
    public void ResetAchievements()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.SetInt("MaxLevel", 0);
        PlayerPrefs.SetInt("ApplesAmount", 0);
    }
    
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Knife_1", 1);
    }
}
