using UnityEngine;

public class HealInteractable : MonoBehaviour
{
    [Header("恢复设置")]
    public float healAmount = 30f;          // 恢复的生命值

    [Header("激活概率")]
    [Range(0f, 1f)]
    public float activationProbability = 0.5f; // 清空怪物后激活的概率（0~1）

    private bool isAvailable = false;       // 是否已激活（可交互）
    private bool isUsed = false;            // 是否已被使用（防止多次交互）
    private SpriteRenderer spriteRenderer;
    private Collider2D triggerCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<Collider2D>();

        // 初始状态：不可见且不可交互
        spriteRenderer.enabled = false;
        triggerCollider.enabled = false;
    }

    private void Update()
    {
        if (isAvailable || isUsed) return;

        // 检测场景中是否还存在敌人
        if (AreAllEnemiesDead())
        {
            float rand = Random.Range(0f, 1f);
            if (rand <= activationProbability)
            {
                Activate();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private bool AreAllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;
    }

    private void Activate()
    {
        isAvailable = true;
        spriteRenderer.enabled = true;
        triggerCollider.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isAvailable || isUsed) return;
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact(other.gameObject);
            }
        }
    }

    private void Interact(GameObject playerObj)
    {
        if (isUsed) return;

        PlayerController playerCtrl = playerObj.GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            playerCtrl.Heal(healAmount);
        }

        isUsed = true;
        Destroy(gameObject);
    }
}