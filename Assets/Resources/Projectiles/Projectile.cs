using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    public float Damage;
    public float Speed;
    private float LifeTime;
    private float timer = 0f;

    public void SetLifeTime(float time)
    {
        LifeTime = time;
    }
    public void SetDamage(float damage) 
    {
        Damage = damage;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Mob>() != null && !collision.gameObject.CompareTag("Player"))
        {
            Mob mob = collision.gameObject.GetComponent<Mob>();
            mob.setHealth(mob.getHealth() - Damage);
            Destroy(this.gameObject);
        }
        else if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log(collision.gameObject.tag);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        HandleLifeTime();

    }


    protected void HandleLifeTime()
    {
        if (timer >= 1f)
        {
            timer = 0f;
            LifeTime--;
        }

        if (LifeTime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
