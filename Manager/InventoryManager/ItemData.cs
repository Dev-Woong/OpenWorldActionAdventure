using UnityEngine;

public enum ItemType 
{
    Equipment,
    Consumable,
    QuestItem,
    ETC
}
public enum EquipType
{
    None,
    Weapon,
    Helmet,
    Armor,
    Belt,
    Gloves,
    Boots
}

[CreateAssetMenu (fileName = "ItemData", menuName = "Item/Item Data")]
public class ItemData: ScriptableObject
{
    [Header("�⺻ ����")]
    public int id;
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int amount;
    [TextArea]
    public string itemDescription;

    [Header("���Ÿ��")]
    public EquipType equipType;
    [Header("����")]
    public int maxStack = 99;

    [Header("��� ������ �ɼ�")]
    public float equipAtkBonus;
    public float equipDefBonus;
    public float equipHpBonus;
    public float equipMpBonus;

    [Header("�Һ� ������ ȿ��")]
    public int hpRecovery;
    public int mpRecovery;
}
