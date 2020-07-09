using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);

        instance = this;
    }

    [Header("Stats")] 
    [SerializeField]
    private Text scoreCounter;
    [SerializeField]
    private Text appleCounter;
    
    [Header("Progress")]
    [SerializeField]
    private Text levelCounter;
    [SerializeField]
    private GameObject topPanel;
    [SerializeField]
    private GameObject levelProgress;
    private Animator levelProgressAnimator;
    [SerializeField]
    private Image[] levelDots;
    [SerializeField]
    private Color activeDotColor;
    [SerializeField]
    private Color normalDotColor;
    [SerializeField]
    private Color bossDotColor;
    
    [Header("Boss Battles")]
    [SerializeField]
    private GameObject bossOverlay;
    [SerializeField]
    private GameObject bossDefeatOverlay;

    private void Start()
    {
        topPanel.SetActive(false);
        UpdateApplesCounter(GameStatus.Apples);
        levelProgressAnimator = levelProgress.GetComponentInChildren<Animator>();

        GameStatus.OnAppleChange += UpdateApplesCounter;
        GameStatus.OnScoreChange += UpdateScoreCounter;
        GameStatus.OnLevelChange += UpdateLevelProgress;
    }
    
    
    private void UpdateLevelProgress(int level)
    {
        if (level % 5 == 0)
            levelProgressAnimator.Play("LevelProgress_Minimize");
        else if (level != 1 && level % 5 == 1)
        {
            levelProgressAnimator.Play("LevelProgress_Maximize");
            levelCounter.text = "LEVEL: " + level.ToString();
        }
        else
            levelCounter.text = "LEVEL: " + level.ToString();
        UpdateLevelDots(level);
    }

    private void UpdateLevelDots(int level)
    {
        for (int i = 0; i < levelDots.Length; i++) 
        {
            levelDots[i].color = level  % levelDots.Length <= i ? normalDotColor : activeDotColor;
            levelCounter.color = activeDotColor;
            if (level % levelDots.Length == 0)
            {
                levelDots[i].color = bossDotColor;
                levelCounter.color = bossDotColor;
            }
            // TOFIX
        }
    }

    public void UpdateBossName(string bossName)
    {
        levelCounter.text = "BOSS: " + bossName;
    }

    private void UpdateScoreCounter(int value)
    {
        scoreCounter.text = value.ToString();
    }

    private void UpdateApplesCounter(int value)
    {
        appleCounter.text = value.ToString();
    }

    public void SwitchBossOverlay(bool status)
    {
        bossOverlay.SetActive(status);
    }

    public void SwitchBossDefeatedOverlay(bool status)
    {
        bossDefeatOverlay.SetActive(status);
    }

    public void SwitchGamePanel(bool status)
    {
        topPanel.SetActive(status);
    }
}
