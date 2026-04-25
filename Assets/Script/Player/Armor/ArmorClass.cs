using UnityEngine;
public enum ArmorType
{
    empty,
    head,
    body,
    hand,
    foot
    
}

[System.Serializable]
public class ArmorClass
{
    public int id;
    public string name;
    public string description;
    public ArmorType type;
    public Sprite icon;
    public RPGattribute attriute;
}
