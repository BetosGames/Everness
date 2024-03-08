using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParticleHelper : MonoBehaviour
{
    public void EndParticle()
    {
        GetComponentInParent<EntityParticle>().RemoveFromWorld();
    }
}
