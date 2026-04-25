using UnityEngine;

public class GroundEnemy : Character
{
    [Header("移动设置")]
    public float patrolSpeed = 1f;      // 巡逻速度
    public float chaseSpeed = 3f;       // 追逐速度
    public float patrolLeftX = -5f;     // 巡逻左边界（相对起始点）
    public float patrolRightX = 5f;     // 巡逻右边界
    public float detectRange = 5f;      // 发现玩家的距离

    [Header("攻击设置")]
    public float attackCooldown = 1f;   // 攻击间隔
    public float attackDamage = 10f;    // 每次碰撞造成的伤害

    [Header("组件")]
    public Transform player;            // 玩家Transform（自动查找）
    public Animator animator;

    private Rigidbody2D rb;
    private float originalScaleX;       // 记录原始朝向
    private bool facingRight = true;    // 当前朝向
    private bool isChasing = false;     // 是否在追逐状态
    private float lastAttackTime = -999f;
    private bool isDead = false;        // 是否死亡

    // 边界记录
    private float startX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startX = transform.position.x;
        originalScaleX = transform.localScale.x;

        // 初始化属性
        maxHp = 30f;  // 可根据需求在Inspector中调整
        Hp = maxHp;
    }

    void Update()
    {
        if (isDead) return;

        // 检测玩家是否在范围内且存活
        bool playerInRange = player != null && Vector2.Distance(transform.position, player.position) <= detectRange;

        if (playerInRange)
        {
            // 追逐玩家
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            // 巡逻
            isChasing = false;
            Patrol();
        }

        // 更新动画
        UpdateAnimations();
    }

    void Patrol()
    {
        // 边界切换方向
        if (transform.position.x <= startX + patrolLeftX)
        {
            facingRight = true;
            Flip();
        }
        else if (transform.position.x >= startX + patrolRightX)
        {
            facingRight = false;
            Flip();
        }

        // 移动
        float move = facingRight ? patrolSpeed : -patrolSpeed;
        rb.velocity = new Vector2(move, rb.velocity.y);
    }

    void ChasePlayer()
    {
        if (player == null) return;

        // 转向玩家
        if (player.position.x > transform.position.x && !facingRight)
        {
            facingRight = true;
            Flip();
        }
        else if (player.position.x < transform.position.x && facingRight)
        {
            facingRight = false;
            Flip();
        }

        // 移动靠近玩家
        float direction = (player.position.x - transform.position.x) > 0 ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? originalScaleX : -originalScaleX;
        transform.localScale = scale;
    }

    void UpdateAnimations()
    {
        if (isDead)
        {
            animator.SetTrigger("Death");
            return;
        }

        // 根据速度判断移动状态
        float speed = Mathf.Abs(rb.velocity.x);
        if (isChasing && speed > 0.1f)
        {
            animator.SetTrigger("Run");
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsWalking", false);
        }
        else if (!isChasing && speed > 0.1f)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            animator.SetTrigger("Idle");
        }
    }

    // 碰撞伤害（由于需要玩家接触敌人，使用 OnCollisionEnter2D 或 OnTriggerEnter2D）
    // 建议敌人的 Collider2D 为触发器（Is Trigger = true），以下使用 OnTriggerEnter2D。
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            TryAttackPlayer(other.GetComponent<Character>());
        }
    }

    // 如果使用非触发器碰撞，可以用 OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttackPlayer(collision.gameObject.GetComponent<Character>());
        }
    }

    void TryAttackPlayer(Character playerChar)
    {
        if (playerChar == null) return;
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            // 播放攻击动画
            animator.SetTrigger("Attack");
            // 造成伤害
            Attack attack = new Attack();
            attack.damage = attackDamage;
            playerChar.TakeDamage(attack);
            // 可选：击退玩家
            Rigidbody2D playerRb = playerChar.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockback = (playerChar.transform.position - transform.position).normalized;
                playerRb.AddForce(knockback * 5f, ForceMode2D.Impulse);
            }
        }
    }

    // 受击（继承自 Character，但需要额外中断行为）
    public override void TakeDamage(Attack attack)
    {
        if (isDead) return;
        base.TakeDamage(attack); // 减少血量
        animator.SetTrigger("Hurt");
        if (Hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
        // 死亡掉落金币（可选）
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.ChangeGold(10);
        }
        Destroy(gameObject, 1f);
    }

    // 在Inspector中可视化巡逻范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(startX + (patrolLeftX + patrolRightX) / 2, transform.position.y, 0),
                            new Vector3(patrolRightX - patrolLeftX, 0.5f, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}