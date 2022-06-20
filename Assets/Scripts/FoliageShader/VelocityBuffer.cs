using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityBuffer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    private static readonly float maxVelocity = 32;

    private void FixedUpdate() {
        Vector4 color = rbody.velocity;
        if (color.x > 0) {
            color.x = (Mathf.Min(color.x, maxVelocity) + maxVelocity) / (maxVelocity * 2);
        } else {
            color.x = (Mathf.Max(color.x, -maxVelocity) + maxVelocity) / (maxVelocity * 2);
        }

        if (color.y > 0) {
            color.y = (Mathf.Min(color.y, maxVelocity) + maxVelocity) / (maxVelocity * 2);
        } else {
            color.y = (Mathf.Max(color.y, -maxVelocity) + maxVelocity) / (maxVelocity * 2);
        }

        color.w = 1;
        sprite.color = color;
    }
}
