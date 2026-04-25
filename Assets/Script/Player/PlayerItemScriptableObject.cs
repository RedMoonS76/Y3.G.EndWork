using UnityEngine;

[CreateAssetMenu(fileName = "PlayerItemData", menuName = "item/PlayerData", order = 3)]
public class PlayerItemScriptableObject : ScriptableObject
{
    public WeaponClass weapon;
    public ArmorClass armorHead;
    public ArmorClass armorBody;
    public ArmorClass armorHand;
    public ArmorClass armorFoot;
}