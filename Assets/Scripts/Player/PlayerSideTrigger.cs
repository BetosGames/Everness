using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideTrigger : MonoBehaviour
{
    public enum Side { Bottom, Left, Right }
    public Side side;

    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(side == Side.Bottom)
        {
            if (player.groundLayers.Includes(collision.gameObject.layer)) player.isGrounded = true;
        }
        else if(side == Side.Left)
        {
            if (player.groundLayers.Includes(collision.gameObject.layer)) player.isLeftGrounded = true;
        }
        else if(side == Side.Right)
        {
            if (player.groundLayers.Includes(collision.gameObject.layer)) player.isRightGrounded = true;
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (side == Side.Bottom)
        {
            if (player.groundLayers.Includes(collision.gameObject.layer)) player.isGrounded = false;
        }
        else if (side == Side.Left)
        {
            if (player.groundLayers.Includes(collision.gameObject.layer)) player.isLeftGrounded = false;
        }
        else if (side == Side.Right)
        {
            if (player.groundLayers.Includes(collision.gameObject.layer)) player.isRightGrounded = false;
        }
    }
}
