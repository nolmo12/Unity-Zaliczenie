using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public class RangeMonster : Mob, IGiveItem
{
    public float Damage;
    public float DamageCooldown;

    public Weapon Weapon;

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
        GiveItem("Weapons/Prefabs/Pistol", new Vector3(5.4f, 18f, 2.15f));
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

            if (distance < Weapon.Range && Weapon.CheckIfInUse())
            {
                StartCoroutine(Weapon.Use(this));

                yield return new WaitForSeconds(DamageCooldown);
            }

            MoveToTarget(player.transform.position);
            yield return null;
        }
    }

    public GameObject GiveItem(string path, Vector3 pos)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>(path);

        if (loadedPrefab == null)
        {
            return null;
        }

        GameObject itemGameObject = Instantiate(loadedPrefab, new Vector3(0, 0, 0), transform.rotation);

        itemGameObject.transform.parent = transform;

        itemGameObject.transform.localPosition = pos;

        itemGameObject.tag = "Player";
        itemGameObject.SetActive(false);

        Weapon = itemGameObject.GetComponent<Weapon>();

        return itemGameObject;
    }

    public void ShootToPlayer()
    {

    }
}
