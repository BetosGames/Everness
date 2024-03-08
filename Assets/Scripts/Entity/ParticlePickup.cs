using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParticlePickup : EntityParticle
{
    public string text;
    public Color outlineColor;

    private TMP_Text textBox;

    public override void Init()
    {
        displayName = "Pickup Particle";
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        textBox = GetComponentInChildren<TMP_Text>();
    }

    public override void WhileExists()
    {
        base.WhileExists();
        textBox.text = text;
        textBox.outlineColor = outlineColor;
    }
}
