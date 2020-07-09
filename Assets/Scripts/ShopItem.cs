using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int id;
    
    public Image knifeImage;
    public Image lockedMask;
    public Image bg;
    public GameObject selectionBorder;

    [SerializeField]
    private Color unlockedColor;
    [SerializeField]
    private Color lockedColor;
    
    [SerializeField]
    private AudioClip unlockedSelectSound;
    [SerializeField]
    private AudioClip lockedSelectSound;
    
    public bool IsUnlocked
    {
        get => PlayerPrefs.GetInt("Knife_" + id, 0) == 1;
        set
        {
            PlayerPrefs.SetInt("Knife_" + id, value ? 1 : 0);
            UpdateUI(value);
        }
    }

    public bool IsSelected
    {
        get => selectionBorder.activeSelf;
        set
        {
            if (value)
            {
                if (ShopManager.instance.selectedItem != null)
                    ShopManager.instance.selectedItem.IsSelected = false;
                ShopManager.instance.UpdateSelectedItem(this);
                if (IsUnlocked)
                    IsEquipped = true;

            }
            selectionBorder.SetActive(value);
        }
    }

    public bool IsEquipped
    {
        get => PlayerPrefs.GetInt("EquippedKnifeID", 1) == id ? true : false;
        set => PlayerPrefs.SetInt("EquippedKnifeID", id);
    }
    
    public void Initialize(KnifeItem k)
    {
        id = k.id;
        knifeImage.sprite = k.prefab.GetComponent<SpriteRenderer>().sprite;
        knifeImage.preserveAspect = true;
        UpdateUI(IsUnlocked);
    }

    public void UpdateUI(bool isUnlocked)
    {
        bg.color = isUnlocked ? unlockedColor : lockedColor;
        lockedMask.enabled = !isUnlocked;
    }

    public void Select()
    {
        if (!ShopManager.instance.IsUnlockingRandom)
        {
            SoundManager.instance.PlayOneShot(IsUnlocked ? unlockedSelectSound : lockedSelectSound);
            IsSelected = true;
        }
    }

    public void Buy()
    {
        IsUnlocked = true;
        IsSelected = true;
    }
}
