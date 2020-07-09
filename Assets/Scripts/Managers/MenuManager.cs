using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);

        instance = this;
    }

    public AudioClip buttonClick;
    
    [Header("Main Panels")]
    public GameObject homePanel;
    public GameObject shopPanel;
    public GameObject settingsPanel;
    public GameObject endgamePanel;
    public Image selectedKnife;
    
    [Header("Endgame Panel")]
    public Text finalScoreCounter;
    public Text finalLevelCounter;
    public Text bestScoreCounter;
    public Text bestLevelCounter;

    [Header("Settings")] 
    public Toggle soundToggle;
    public Toggle vibrationToggle;

    [Header("Gift")] 
    public EventTrigger giftButton;
    public GameObject giftPanel;
    public CanvasGroup giftCanvasGroup;
    public Text timeLeft;
    public ParticleSystem giftEffect;
    public AudioClip giftSound;
    public Text applesGifted;
    [SerializeField]
    private int maxApples = 100;
    [SerializeField]
    private int minApples = 50;
    [SerializeField]
    private int timeBtwGifts = 60;
    [SerializeField]
    
    void Start()
    {
        InitializeSettings();
        homePanel.SetActive(true);
        shopPanel.SetActive(false);
        settingsPanel.SetActive(false);
        giftPanel.SetActive(false);
        endgamePanel.SetActive(false);
        InvokeRepeating(nameof(UpdateGiftStatus), 0f, 1f);
    }

    public void InitializeSettings()
    {
        soundToggle.isOn = GameStatus.Sound;
        vibrationToggle.isOn = GameStatus.Vibration;
        soundToggle.onValueChanged.AddListener(x =>
        {
            GameStatus.Sound = x;
            ButtonClickSound();
        });
        vibrationToggle.onValueChanged.AddListener(x =>
        {
            GameStatus.Vibration = x;
            ButtonClickSound();
        });
    }

    public void PlayGame()
    {
        GameManager.instance.StartGame();
        homePanel.SetActive(false);
    }

    public void ShowEndgameScreen()
    {
        endgamePanel.SetActive(true);
        finalScoreCounter.text = GameStatus.Score.ToString();
        finalLevelCounter.text = GameStatus.Level.ToString();
        bestScoreCounter.text = GameStatus.Highscore.ToString();
        bestLevelCounter.text = GameStatus.MaxLevel.ToString();
    }

    public void RestartGame()
    {
        endgamePanel.SetActive(false);
        GameManager.instance.RestartGame();
    }

    public void UpdateSelectedKnife(ShopItem item)
    {
        selectedKnife.sprite = item.knifeImage.sprite;
    }

    public void ButtonClickSound()
    { 
        SoundManager.instance.PlayOneShot(buttonClick);   
    }

    public void UpdateGiftStatus()
    {
        if (GameStatus.IsGiftAvailable)
        {
            giftButton.enabled = true;
            timeLeft.text = "Ready!";
        }
        else
        {
            giftButton.enabled = false;
            timeLeft.text =
                GameStatus.RemainingTimeToGift.Hours.ToString("00") + ":" +
                GameStatus.RemainingTimeToGift.Minutes.ToString("00") + ":" +
                GameStatus.RemainingTimeToGift.Seconds.ToString("00");
        }
    }

    public void GetGift()
    {
        int applesAmount = Random.Range(minApples, maxApples + 1);
        applesGifted.text = "+" + applesAmount.ToString();
        GameStatus.Apples += applesAmount;
        GameStatus.GiftTimeLeft = DateTime.Now.AddMinutes(timeBtwGifts);
        UpdateGiftStatus();
        giftPanel.SetActive(true);
        Instantiate(giftEffect);
        SoundManager.instance.PlayOneShot(giftSound);
        LeanTween.alphaCanvas (giftCanvasGroup, 0f, 2f).setOnComplete (() =>
        {
            giftPanel.SetActive(false);
            giftCanvasGroup.alpha = 1f;
        });
    }
}
