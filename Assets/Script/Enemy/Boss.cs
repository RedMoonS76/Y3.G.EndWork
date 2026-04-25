using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Character
{
    [HideInInspector] public Rigidbody2D rb2d;            // Rigidbody2D 组件
    [HideInInspector] public Animator anim;               // 动画组件
    [HideInInspector] public PhysicsCheck physicsCheck;   // 物理检测工具
    public GameObject bullet;                              // 子弹对象预制体
    public GameObject shootPos;                            // 射击位置对象
    private BossBaseState b_inNomAttack;                   // Boss 普通攻击状态
    private BossBaseState b_inSkillA;                      // Boss 技能 A 状态
    private BossBaseState b_inSkillB;                      // Boss 技能 B 状态

    [HideInInspector] public Vector3 playerCenterPoint;    // 玩家中心点位置

    [HideInInspector] public BossBaseState currentState;   // Boss 当前状态
    [HideInInspector] public GameObject player;            // 玩家对象
    [Header("boss通用变量")]
    public float fireInterval;   // 开火间隔
    public float fireTimesInOne; // 攻击次数
    public float fireCoolDownTime; //攻击冷却时间
    public float bossThinkTime;  // Boss 思考时间
    public int bossDamage;       // Boss 伤害

    [Header("BossInNom变量")]
    public float KeepRange;       // Boss 与玩家保持的距离
    public float NomSpeed;        // Boss'nomState移动速度
    public Vector3 keepOffset;    // 用于保持位置的偏移量
    [Header("BossSkillA变量")]
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float SkillASpeed;
    public float SkillAtime_a_t;
    public float SkillAtime_b_t;
    private void Awake()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        b_inNomAttack = new BossInNomAttack();
        b_inSkillA = new BossInSkillA();
        b_inSkillB = new BossInSkillB();

        BossSwitchState(bossState.NomAttack);

    }
    private void OnEnable()
    {
        currentState.OnEnter(this);
    }
    private void Update()
    {
        playerCenterPoint = new Vector3(player.transform.position.x, player.transform.position.y + player.transform.localScale.y / 2, player.transform.position.z);
        currentState.LogicUpdate();
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter < 0)
            {
                invulnerable = false;
            }
        }
    }
    public void FixedUpdate()
    {
        currentState.PhysicsUpdate();
        AutoTurn();
    }
    public void OnDisable()
    {
        currentState.OnExit();
    }
    public void BossSwitchState(bossState t_boosState)
    {
        switch (t_boosState)
        {
            case bossState.NomAttack: currentState = b_inNomAttack; break;
            case bossState.SkillA: currentState = b_inSkillA; break;
            case bossState.SkillB: currentState = b_inSkillB; break;
            default: currentState = b_inNomAttack; break;
        }
        currentState.OnEnter(this);
        Debug.Log(t_boosState);
    }
    public void Test()
    {
        BossSwitchState(bossState.SkillA);
    }
    public void DieMethod()
    {
        SceneManager.LoadScene("Win");
    }
    #region 通用方法

    public bool FacePlayer()
    {
        if ((this.gameObject.transform.position.x >= player.transform.position.x && this.gameObject.transform.localScale.x > 0) || (this.gameObject.transform.position.x <= player.transform.position.x && this.gameObject.transform.localScale.x < 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool EnemyWithPlayer()
    {
        if (this.gameObject.transform.position.x >= player.transform.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AutoTurn()
    {
        if (!FacePlayer())
        {
            TurnAround();
        }
    }
    public void TurnAround()
    {
        float s = this.GetComponentInParent<Transform>().localScale.x;
        this.GetComponentInParent<Transform>().localScale = new Vector2(-s, this.GetComponentInParent<Transform>().localScale.y);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(startPoint, 0.5f);
        Gizmos.DrawWireSphere(endPoint, 0.5f);
    }
    #endregion
    #region nom方法
    public void KeepWithPlayer()
    {
        Vector3 keepPointPos = playerCenterPoint + keepOffset;
        if (Vector2.Distance(this.transform.position, keepPointPos) >= KeepRange)
        {

            Vector2 direction = (keepPointPos - this.transform.position).normalized;
            rb2d.velocity = direction * NomSpeed * Vector2.Distance(this.transform.position, keepPointPos);
        }
        else
        {
            this.rb2d.velocity = Vector2.zero;
        }
        Debug.DrawLine(this.transform.position, playerCenterPoint);
    }
    private Coroutine shootCoroutine;
    public void ShootByTimes()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
        shootCoroutine = StartCoroutine("BossAttack");
    }
    public void ShootPlayer(GameObject bullet, GameObject shootPos)
    {
        Instantiate(bullet, shootPos.transform.position, shootPos.transform.rotation);
    }
    IEnumerator BossAttack()
    {
        int t = 0;
        while (t <= fireTimesInOne - 1)
        {
            ShootPlayer(bullet, shootPos);
            yield return new WaitForSeconds(fireInterval);
            t++;
        }
        yield return new WaitForSeconds(fireCoolDownTime);
        shootCoroutine = null;
    }
    #endregion
    #region BossSkillA
    private Coroutine bossSkillA;
    public void BossSkillA()
    {
        if (bossSkillA != null)
        {
            StopCoroutine(bossSkillA);
        }
        bossSkillA = StartCoroutine("bossSprint");
    }
    bool SkillAOnStart = false;
    public bool SkillAOnEnd = false;
    float SkillAtime_a_c;
    float SkillAtime_b_c;
    public void bossStartSkillA()
    {
        SkillAOnStart = false;
        SkillAOnEnd = false;
        SkillAtime_a_c = 0;
        SkillAtime_b_c = 0;
    }
    public void bossSprint()
    {
        if (!SkillAOnStart)
        {
            if (Vector3.Distance(this.gameObject.transform.position, startPoint) > 0.5 || SkillAtime_a_c <= SkillAtime_a_t)
            {
                Vector3 t = startPoint - this.gameObject.transform.position;
                t = t.normalized;

                rb2d.velocity = t * SkillASpeed * Vector3.Distance(this.gameObject.transform.position, startPoint);

                SkillAtime_a_c += Time.deltaTime;
            }
            else if (Vector3.Distance(this.gameObject.transform.position, startPoint) <= 0.5 || SkillAtime_a_c >= SkillAtime_a_t)
            {
                rb2d.velocity = Vector3.zero;
                SkillAOnStart = true;
            }
        }
        if (!SkillAOnEnd && SkillAOnStart)
        {
            if (Vector3.Distance(this.gameObject.transform.position, endPoint) > 0.5 || SkillAtime_b_c <= SkillAtime_b_t)
            {
                Vector3 t = endPoint - this.gameObject.transform.position;
                t = t.normalized;
                rb2d.velocity = t * SkillASpeed * Vector3.Distance(this.gameObject.transform.position, endPoint);
                SkillAtime_b_c += Time.deltaTime;
            }
            else if (Vector3.Distance(this.gameObject.transform.position, endPoint) <= 0.5 || SkillAtime_b_c >= SkillAtime_b_t)
            {
                SkillAOnEnd = true;
            }
        }
    }
    #endregion
}

public enum bossState
{
    NomAttack,
    SkillA,
    SkillB
}

