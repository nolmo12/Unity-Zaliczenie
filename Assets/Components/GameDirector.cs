using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    private string[] Mobs = new string[5];

    private float timer = 0f;
    private int howMuchTimeHasPassed;
    private int howManyEnemiesSlain = 0;
    public float delayAmount;

    public Vector3 PlayerPos;

    public GameObject EndScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Mobs[0] = "BasicMonster";
        Mobs[1] = "RangeMonster";
        Mobs[2] = "BasicMonster";
        Mobs[3] = "RangeMonster";
        Mobs[4] = "BasicMonster";
    }

    void Start()
    {
        StartCoroutine(MobSpawner());
        Debug.Log("This is alive!");
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delayAmount && !FindObjectOfType<Player>().isDead())
        {
            timer = 0f;
            howMuchTimeHasPassed++;
        }
    }
    public void IncrementEnemiesSlain()
    {
        howManyEnemiesSlain++;
    }

    public int getHowManyEnemiesSlain()
    {
        return howManyEnemiesSlain;
    }

    public int getHowMuchTimeHasPassed()
    {
        return howMuchTimeHasPassed;
    }

    public IEnumerator MobSpawner()
    {
        while(true)
        {
            if (EntityController.GetAllEntitiesOfType<Mob>().Count < 100 && howMuchTimeHasPassed > 5)
            {
                int randomMobChance = Random.Range(0, Mobs.Length);

                Debug.Log(randomMobChance);

                Vector2 randomPoint = GetRandomPointInCircle(10f, 15f);
                Vector3 randomPoint3 = new Vector3(randomPoint.x, 0, randomPoint.y);

                EntityController.CreateEntity(Mobs[randomMobChance], randomPoint3);

                yield return new WaitForSeconds(Random.Range(0, 10 / Mathf.Log10(howMuchTimeHasPassed + 5)));
            }
            yield return null;
        }
    }

    public Vector2 GetRandomPointInCircle(float min, float max)
    {
        float angle = Random.Range(0, Mathf.PI*2);

        float distance = Mathf.Sqrt(Random.Range(min*min, max*max));

        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;

        return new Vector2(PlayerPos.x + x, PlayerPos.y + y);
    }
}
