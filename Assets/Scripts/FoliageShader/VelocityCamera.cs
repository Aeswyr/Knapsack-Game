using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCamera : MonoBehaviour
{
    [SerializeField] private Material[] velocityShaders;
    void Update()
    {
        foreach (var shader in velocityShaders)
            shader.SetFloat("_TimeDelta", Time.deltaTime);
    }
}
