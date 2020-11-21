//from https://www.youtube.com/watch?v=tdSmKaJvCoA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{

    public string Tag;
    public GameObject Prefab;
    public int PoolSize;
}

//Pool obstacles with enough number to show at the same time on screen
public class ObjectPooler : MonoBehaviour
{
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public Transform PooledObjectsTransform;

    public GameObject SpawnFromPool (string tag, Vector2 position)
    {
        GameObject objToSpawn = PoolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;

        //If we pooled each obstacles enough to show at the same time on screen,
        //there will be no problems enqueing it right away.
        PoolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }

    private void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i=0; i<pool.PoolSize; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, PooledObjectsTransform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Tag, objectPool);
        }
    }
}