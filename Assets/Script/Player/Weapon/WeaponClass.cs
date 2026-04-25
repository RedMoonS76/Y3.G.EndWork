using UnityEngine;
public enum WeaponQ
{
    empty,
    Nom,
    High,
    Unique
}
[System.Serializable]
public class WeaponClass
{
    public int id;
    public string name;
    public string description;
    public WeaponQ weaponQ;
    public float damage;
    public bool isClosed;
    public Sprite icon;
    public Sprite weaponModel;
    public WeaponsEntryClass weaponsEntry_a;
    public WeaponsEntryClass weaponsEntry_b;
    public WeaponsEntryClass weaponsEntry_c;
}
