using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicMonster : Mob
{
    public float Damage;
    public float DamageCooldown;
    public NavMeshAgent Agent;

    public void MoveToTarget(Vector3 destination)
    {
        Agent.SetDestination(destination);
        Agent.isStopped = false;
    }

    public void StopAgent()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
    }

    public override void Init()
    {
        base.Init();
    }

    void Start()
    {
        Init();
        StartCoroutine(FollowPlayer());
    }
    void Update()
    {
        if(isDead())
        {
            Death();
        }
    }

    private IEnumerator FollowPlayer()
    {
        Player player = EntityController.GetAllEntitiesOfType<Player>()[0];
        if (player == null)
        {
            yield break;
        }

        while (true)
        {
            if (player.isDead())
            {
                yield break;
            }

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < 2.25f)
            {
                player.setHealth(player.getHealth() - Damage);

                CameraShaker cameraShaker = player.GetComponentInChildren<CameraShaker>();
                if (cameraShaker != null)
                {
                    cameraShaker.StartCoroutine(cameraShaker.Shake(0.15f, 0.4f));
                }

                yield return new WaitForSeconds(DamageCooldown);
            }

            MoveToTarget(player.transform.position);
            yield return null;
        }
    }
}
