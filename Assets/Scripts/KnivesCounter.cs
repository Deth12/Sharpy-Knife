using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnivesCounter : MonoBehaviour
{
    public static KnivesCounter instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);

        instance = this;
    }

    [Header("Knives Counter")]
    public GameObject knifeIcon;
    public GameObject knivesPanel;
    
    private List<Image> knifeIcons = new List<Image>();
    private int currentKnifeIndex = 0;
    
    [SerializeField]
    private Color thrownColor;
    [SerializeField]
    private Color activeColor;

    public void FillKnivesPanel(int amount)
    {
        if(!knivesPanel.activeSelf)
            knivesPanel.SetActive(true);
        
        if (knifeIcons.Count > 0)
        {
            foreach (Image img in knifeIcons)
            {
                Destroy(img.gameObject);
            }
            knifeIcons.Clear();
        }
        currentKnifeIndex = 0;
        for (int i = 0; i < amount; i++)
        {
            GameObject g = Instantiate(knifeIcon, knivesPanel.transform);
            knifeIcons.Add(g.GetComponent<Image>());
            knifeIcons[i].color = activeColor;
        }
    }

    public void DecreaseKnivesAmount()
    {
        knifeIcons[currentKnifeIndex].color = thrownColor;
        currentKnifeIndex++;
    }
    
    public void Hide()
    {
        knivesPanel.SetActive(false);
    }
}
