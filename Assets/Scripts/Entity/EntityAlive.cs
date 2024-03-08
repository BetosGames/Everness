using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityAlive : Entity
{
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public Vector2 healthbarOffset = Vector2.zero;
    [HideInInspector]
    public float healthbarScale = 1;
    private int health;
    private bool isAlive = true;
    private float healthBarShowTimer;
    private Quaternion frozenHealthBarRotation;

    public Transform healthbarHolder;
    public Image healthFill;
    public EntityParticle deathParticle;

    public const float healthBarShowtime = 2;

    public override void OnSpawn()
    {
        base.OnSpawn();
        health = maxHealth;
        frozenHealthBarRotation = healthbarHolder.rotation;
    }

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int amount)
    {
        if(health - amount <= 0)
        {
            health = 0;
            Kill();
        }
        else
        {
            health -= amount;
        }

        ShowHealthbar();
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public override void WhileExists()
    {
        base.WhileExists();
        if (IsAlive())
        {
            if (healthFill != null) healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, (float)health / (float)maxHealth, 7 * Time.deltaTime);
            WhileAlive();
        }
        else
        {
            WhileDead();
        }

        healthBarShowTimer = Mathf.Max(0, healthBarShowTimer - Time.deltaTime);

        foreach(Image image in healthbarHolder.GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, GameManager.INSTANCE.healthBarFade.Evaluate(healthBarShowTimer));
        }

        healthbarHolder.localScale = Vector2.one * healthbarScale * 0.13107f;
    }

    public void LateUpdate()
    {
        healthbarHolder.rotation = frozenHealthBarRotation;
        healthbarHolder.position = (Vector2)transform.position + healthbarOffset;
    }

    public virtual void WhileAlive()
    {

    }

    public virtual void WhileDead()
    {

    }

    public virtual void OnDeath()
    {

    }

    public void Kill()
    {
        isAlive = false;
        if(deathParticle != null) Instantiate(deathParticle, transform.position, Quaternion.identity);
        OnDeath();
        RemoveFromWorld();
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void ShowHealthbar()
    {
        healthBarShowTimer = healthBarShowtime + 1;
    }

}
