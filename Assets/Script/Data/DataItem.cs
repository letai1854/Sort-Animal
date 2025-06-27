using UnityEngine;

[CreateAssetMenu(fileName = "NewDataItem", menuName = "Shop/Data Item")]
public class DataItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int value;
    public bool isPurchasable;
    public bool isChoiceItem;
    public AnimalHabitatType habitatType;
}

