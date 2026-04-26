using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;
    private Enemy enemy;
    private void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        enemy = this.GetComponent<Enemy>();
    }
    private void FixedUpdate()
    {
        anim.SetFloat("velocity", rb2d.velocity.x);
        if (enemy.isDie)
        {
            anim.SetTrigger("OnDie");
        }
        if (enemy.isHurt)
        {
            anim.SetTrigger("OnHurt");
            enemy.isHurt = false;
        }
    }
    public void EnemyAttackAnim()
    {
        anim.SetTrigger("OnAttack");
    }
}
