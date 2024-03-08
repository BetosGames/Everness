using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPickup : Entity
{
    private Rigidbody rb;
    [HideInInspector]
    public Item assignedItem;

    public override void Init()
    {
        displayName = "Pickup";
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        rb = GetComponent<Rigidbody>();

        GetComponentInChildren<SpriteRenderer>().sprite = assignedItem == null ? Sprite.Create(FileManager.MissingTexture(), new Rect(0, 0, FileManager.MissingTexture().width, FileManager.MissingTexture().height), new Vector2(0.5f, 0.5f), 12) : assignedItem.sprite;
    }

    public override void WhileExists()
    {
        base.WhileExists();
        rb.velocity += Vector3.up * Physics.gravity.y * 2 * Time.deltaTime;
    }

    public virtual void OnCollect(Player player)
    {
        if(assignedItem != null)
        {
            assignedItem.OnItemPickup(player);
        }

        ParticlePickup newParticlePickup = (ParticlePickup) planet.SpawnEntity("particle_pickup", GetCoordinates());
        newParticlePickup.text = assignedItem == null ? "Missing" : $"+{assignedItem.displayName}";

        RemoveFromWorld();
    }
}
