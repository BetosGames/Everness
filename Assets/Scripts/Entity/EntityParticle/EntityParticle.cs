using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParticle : Entity
{
    public Transform followTransform;
    public float followSpeed;

    public override void Init()
    {
        displayName = "Unnamed Particle";
    }

    public override void WhileExists()
    {
        base.WhileExists();
        if(followTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, Time.deltaTime * followSpeed);
        }
    }
}
