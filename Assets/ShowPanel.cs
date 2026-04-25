using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    public Text itemName;
    public Text itemDescription;
    public Text itemEntry_a;
    public Text itemEntry_b;
    public Text itemEntry_c;
    public Text armorsAttriuteDisplay;

    public GameObject Panel;

    private WeaponClass currentWeapon;
    private ArmorClass currentArmor;

    private void Start()
    {
        CloseUi();
    }

    private void Update()
    {
        DisplayItemDescription();
        UpdateAttributesDisplay();
    }

    // --- 对外接口 ---
    public void SetDisplayWeapon(WeaponClass weapon)
    {
        currentWeapon = weapon;
        currentArmor = null;
    }

    public void SetDisplayArmor(ArmorClass armor)
    {
        currentArmor = armor;
        currentWeapon = null;
    }

    public void ClearDisplay()
    {
        currentWeapon = null;
        currentArmor = null;
        itemName.text = "";
        itemDescription.text = "";
        itemEntry_a.text = "";
        itemEntry_b.text = "";
        itemEntry_c.text = "";
        armorsAttriuteDisplay.text = "";
    }

    public void OpenUi()
    {
        Panel.SetActive(true);
    }

    public void CloseUi()
    {
        Panel.SetActive(false);
    }

    // --- 内部显示逻辑 ---
    private void DisplayItemDescription()
    {
        if (currentWeapon != null)
        {
            itemName.text = currentWeapon.name;
            switch (currentWeapon.weaponQ)
            {
                case WeaponQ.Nom: itemName.color = Color.white; break;
                case WeaponQ.High: itemName.color = Color.blue; break;
                case WeaponQ.Unique: itemName.color = Color.red; break;
                default: itemName.color = Color.white; break;
            }
            itemDescription.text = currentWeapon.description;

            itemEntry_a.text = FormatWeaponEntry(currentWeapon.weaponsEntry_a);
            itemEntry_b.text = FormatWeaponEntry(currentWeapon.weaponsEntry_b);
            itemEntry_c.text = FormatWeaponEntry(currentWeapon.weaponsEntry_c);

            armorsAttriuteDisplay.text = "";
        }
        else if (currentArmor != null)
        {
            itemName.text = currentArmor.name;
            itemName.color = Color.white;
            itemDescription.text = currentArmor.description;

            itemEntry_a.text = "";
            itemEntry_b.text = "";
            itemEntry_c.text = "";
        }
        else
        {
            ClearDisplay();
        }
    }

    private void UpdateAttributesDisplay()
    {
        if (currentArmor == null)
        {
            armorsAttriuteDisplay.text = "";
            return;
        }

        RPGattribute attr = currentArmor.attriute;
        string displayText = "";

        if (Mathf.Abs(attr.maxHp) > 0) displayText += $"最大生命: {attr.maxHp}\n";
        if (Mathf.Abs(attr.maxMp) > 0) displayText += $"最大魔法: {attr.maxMp}\n";
        if (Mathf.Abs(attr.atk) > 0) displayText += $"攻击力: {attr.atk}\n";
        if (Mathf.Abs(attr.atkSpeed) > 0) displayText += $"攻击速度: {attr.atkSpeed}\n";
        if (Mathf.Abs(attr.crit_Change) > 0) displayText += $"暴击率: {attr.crit_Change}\n";
        if (Mathf.Abs(attr.crit_Damage) > 0) displayText += $"暴击伤害: {attr.crit_Damage}\n";
        if (Mathf.Abs(attr.def) > 0) displayText += $"防御力: {attr.def}\n";
        if (Mathf.Abs(attr.relief) > 0) displayText += $"减伤率: {attr.relief}\n";
        if (Mathf.Abs(attr.luck) > 0) displayText += $"幸运值: {attr.luck}\n";
        if (Mathf.Abs(attr.moveSpeed) > 0) displayText += $"移动速度: {attr.moveSpeed}\n";
        if (Mathf.Abs(attr.vampire_Atk) > 0) displayText += $"吸血攻击: {attr.vampire_Atk}\n";
        if (Mathf.Abs(attr.recovery_HP) > 0) displayText += $"生命恢复: {attr.recovery_HP}\n";
        if (Mathf.Abs(attr.recovery_Mp) > 0) displayText += $"魔法恢复: {attr.recovery_Mp}\n";

        armorsAttriuteDisplay.text = displayText;
    }

    private string FormatWeaponEntry(WeaponsEntryClass entry)
    {
        if (entry == null) return "";
        string valueText = WeaponsEntryDisplay(entry);
        return $"{entry.name}: {entry.description}\n{valueText}";
    }

    private string WeaponsEntryDisplay(WeaponsEntryClass entry)
    {
        if (entry == null || entry.valueType == null) return "";
        var mapping = CreateEnumToStringMapping();
        string text = "";
        foreach (var vt in entry.valueType)
        {
            if (mapping.TryGetValue(vt.ramValueType, out string cn))
                text += $"{cn}: {vt.realValue}\n";
        }
        return text;
    }

    private Dictionary<ramValueType, string> CreateEnumToStringMapping()
    {
        return new Dictionary<ramValueType, string>
        {
            { ramValueType.MaxHp, "最大生命值" },
            { ramValueType.MaxMp, "最大魔法值" },
            { ramValueType.Atk, "攻击力" },
            { ramValueType.AtkSpeed, "攻击速度" },
            { ramValueType.CritChange, "暴击几率" },
            { ramValueType.CritDamage, "暴击伤害" },
            { ramValueType.Def, "防御力" },
            { ramValueType.Luck, "幸运值" },
            { ramValueType.MoveSpeed, "移动速度" },
            { ramValueType.VampireAtk, "吸血攻击" },
            { ramValueType.RecoveryHp, "生命恢复" },
            { ramValueType.RecoveryMp, "魔法恢复" }
        };
    }
}