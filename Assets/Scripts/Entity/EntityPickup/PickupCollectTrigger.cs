using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollectTrigger : MonoBehaviour
{

    private bool collidedWithPlayer = false;

    private void OnTriggerStay(Collider other)
    {
        if (!collidedWithPlayer)
        {
            Player targetPlayer = other.GetComponent<Player>();
            if (targetPlayer != null)
            {
                GetComponentInParent<EntityPickup>().OnCollect(targetPlayer);
                collidedWithPlayer = true;
            }
        }
    }
}
