using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponEntryDB", menuName = "item/WeaponEntryDB", order = 4)]
public class WeaponEntryDB : ScriptableObject
{
    [SerializeField]
    public List<WeaponsEntryClass> weaponsEntries;
}
