using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private static List<Entity> allEntities = new List<Entity>();

    public static void RegisterEntity(Entity entity)
    {
        if (!allEntities.Contains(entity))
        {
            allEntities.Add(entity);
        }
    }

    public static void UnregisterEntity(Entity entity)
    {
        if (allEntities.Contains(entity))
        {
            allEntities.Remove(entity);
        }
    }

    public static List<Entity> GetAllEntities()
    {
        return new List<Entity>(allEntities);
    }

    public static List<T> GetAllEntitiesOfType<T>() where T : Entity
    {
        List<T> result = new List<T>();
        foreach (Entity entity in allEntities)
        {
            if (entity is T)
            {
                result.Add(entity as T);
            }
        }
        return result;
    }

    public static Entity CreateEntity(string entityName, Vector3 pos)
    {
        GameObject entity = Instantiate(Resources.Load<GameObject>($"Mobs/Enemies/Prefabs/{entityName}"));
        if (entity != null)
        {
            entity.transform.position = pos;
            return entity.GetComponent<Entity>();

        }
        return null;
    }
}
