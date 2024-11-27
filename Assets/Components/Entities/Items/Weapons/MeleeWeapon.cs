using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float Angle;
    public override void Init()
    {
        base.Init();
        isInUse = false;
    }
    public override string GetStats()
    {
        return "";
    }
    void Start()
    {
        Init();
    }
    public override IEnumerator Use(Mob mob)
    {
        isInUse = true;
        Vector3 direction;
        Vector3 origin;
        if (mob is Player player)
        {
            direction = player.playerCharacter.transform.forward;

            origin = player.playerCharacter.transform.position + direction * 2f;
            origin.y = 1f;
        }
        else
        {
            direction = mob.transform.forward;
            origin = mob.transform.position + direction * 2f;
            origin.y = 1f;
        }

        Collider[] hitColliders = Physics.OverlapSphere(origin, Range);

        foreach (Collider hit in hitColliders)
        {
            Vector3 toTarget = (hit.transform.position - origin).normalized;

            if (Vector3.Angle(direction, toTarget) <= Angle)
            {
                if(!hit.CompareTag("Player") && !hit.CompareTag("Terrain"))
                {
                    if(hit.CompareTag("Enemy"))
                    {
                        Debug.Log("trafiłem wroga");
                        Mob enemy = hit.GetComponent<Mob>();
                        if (enemy != null)
                        {
                            int randomSound = Random.Range(0, audioClips.Count);
                            audioSource.enabled = true;
                            PlaySound(audioClips[randomSound]);

                            Debug.Log($"Before: {enemy.getHealth()}");
                            enemy.setHealth(enemy.getHealth() - Damage);
                            Debug.Log($"After: {enemy.getHealth()}");
                        }
                    }
                    else
                    {

                    }    
                }
            }
        }


        Debug.Log($"{Name} was used");

        yield return new WaitForSeconds(AttackSpeed);

        isInUse = false;
    }
}
