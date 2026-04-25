using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("×´Ì¬")]
    public bool isDie;
    public bool isHurt;

    [Header("»ù´¡ÊôÐÔ")]
    public float maxHp;
    public float Hp;
    public float maxMp;
    public float Mp;

    public float invulnerableDuration;
    [HideInInspector] public float invulnerableCounter;
    [HideInInspector] public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Character> OnManaChange;   
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    protected virtual void Start()
    {
        Hp = maxHp;
        Mp = maxMp;
        OnHealthChange?.Invoke(this);
        OnManaChange?.Invoke(this);
    }

    protected virtual void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
                invulnerable = false;
        }
    }

    public virtual void TakeDamage(Attack attack)
    {
        if (invulnerable) return;
        if (attack.damage < Hp)
        {
            Hp -= attack.damage;
            OnTakeDamage?.Invoke(attack.transform);
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
        else
        {
            Hp = 0;
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(this);
    }

    public virtual bool ConsumeMp(float amount)
    {
        if (Mp >= amount)
        {
            Mp -= amount;
            OnManaChange?.Invoke(this);
            return true;
        }
        return false;
    }

    public virtual void RestoreMp(float amount)
    {
        Mp = Mathf.Min(maxMp, Mp + amount);
        OnManaChange?.Invoke(this);
    }
}