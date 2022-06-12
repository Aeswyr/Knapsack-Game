using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour
{
    [SerializeField] private AnimationCurve[] arcs;
    [SerializeField] private float speed;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private Rigidbody2D rbody;
    private bool jumping, frame1;
    private int curveIndex;
    private float timer;
    public void StartJump(int type) {
        jumping = true;
        if (type >= arcs.Length)
            Debug.LogError("Attempting to use undefined jump arc");
        curveIndex = type;
        timer = Time.time;
        frame1 = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float time = Time.time - timer;
        if (!frame1)
            jumping = jumping && !ground.CheckGrounded() && time <= arcs[curveIndex].keys[arcs[curveIndex].length-1].time;
        if (jumping) {
            Vector2 velocity = rbody.velocity;
            velocity.y = speed * arcs[curveIndex].Evaluate(Time.time - timer);
            rbody.velocity = velocity;
            frame1 = false;
        }
    }
}
