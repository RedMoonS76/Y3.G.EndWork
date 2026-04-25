using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    public PlayerController player;
    private void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (player.isAttack)
            {
                Debug.Log("敌人受击");
                damage = AttackDamageSum(player.rpg_Sum, other.GetComponent<Enemy>().enemyRpgAttribute);
                Debug.Log("受击：" + damage);
                other.GetComponent<Character>()?.TakeDamage(this);
            }
        }
        if (other.tag == "Player")
        {
            Debug.Log("玩家受击");
            damage = AttackDamageSum(this.GetComponent<Bullet>().enemy.enemyRpgAttribute, other.GetComponent<PlayerController>().rpg_Sum);
            Debug.Log("受击：" + damage);
            other.GetComponent<Character>()?.TakeDamage(this);
        }
    }
    public float AttackDamageSum(RPGattribute attacker, RPGattribute taker)
    {
        float endDamge;
        float temp = (float)(attacker.atk * ((Random.Range(0f, 1f) < attacker.crit_Change) ? attacker.crit_Damage : 1));
        endDamge = (temp - taker.def) * (1 - taker.relief);
        if (endDamge < 0) endDamge = 0;
        return endDamge;
    }
}
