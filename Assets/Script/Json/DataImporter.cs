using LitJson;
using System.IO;
using UnityEngine;

public class DataImporter : MonoBehaviour
{
    public WeaponRamDB weaponRamDB;
    public WeaponEntryDB weaponEntryDB;
    public ArmorRamDB armorRamDB;

    // 从 JSON 文件导入武器数据
    public void ImportWeaponData()
    {
        string filePath = Application.dataPath + "/weaponData.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonData weaponData = JsonMapper.ToObject(json);

            foreach (JsonData weaponJson in weaponData["weaponClasses"])
            {
                WeaponClass weapon = new WeaponClass();
                weapon.id = (int)weaponJson["id"];
                weapon.name = weaponJson["name"].ToString();
                weapon.description = weaponJson["description"].ToString();
                weapon.weaponQ = (WeaponQ)System.Enum.Parse(typeof(WeaponQ), weaponJson["weaponQ"].ToString());
                weapon.damage = (float)weaponJson["damage"];

                // 导入武器词条
                weapon.weaponsEntry_a = DeserializeWeaponEntry(weaponJson["weaponsEntry_a"]);
                weapon.weaponsEntry_b = DeserializeWeaponEntry(weaponJson["weaponsEntry_b"]);
                weapon.weaponsEntry_c = DeserializeWeaponEntry(weaponJson["weaponsEntry_c"]);

                weaponRamDB.weaponClasses.Add(weapon);
            }
        }
    }

    // 从 JSON 解析武器词条
    private WeaponsEntryClass DeserializeWeaponEntry(JsonData entryJson)
    {
        WeaponsEntryClass entry = new WeaponsEntryClass();
        entry.id = (int)entryJson["id"];
        entry.name = entryJson["name"].ToString();
        entry.description = entryJson["description"].ToString();
        entry.isRam = (entryType)System.Enum.Parse(typeof(entryType), entryJson["isRam"].ToString());
        entry.uniqueEntry = (uniqueEntry)System.Enum.Parse(typeof(uniqueEntry), entryJson["uniqueEntry"].ToString());

        entry.valueType = new ramTypeClass[entryJson["valueType"].Count];
        for (int i = 0; i < entryJson["valueType"].Count; i++)
        {
            JsonData valueJson = entryJson["valueType"][i];
            entry.valueType[i] = new ramTypeClass();
            entry.valueType[i].ramValueType = (ramValueType)System.Enum.Parse(typeof(ramValueType), valueJson["ramValueType"].ToString());
            entry.valueType[i].realValue = (float)valueJson["realValue"];
            entry.valueType[i].minValue = (float)valueJson["minValue"];
            entry.valueType[i].maxValue = (float)valueJson["maxValue"];
        }

        return entry;
    }
}
