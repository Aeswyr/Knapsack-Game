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
    {
        grounded = ground.CheckGrounded();
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
        if (input.dir != 0) {
            sprite.flipX = input.dir < 0;
            facing = input.dir > 0 ? 1 : -1;
        }

        
    }

    void ReadJumps() {
        if (!grounded)
            return;
        if (input.jump.pressed) {
            if (!moveUnlocked)
                Instantiate(shockwave, transform.position, Quaternion.identity);
            velocity = new Vector2((sprinting ? 1.7f : 1) * input.dir * 12, sprinting ? 35 : 50);
            CompleteAction();
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
            BeginAction();
            sprinting = true;
        }
    }

    void ReadActions() {

    }

    void SetHopCancel() {hopCancel = true;}

    void SetActCancel() {actionCancel = true;}

    void SetJumpCancel() {jumpCancel = true;}

    void CompleteAction() {
        animator.SetBool("acting", false);
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

}
