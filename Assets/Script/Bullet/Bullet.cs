using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Enemy enemy;
    private Rigidbody2D rb2d;
    private bool isHit;
    private AudioManager audioManager;

    public float BulletSpeed;
    private void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        BulletFly();
        Destroy(this, 10);
        Debug.DrawLine(this.transform.position, this.transform.position + transform.right * 5, Color.red);
    }
    void BulletFly()
    {
        int conSpeed;
        if (isHit) { conSpeed = 0; } else { conSpeed = 1; }
        rb2d.velocity = transform.right * BulletSpeed * conSpeed;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {

            Destroy(this.gameObject, 0.1f);
            isHit = true;
            audioManager.PlayEffect("playHurt");
        }
        if (collision.CompareTag("Bullet"))
        {
            Destroy(this.gameObject, 0.1f);
            audioManager.PlayEffect("playHurt");
        }
        else
        {
            isHit = false;
        }
    }
}
