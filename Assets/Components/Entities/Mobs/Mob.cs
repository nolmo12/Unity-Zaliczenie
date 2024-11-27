using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Mob : Entity
{
    protected float Health;
    public float MaxHealth;
    public float Speed;
    public float SprintModifier;
    public float RotationSpeed = 5f;
    public List<Effect> Effects;
    public Rigidbody Rigidbody;

    public bool isDead()
    {
        return Health <= 0;
    }

    public virtual void Death()
    {
        GameDirector.Instance.IncrementEnemiesSlain();
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    public float getHealth()
    {
        return Health;
    }
    public void setHealth(float val)
    {
        Health = Mathf.Max(0, Mathf.Min(val, MaxHealth));
    }

    public override void Init()
    {
        base.Init();
        Health = MaxHealth;
    }
}
