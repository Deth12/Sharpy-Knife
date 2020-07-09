using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);

        instance = this;
    }

    public KnivesList knivesList;
    public GameObject shopItemPrefab;
    public Transform itemsContainer;
    
    [Header("Selected Knife")]
    public Image selectedKnife;
    public Image lockedMask;
    public GameObject glowEffect;
    
    [Header("Buttons")]
    public GameObject buyButton;
    public Text buyPrice;
    public GameObject buyRandomButton;
    public GameObject backButton;

    [HideInInspector]
    public ShopItem selectedItem;
    
    public List<ShopItem> shopItems = new List<ShopItem>();
    public int randomUnlockPrice = 50;
    
    [Header("Sounds")]
    public AudioClip buySound;
    public AudioClip randomStepSound;

    private bool _isUnlockingRandom;
    public bool IsUnlockingRandom
    {
        get => _isUnlockingRandom;
        private set
        {
            _isUnlockingRandom = value;
            buyRandomButton.SetActive(!_isUnlockingRandom);
            buyButton.SetActive(!_isUnlockingRandom);
            backButton.SetActive(!_isUnlockingRandom);
        }
    }
    
    
    void Start()
    {
        PlayerPrefs.SetInt("Knife_1", 1);
        if (knivesList == null) return;
        foreach (var knifeItem in knivesList.knives)
        {
            GameObject shopItemObj = Instantiate(shopItemPrefab, itemsContainer);
            ShopItem shopItem = shopItemObj.GetComponent<ShopItem>();
            shopItem.Initialize(knifeItem);
            shopItems.Add(shopItem);
        }
        UpdateCurrentEquippedKnife();
    }

    private void UpdateCurrentEquippedKnife()
    {
        int selectedIndex = PlayerPrefs.GetInt("EquippedKnifeID", 1);
        ShopItem current = shopItems.Find(x => x.id == selectedIndex);
        current.IsSelected = true;
        selectedItem = current;
        MenuManager.instance.UpdateSelectedKnife(selectedItem); 
    }

    public void UpdateSelectedItem(ShopItem item)
    {
        selectedItem = item;
        selectedKnife.sprite = selectedItem.knifeImage.sprite;
        bool isUnlocked = selectedItem.IsUnlocked;
        buyPrice.text = isUnlocked ? "" : knivesList.knives.Find(x => x.id == selectedItem.id).price.ToString();
        buyButton.SetActive(!IsUnlockingRandom && !isUnlocked);
        lockedMask.enabled = !isUnlocked;
        glowEffect.SetActive(isUnlocked);
        if(isUnlocked)
            MenuManager.instance.UpdateSelectedKnife(selectedItem);
    }
    
    public GameObject GetSelectedKnifePrefab()
    {
        return knivesList.knives.Find
            (x => x.id == PlayerPrefs.GetInt("EquippedKnifeID", 1)).prefab;
    }

    public void BuySelectedItem()
    {
        int price = knivesList.knives.Find(x => x.id == selectedItem.id).price;
        if (GameStatus.Apples >= price)
        {
            GameStatus.Apples -= price;
            SoundManager.instance.PlayOneShot(buySound);
            selectedItem.Buy();
        }
    }

    public void BuyRandomItem()
    {
        if(randomUnlockPrice <= GameStatus.Apples)
            StartCoroutine(UnlockRandomKnife());
    }
    
    IEnumerator UnlockRandomKnife()
    {
        IsUnlockingRandom = true;
        List<ShopItem> lockedItems=shopItems.FindAll(x => !x.IsUnlocked);
        ShopItem randomSelect = null;
        for (int i = 0; i < lockedItems.Count * 2; i++) 
        {
            randomSelect = lockedItems[UnityEngine.Random.Range(0, lockedItems.Count)];
            if (!randomSelect.IsSelected) {
                randomSelect.IsSelected = true;
            }
            SoundManager.instance.PlayOneShot(randomStepSound, 0.4f);
            yield return new WaitForSeconds (.1f);
        }
        IsUnlockingRandom = false;
        GameStatus.Apples -= randomUnlockPrice;
        SoundManager.instance.PlayOneShot(buySound);
        selectedItem.Buy();
    }

    public void RestorePurchasesButton()
    {
        for (int i = 1; i < shopItems.Count; i++)
        {
            shopItems[i].IsUnlocked = false;
        }
        PlayerPrefs.SetInt("EquippedKnifeID", 1); 
        UpdateCurrentEquippedKnife();
    }
    
}
