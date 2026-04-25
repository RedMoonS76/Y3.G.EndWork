using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;
    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        physicsCheck = this.GetComponent<PhysicsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Velocity", Math.Abs(rb.velocity.x));
        anim.SetBool("IsGround", physicsCheck.isGround);
    }
    public void HurtAnim()
    {
        anim.SetTrigger("IsHurt");
    }
}
