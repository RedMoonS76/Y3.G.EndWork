using UnityEngine;

public class PickWeapon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool isStay = false;
    public WeaponClass weapon;
    public WeaponRamDB weaponRamDB;
    public WeaponEntryDB entryDB;
    public PlayerItemScriptableObject playerItem;

    private ShowPanel showPanel;

    private void OnEnable()
    {
        showPanel = FindObjectOfType<ShowPanel>();
        weapon = RandomWeaponEntry(weaponRamDB, entryDB);
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.icon;
    }

    private void Update()
    {
        if (isStay && Input.GetKeyDown(KeyCode.E))
        {
            ExchangeWeapon();
        }
    }

    private void ExchangeWeapon()
    {
        // 交换玩家武器和地上武器
        WeaponClass temp = playerItem.weapon;
        playerItem.weapon = this.weapon;
        this.weapon = temp;

        // 更新显示
        spriteRenderer.sprite = (this.weapon != null) ? this.weapon.icon : null;

        // 如果地上武器为空，销毁物体
        if (this.weapon == null || this.weapon.weaponQ == WeaponQ.empty)
        {
            Destroy(gameObject);
            return;
        }

        // 更新显示面板（如果玩家还在范围内）
        if (isStay && showPanel != null)
        {
            showPanel.SetDisplayWeapon(this.weapon);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStay = true;
            if (showPanel != null)
            {
                showPanel.SetDisplayWeapon(this.weapon);
                showPanel.OpenUi();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStay = false;
            if (showPanel != null)
            {
                showPanel.CloseUi();
                showPanel.ClearDisplay();
            }
        }
    }

    // --- 随机武器生成（原有逻辑，拷贝过来）---
    private WeaponClass RandomWeaponEntry(WeaponRamDB weaponRamDB, WeaponEntryDB weaponEntrys)
    {
        WeaponClass weapon = new WeaponClass();
        WeaponEntryDB weaponEntryDB = weaponEntrys;
        WeaponClass[] WeaponClasses = weaponRamDB.weaponClasses.ToArray();

        int temp = Random.Range(0, WeaponClasses.Length);
        WeaponClass selectedWeapon = WeaponClasses[temp];
        weapon = CloneWeaponClass(selectedWeapon);

        WeaponsEntryClass weaponEntry_a = new WeaponsEntryClass();
        WeaponsEntryClass weaponEntry_b = new WeaponsEntryClass();
        WeaponsEntryClass weaponEntry_c = new WeaponsEntryClass();

        WeaponsEntryClass[] weaponsEntries = weaponEntryDB.weaponsEntries.ToArray();

        weaponEntry_a = GetUniqueRandomWeaponEntry(weaponsEntries, new System.Collections.Generic.List<WeaponsEntryClass>());
        weaponEntry_b = GetUniqueRandomWeaponEntry(weaponsEntries, new System.Collections.Generic.List<WeaponsEntryClass> { weaponEntry_a });
        weaponEntry_c = GetUniqueRandomWeaponEntry(weaponsEntries, new System.Collections.Generic.List<WeaponsEntryClass> { weaponEntry_a, weaponEntry_b });

        ApplyRandomValues(weaponEntry_a);
        ApplyRandomValues(weaponEntry_b);
        ApplyRandomValues(weaponEntry_c);

        weapon.weaponsEntry_a = weaponEntry_a;
        weapon.weaponsEntry_b = weaponEntry_b;
        weapon.weaponsEntry_c = weaponEntry_c;

        return weapon;
    }

    private WeaponClass CloneWeaponClass(WeaponClass original)
    {
        WeaponClass clone = new WeaponClass();
        clone.id = original.id;
        clone.name = original.name;
        clone.weaponModel = original.weaponModel;
        clone.description = original.description;
        clone.weaponQ = original.weaponQ;
        clone.damage = original.damage;
        clone.icon = original.icon;
        clone.weaponsEntry_a = CloneWeaponEntry(original.weaponsEntry_a);
        clone.weaponsEntry_b = CloneWeaponEntry(original.weaponsEntry_b);
        clone.weaponsEntry_c = CloneWeaponEntry(original.weaponsEntry_c);
        return clone;
    }

    private WeaponsEntryClass CloneWeaponEntry(WeaponsEntryClass original)
    {
        if (original == null) return null;
        WeaponsEntryClass clone = new WeaponsEntryClass();
        clone.id = original.id;
        clone.name = original.name;
        clone.description = original.description;
        clone.isRam = original.isRam;
        clone.uniqueEntry = original.uniqueEntry;
        clone.valueType = new ramTypeClass[original.valueType.Length];
        for (int i = 0; i < original.valueType.Length; i++)
        {
            clone.valueType[i] = new ramTypeClass()
            {
                ramValueType = original.valueType[i].ramValueType,
                realValue = original.valueType[i].realValue,
                minValue = original.valueType[i].minValue,
                maxValue = original.valueType[i].maxValue
            };
        }
        clone.weapWords = original.weapWords;
        return clone;
    }

    private WeaponsEntryClass GetUniqueRandomWeaponEntry(WeaponsEntryClass[] weaponsEntries, System.Collections.Generic.List<WeaponsEntryClass> excludedEntries)
    {
        WeaponsEntryClass randomEntry;
        do
        {
            int randomIndex = Random.Range(0, weaponsEntries.Length);
            randomEntry = CloneWeaponEntry(weaponsEntries[randomIndex]);
        } while (excludedEntries.Contains(randomEntry));
        return randomEntry;
    }

    private void ApplyRandomValues(WeaponsEntryClass weaponEntry)
    {
        if (weaponEntry.isRam == entryType.random)
        {
            for (int i = 0; i < weaponEntry.valueType.Length; i++)
            {
                weaponEntry.valueType[i].realValue = Random.Range(weaponEntry.valueType[i].minValue, weaponEntry.valueType[i].maxValue);
            }
        }
    }
}