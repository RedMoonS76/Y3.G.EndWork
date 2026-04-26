using UnityEngine;
using System.Collections;

public class GroundEnemy : Enemy
{
    [Header("地面敌人专属")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;
    public float patrolDistance = 3f;
    public float idleTimeMin = 1f;
    public float idleTimeMax = 3f;
    public float attackCooldown = 1f;
    public float attackDamage = 10f;
    public LayerMask playerLayer;

    private Vector2 startPos;
    private Vector2 targetPos;
    private bool isChasing = false;
    private bool isIdle = false;      // 发呆标志
    private float lastAttackTime = -999f;

    // 新增：控制巡逻流程的协程
    private Coroutine patrolCoroutine;

    private int animIsMoving;
    private int animAttack;
    private int animHurt;
    private int animDeath;

    private Animator groundAnim;
    private SpriteRenderer sprite;

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
        targetPos = startPos + Vector2.right * patrolDistance;

        groundAnim = GetComponent<Animator>();
        if (groundAnim == null) groundAnim = GetComponentInChildren<Animator>();

        animIsMoving = Animator.StringToHash("IsMoving");
        animAttack = Animator.StringToHash("Attack");
        animHurt = Animator.StringToHash("Hurt");
        animDeath = Animator.StringToHash("Death");

        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>();

        // 启动巡逻逻辑
        if (patrolCoroutine == null)
            patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // 如果在追逐或死亡，等待
            while (isChasing || isDie) yield return null;

            // 移动至目标点
            yield return StartCoroutine(MoveToTarget(targetPos));

            // 到达尽头，进入发呆（停止移动，切换动画）
            isIdle = true;
            if (groundAnim != null)
                groundAnim.SetBool(animIsMoving, false);
            rb2d.velocity = Vector2.zero;

            float idleDuration = Random.Range(idleTimeMin, idleTimeMax);
            yield return new WaitForSeconds(idleDuration);

            isIdle = false;

            // 切换巡逻目标（来回）
            if (Mathf.Abs(targetPos.x - startPos.x) < 0.1f)
                targetPos = startPos + Vector2.right * patrolDistance;
            else
                targetPos = startPos;
        }
    }

    private IEnumerator MoveToTarget(Vector2 target)
    {
        // 设置移动中动画
        if (groundAnim != null)
            groundAnim.SetBool(animIsMoving, true);

        while (Vector2.Distance(transform.position, target) > 0.1f)
        {
            // 如果被追逐或死亡，中途停止移动协程
            if (isChasing || isDie) yield break;

            Vector2 direction = (target - (Vector2)transform.position).normalized;
            rb2d.velocity = new Vector2(direction.x * moveSpeed, rb2d.velocity.y);
            UpdateFacing(direction.x);

            yield return null;
        }
        // 精确到达
        rb2d.velocity = Vector2.zero;
        transform.position = target;
    }

    private void FixedUpdate()
    {
        if (isDie) return;

        // 检测玩家（优先级最高，打断巡逻和发呆）
        if (player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distToPlayer <= enemySeekRange && FindPlayer())
            {
                if (!isChasing)
                {
                    isChasing = true;
                    // 打断当前移动协程（如果有）
                    if (patrolCoroutine != null) StopCoroutine(patrolCoroutine);
                    patrolCoroutine = null;
                }
                ChasePlayer();
                return;
            }
            else
            {
                if (isChasing)
                {
                    isChasing = false;
                    // 重新启动巡逻协程
                    if (patrolCoroutine == null)
                        patrolCoroutine = StartCoroutine(PatrolRoutine());
                }
            }
        }

        // 如果不是追逐状态，移动由协程负责，这里不做额外移动（避免冲突）
        if (!isChasing && !isIdle && patrolCoroutine == null)
        {
            patrolCoroutine = StartCoroutine(PatrolRoutine());
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb2d.velocity = new Vector2(direction.x * chaseSpeed, rb2d.velocity.y);
        UpdateFacing(direction.x);

        if (groundAnim != null)
            groundAnim.SetBool(animIsMoving, true);
    }

    private void UpdateFacing(float directionX)
    {
        if (directionX != 0)
        {
            if (sprite != null)
                sprite.flipX = directionX < 0;
            else
                transform.localScale = new Vector3(Mathf.Sign(directionX), 1, 1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDie) return;
        if (collision.gameObject.CompareTag("Player") && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            if (groundAnim != null)
                groundAnim.SetTrigger(animAttack);

            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                Attack attack = new Attack();
                attack.damage = attackDamage;
                (pc as Character)?.TakeDamage(attack);
            }
        }
    }

    public override void TakeDamage(Attack attack)
    {
        base.TakeDamage(attack);
        if (groundAnim != null && !isDie)
            groundAnim.SetTrigger(animHurt);
    }

    public override void ActionDie()
    {
        if (hasDroppedGold) return;
        DropGold();
        hasDroppedGold = true;

        if (groundAnim != null)
            groundAnim.SetTrigger(animDeath);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        rb2d.simulated = false;

        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        if (patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
    }
}