using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ArmorData", menuName = "item/ArmorData", order = 1)]
public class ArmorRamDB : ScriptableObject
{
    [SerializeField]
    public List<ArmorClass> armorClasses;
}
