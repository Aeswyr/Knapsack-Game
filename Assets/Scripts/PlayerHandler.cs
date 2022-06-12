using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private InputHandler input;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private GameObject shockwave;
    [SerializeField] private JumpHandler jumps;

    // internals
    private bool moveUnlocked = true, jumpCancel = true, hopCancel = true, actionCancel = true;
    private bool sprinting = false;
    private int facing = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    Vector2 velocity;
    bool grounded = false;
    void FixedUpdate()
    {   bool lastGround = grounded;
        grounded = ground.CheckGrounded();
        animator.SetBool("grounded", grounded);
        if (!lastGround && grounded)
            animator.SetTrigger("DoLand");
        velocity = rbody.velocity;

        if (moveUnlocked) {
            ReadMovement();
        }
        if (jumpCancel) {
            ReadJumps();
        }
        if (hopCancel) {
            ReadHops();
        }
        if (actionCancel) {
            ReadActions();
        }
        
        rbody.velocity = velocity;
    }

    void ReadMovement() {
        animator.SetBool("running", input.dir != 0 && moveUnlocked && grounded);
        sprinting = sprinting && input.move.down && input.dash.down;
        animator.SetBool("sprinting", sprinting);

        if (!grounded)
            return;

        velocity = new Vector2((sprinting ? 1.7f : 1) * input.dir * 12, rbody.velocity.y);
        UpdateFacing();
    }

    void ReadJumps() {
        if (!grounded)
            return;
        if (input.jump.pressed) {
            
            if (!moveUnlocked)
                Instantiate(shockwave, transform.position, Quaternion.identity);
            velocity = new Vector2((sprinting ? 1.7f : 1) * input.dir * 12, rbody.velocity.y);
            jumps.StartJump(sprinting ? 1 : 0);
            CompleteAction();
            UpdateFacing();
            animator.SetBool("acting", true);
            animator.SetTrigger("DoJump");
        }
    }

    void ReadHops() {
        if (!grounded)
            return;
        if (input.dash.pressed) {
            if (!moveUnlocked)
                Instantiate(shockwave, transform.position, Quaternion.identity);
            if (input.dir == 0) {
                velocity = new Vector2(-25 * facing, 0);
                animator.SetTrigger("DoBackhop");
            }
            else {
                velocity = new Vector2(30 * input.dir, 0);
                animator.SetTrigger("DoHop");
            }
            animator.SetBool("attacking", false);
            BeginAction();
            sprinting = true;
        }
    }

    private void UpdateFacing() {
        if (input.dir != 0) {
            sprite.flipX = input.dir < 0;
            facing = input.dir > 0 ? 1 : -1;
        }
    }
    void ReadActions() {
        if (input.primary.pressed || input.secondary.pressed) {
            if (!moveUnlocked)
                Instantiate(shockwave, transform.position, Quaternion.identity);
            animator.SetTrigger("DoAction");
            BeginAction();
        }
    }

    void SetHopCancel() {hopCancel = true;}

    void SetActCancel() {actionCancel = true;}

    void SetJumpCancel() {jumpCancel = true;}

    void CompleteAction() {
        animator.SetBool("acting", false);
        animator.SetBool("attacking", false);
        moveUnlocked = true;
        hopCancel = true;
        jumpCancel = true;
        actionCancel = true;
    }

    void BeginAction() {
        animator.SetBool("acting", true);
        sprinting = false;
        moveUnlocked = false;
        hopCancel = false;
        jumpCancel = false;
        actionCancel = false;
    }

    void BeginAttack() {
        animator.SetBool("attacking", true);
    }

    void SetSpeed(float speed) {
        Vector2 velocity = rbody.velocity;
        velocity.x = facing * Mathf.Max(speed, Mathf.Abs(velocity.x));
        rbody.velocity = velocity;
    }

}
