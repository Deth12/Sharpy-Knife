using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KnifeHit/KnivesList")]
public class KnivesList : ScriptableObject
{
    public List<KnifeItem> knives = new List<KnifeItem>();
}

[System.Serializable]
public class KnifeItem
{
    public int id;
    public GameObject prefab;
    public int price;
}
