using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponData", menuName = "item/WeaponData", order = 2)]
public class WeaponRamDB : ScriptableObject
{
    [SerializeField]
    public List<WeaponClass> weaponClasses;
}
