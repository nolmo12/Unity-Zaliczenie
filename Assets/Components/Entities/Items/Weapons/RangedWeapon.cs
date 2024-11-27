using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public GameObject projectile;
    [SerializeField]
    private int ammo;
    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = Math.Clamp(value, 0, MaxAmmo);
        }
    }
    public int MaxAmmo;
    public float ReloadTime;
    public int NumberOfShots;
    public float Spread;

    private bool isReloading;

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        isInUse = false;
        isReloading = false;
    }

    public override IEnumerator Use(Mob mob)
    {
        if(!isReloading)
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


            for (int i = 0; i < NumberOfShots; i++)
            {
                float yaw = UnityEngine.Random.Range(-Spread, Spread);
                float angle = yaw;
                ShootProjectile(direction, origin, angle);
                Ammo--;

                if (Ammo < 1)
                {
                    if (!isReloading)
                    {
                        yield return Reload();
                    }
                    break;
                }
            }

            yield return new WaitForSeconds(AttackSpeed);

            isInUse = false;
        }
    }

    public IEnumerator Reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            Name += " (Reloading)";
            yield return new WaitForSeconds(ReloadTime);

            int index = Name.IndexOf(" (Reloading)");
            string cleanName = (index < 0)
                ? Name
                : Name.Remove(index, " (Reloading)".Length);

            Name = cleanName;

            Ammo = MaxAmmo;
            isReloading = false;
        }
    }

    public override string GetStats()
    {
        return $"{Ammo}/{MaxAmmo}, Czas przeładowywania: {ReloadTime}s, Liczba strzałów: {NumberOfShots}";
    }

        public void ShootProjectile(Vector3 direction, Vector3 origin, float angle)
    {

        Quaternion spreadRotation = Quaternion.Euler(0, angle, 0);
        GameObject spawnedProjectile = Instantiate(projectile, origin, Quaternion.identity);
        Projectile projectileComponent = spawnedProjectile.GetComponent<Projectile>();

        projectileComponent.SetLifeTime(Range);
        projectileComponent.SetDamage(Damage);

        Rigidbody rb = spawnedProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = spreadRotation * direction * projectileComponent.Speed;

            Debug.Log(rb.velocity.magnitude);
        }
    }
}
