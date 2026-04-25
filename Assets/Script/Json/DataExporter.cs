using LitJson;
using System.IO;
using UnityEngine;

public class DataExporter : MonoBehaviour
{
    public WeaponRamDB weaponRamDB;
    public WeaponEntryDB weaponEntryDB;
    public ArmorRamDB armorRamDB;

    // 导出武器数据到 JSON 文件
    public void ExportWeaponData()
    {
        JsonData weaponData = new JsonData();
        weaponData["weaponClasses"] = new JsonData();

        foreach (var weapon in weaponRamDB.weaponClasses)
        {
            JsonData weaponJson = new JsonData();
            weaponJson["id"] = weapon.id;
            weaponJson["name"] = weapon.name;
            weaponJson["description"] = weapon.description;
            weaponJson["weaponQ"] = weapon.weaponQ.ToString();
            weaponJson["damage"] = weapon.damage;

            // 处理武器词条
            weaponJson["weaponsEntry_a"] = SerializeWeaponEntry(weapon.weaponsEntry_a);
            weaponJson["weaponsEntry_b"] = SerializeWeaponEntry(weapon.weaponsEntry_b);
            weaponJson["weaponsEntry_c"] = SerializeWeaponEntry(weapon.weaponsEntry_c);

            weaponData["weaponClasses"].Add(weaponJson);
        }

        string json = weaponData.ToJson();
        File.WriteAllText(Application.dataPath + "/weaponData.json", json);
    }

    // 导出武器词条数据到 JSON 文件
    public void ExportWeaponEntryData()
    {
        JsonData entryData = new JsonData();
        entryData["weaponsEntries"] = new JsonData();

        foreach (var entry in weaponEntryDB.weaponsEntries)
        {
            JsonData entryJson = new JsonData();
            entryJson["id"] = entry.id;
            entryJson["name"] = entry.name;
            entryJson["description"] = entry.description;
            entryJson["isRam"] = entry.isRam.ToString();
            entryJson["uniqueEntry"] = entry.uniqueEntry.ToString();

            // 处理每个词条的属性值
            entryJson["valueType"] = new JsonData();
            foreach (var value in entry.valueType)
            {
                JsonData valueJson = new JsonData();
                valueJson["ramValueType"] = value.ramValueType.ToString();
                valueJson["realValue"] = value.realValue;
                valueJson["minValue"] = value.minValue;
                valueJson["maxValue"] = value.maxValue;

                entryJson["valueType"].Add(valueJson);
            }

            entryData["weaponsEntries"].Add(entryJson);
        }

        string json = entryData.ToJson();
        File.WriteAllText(Application.dataPath + "/weaponEntryData.json", json);
    }

    // 导出护甲数据到 JSON 文件
    public void ExportArmorData()
    {
        JsonData armorData = new JsonData();
        armorData["armorClasses"] = new JsonData();

        foreach (var armor in armorRamDB.armorClasses)
        {
            JsonData armorJson = new JsonData();
            armorJson["id"] = armor.id;
            armorJson["name"] = armor.name;
            armorJson["description"] = armor.description;
            armorJson["type"] = armor.type.ToString();
            armorJson["icon"] = armor.icon != null ? armor.icon.name : null;

            // 处理 RPGattribute
            armorJson["attribute"] = SerializeRPGAttribute(armor.attriute);

            armorData["armorClasses"].Add(armorJson);
        }

        string json = armorData.ToJson();
        File.WriteAllText(Application.dataPath + "/armorData.json", json);
    }

    // 序列化武器词条数据
    private JsonData SerializeWeaponEntry(WeaponsEntryClass entry)
    {
        JsonData entryJson = new JsonData();
        entryJson["id"] = entry.id;
        entryJson["name"] = entry.name;
        entryJson["description"] = entry.description;
        entryJson["isRam"] = entry.isRam.ToString();
        entryJson["uniqueEntry"] = entry.uniqueEntry.ToString();

        entryJson["valueType"] = new JsonData();
        foreach (var value in entry.valueType)
        {
            JsonData valueJson = new JsonData();
            valueJson["ramValueType"] = value.ramValueType.ToString();
            valueJson["realValue"] = value.realValue;
            valueJson["minValue"] = value.minValue;
            valueJson["maxValue"] = value.maxValue;

            entryJson["valueType"].Add(valueJson);
        }

        return entryJson;
    }

    // 序列化 RPGattribute
    private JsonData SerializeRPGAttribute(RPGattribute attr)
    {
        JsonData attrJson = new JsonData();
        attrJson["maxHp"] = attr.maxHp;
        attrJson["maxMp"] = attr.maxMp;
        attrJson["atk"] = attr.atk;
        attrJson["atkSpeed"] = attr.atkSpeed;
        attrJson["crit_Change"] = attr.crit_Change;
        attrJson["crit_Damage"] = attr.crit_Damage;
        attrJson["def"] = attr.def;
        attrJson["relief"] = attr.relief;
        attrJson["luck"] = attr.luck;
        attrJson["moveSpeed"] = attr.moveSpeed;
        attrJson["vampire_Atk"] = attr.vampire_Atk;
        attrJson["recovery_HP"] = attr.recovery_HP;
        attrJson["recovery_Mp"] = attr.recovery_Mp;

        return attrJson;
    }
}
