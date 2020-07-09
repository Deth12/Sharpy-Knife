using System;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    private static bool _isGameOver;

    public static bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            _isGameOver = value;
            if(value)
                UpdateStatistics();
        }
    }
    
    #region Gameplay

    private static int _level;
    public static int Level
    {
        get => _level;
        set
        {
            _level = value;
            OnLevelChange?.Invoke(_level);
        }
    }
    
    private static int _score;

    public static int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChange?.Invoke(_score);
        }
    }

    private static int _apples;
    public static int Apples
    {
        get => PlayerPrefs.GetInt ("ApplesAmount", 0);
        set
        {
            _apples = value;
            PlayerPrefs.SetInt ("ApplesAmount", value);
            OnAppleChange?.Invoke(_apples);
        }
    }

  
    private static int _highscore;
    public static int Highscore
    {
        get => PlayerPrefs.GetInt ("Highscore", 0);
        set
        {
            _highscore = value;
            PlayerPrefs.SetInt ("Highscore", value);
        }
    }
    
    
    private static int _maxLevel;
    public static int MaxLevel
    {
        get => PlayerPrefs.GetInt ("MaxLevel", 0);
        set
        {
            _apples = value;
            PlayerPrefs.SetInt ("MaxLevel", value);
        }
    }

    public static DateTime GiftTimeLeft
    {
        get => DateTime.FromFileTime(long.Parse(
            PlayerPrefs.GetString("LastGiftTime", DateTime.Now.ToFileTime().ToString())
            ));
        set => PlayerPrefs.SetString("LastGiftTime", value.ToFileTime().ToString());
    }

    public static TimeSpan RemainingTimeToGift
    {
        get => GiftTimeLeft - DateTime.Now;
    }

    public static bool IsGiftAvailable
    {
        get => RemainingTimeToGift.TotalSeconds <= 0;
    }

    public static Action<int> OnLevelChange;
    public static Action<int> OnScoreChange;
    public static Action<int> OnAppleChange;

    public static void NewGame()
    {
        IsGameOver = false;
        Score = 0;
        Level = 1;
    }
    
    private static void UpdateStatistics()
    {
        if (MaxLevel < Level)
            MaxLevel = Level;
        if (Highscore < Score)
            Highscore = Score;
    }
    #endregion
    
    #region Settings

    public static bool Sound
    {
        get => PlayerPrefs.GetInt("Sound", 1) == 1;
        set => PlayerPrefs.SetInt("Sound", value ? 1 : 0);
    }

    public static bool Vibration
    {
        get => PlayerPrefs.GetInt("Vibration", 1) == 1;
        set => PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
    }
    
    #endregion



}
