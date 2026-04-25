using UnityEngine.UI;

[System.Serializable]
public class RPGattribute
{
    //角色基础属性
    public float maxHp;
    public float maxMp;
    //攻击属性
    public float atk;
    public float atkSpeed;
    public float crit_Change;
    public float crit_Damage;
    //防御属性
    public float def;
    public float relief;
    //其他属性
    public float luck;
    public float moveSpeed;
    public float vampire_Atk;
    public float recovery_HP;
    public float recovery_Mp;
    //函数
    public static RPGattribute operator +(RPGattribute a, RPGattribute b)
    {
        RPGattribute c = new RPGattribute();
        // 基础属性加法
        c.maxHp = a.maxHp + b.maxHp;
        c.maxMp = a.maxMp + b.maxMp;

        // 攻击属性加法
        c.atk = a.atk + b.atk;
        c.atkSpeed = a.atkSpeed + b.atkSpeed;
        c.crit_Change = a.crit_Change + b.crit_Change;
        c.crit_Damage = a.crit_Damage + b.crit_Damage;

        // 防御属性加法
        c.def = a.def + b.def;
        c.relief = a.relief + b.relief;
        // 其他属性加法
        c.luck = a.luck + b.luck;
        c.moveSpeed = a.moveSpeed + b.moveSpeed;
        c.vampire_Atk = a.vampire_Atk + b.vampire_Atk;
        c.recovery_HP = a.recovery_HP + b.recovery_HP;
        c.recovery_Mp = a.recovery_Mp + b.recovery_Mp;

        return c; // 返回加法后的新对象

    }
    public void OutputToUIText(Text uiText, RPGattribute inputAttributes)
    {
        // 更新属性值
        maxHp = inputAttributes.maxHp;
        maxMp = inputAttributes.maxMp;
        atk = inputAttributes.atk;
        atkSpeed = inputAttributes.atkSpeed;
        crit_Change = inputAttributes.crit_Change;
        crit_Damage = inputAttributes.crit_Damage;
        def = inputAttributes.def;
        relief = inputAttributes.relief;
        luck = inputAttributes.luck;
        moveSpeed = inputAttributes.moveSpeed;
        vampire_Atk = inputAttributes.vampire_Atk;
        recovery_HP = inputAttributes.recovery_HP;
        recovery_Mp = inputAttributes.recovery_Mp;

        // 将各个属性拼接为字符串，左侧为简短的中文翻译
        string output = $"最大生命: {maxHp}\n" +
                        $"最大魔法: {maxMp}\n" +
                        $"攻击力: {atk}\n" +
                        $"攻击速度: {atkSpeed}\n" +
                        $"暴击几率: {crit_Change}\n" +
                        $"暴击伤害: {crit_Damage}\n" +
                        $"防御力: {def}\n" +
                        $"抗性: {relief}\n" +
                        $"幸运值: {luck}\n" +
                        $"移动速度: {moveSpeed}\n" +
                        $"吸血攻击: {vampire_Atk}\n" +
                        $"生命恢复: {recovery_HP}\n" +
                        $"魔法恢复: {recovery_Mp}";

        // 设置Text组件的文本内容
        uiText.text = output;
    }
}

