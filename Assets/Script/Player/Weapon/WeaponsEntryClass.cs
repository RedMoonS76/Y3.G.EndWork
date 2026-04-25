public enum entryType
{
    random,
    definite,
    unique
}
public enum ramValueType
{
    MaxHp,
    MaxMp,
    Atk,
    AtkSpeed,
    CritChange,
    CritDamage,
    Def,
    Luck,
    MoveSpeed,
    VampireAtk,
    RecoveryHp,
    RecoveryMp
}
public enum uniqueEntry
{
    Null,
    mustCrit,
    poison
}

[System.Serializable]
public class WeaponsEntryClass
{
    public int id;
    public string name;
    public string description;
    public entryType isRam;
    public uniqueEntry uniqueEntry;
    public ramTypeClass[] valueType;
    public RPGattribute weapWords;

}
[System.Serializable]
public class ramTypeClass
{
    public ramValueType ramValueType;
    public float realValue;
    public float minValue;
    public float maxValue;
}