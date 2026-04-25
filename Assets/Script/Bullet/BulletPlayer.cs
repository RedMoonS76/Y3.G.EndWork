using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool isHit;
    private AudioManager audioManager;

    public float BulletSpeed;
    private void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void Update()
    {

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
        if (collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
            isHit = true;
            audioManager.PlayEffect("playHurt");
        }
        if (collision.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);

        }
        else
        {
            isHit = false;
        }
    }
}
