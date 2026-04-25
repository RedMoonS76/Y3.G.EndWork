using UnityEngine;

public class PickArmor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool isStay = false;
    public ArmorClass armor;
    public ArmorRamDB armorRamDB;
    public PlayerItemScriptableObject playerItem;

    private ShowPanel showPanel;

    private void OnEnable()
    {
        showPanel = FindObjectOfType<ShowPanel>();
        armor = GetRandomArmor();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = armor.icon;
    }

    private void Update()
    {
        if (isStay && Input.GetKeyDown(KeyCode.E))
        {
            ExchangeArmor();
        }
    }

    private void ExchangeArmor()
    {
        // 根据护甲类型获取玩家对应槽位的装备
        ArmorClass playerArmor = GetPlayerArmorByType(armor.type);
        SetPlayerArmorByType(armor.type, this.armor);
        this.armor = playerArmor;

        // 更新显示
        spriteRenderer.sprite = (this.armor != null) ? this.armor.icon : null;

        // 如果地上变成了空装备，销毁物体
        if (this.armor == null || this.armor.type == ArmorType.empty)
        {
            Destroy(gameObject);
            return;
        }

        if (isStay && showPanel != null)
            showPanel.SetDisplayArmor(this.armor);
    }

    private ArmorClass GetPlayerArmorByType(ArmorType type)
    {
        switch (type)
        {
            case ArmorType.head: return playerItem.armorHead;
            case ArmorType.body: return playerItem.armorBody;
            case ArmorType.hand: return playerItem.armorHand;
            case ArmorType.foot: return playerItem.armorFoot;
            default: return null;
        }
    }

    private void SetPlayerArmorByType(ArmorType type, ArmorClass newArmor)
    {
        switch (type)
        {
            case ArmorType.head: playerItem.armorHead = newArmor; break;
            case ArmorType.body: playerItem.armorBody = newArmor; break;
            case ArmorType.hand: playerItem.armorHand = newArmor; break;
            case ArmorType.foot: playerItem.armorFoot = newArmor; break;
        }
    }

    private ArmorClass GetRandomArmor()
    {
        ArmorClass[] allArmors = armorRamDB.armorClasses.ToArray();
        if (allArmors.Length == 0) return null;
        ArmorClass template = allArmors[Random.Range(0, allArmors.Length)];
        return CloneArmor(template);
    }

    private ArmorClass CloneArmor(ArmorClass original)
    {
        if (original == null) return null;
        return new ArmorClass
        {
            id = original.id,
            name = original.name,
            description = original.description,
            type = original.type,
            icon = original.icon,
            attriute = original.attriute   // 如果RPGattribute是class且需要独立副本，请深拷贝
        };
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStay = true;
            if (showPanel != null)
            {
                showPanel.SetDisplayArmor(this.armor);
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
}