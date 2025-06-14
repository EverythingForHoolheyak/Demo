using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PooledObject[] objectsToPool;

    [Serializable]
    public struct PooledObject
    {
        public string id;
        public GameObject gameObject;
        public int initialCount;
    }

    private Dictionary<string, Queue<GameObject>> objectPoolInStock;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (objectPoolInStock == null)
        {
            objectPoolInStock = new Dictionary<string, Queue<GameObject>>();
        }
        foreach (var pooledObject in objectsToPool)
        {
            if (!objectPoolInStock.ContainsKey(pooledObject.id))
            {
                objectPoolInStock.Add(pooledObject.id, new Queue<GameObject>());
            }
            for (var i = 0; i < pooledObject.initialCount; i++)
            {
                var obj = Instantiate(pooledObject.gameObject, transform);
                obj.SetActive(false);

                objectPoolInStock[pooledObject.id].Enqueue(obj);
            }
        }
    }

    public GameObject GetObject(string id)
    {
        if (objectPoolInStock.ContainsKey(id) && objectPoolInStock[id].Count > 0)
        {
            var obj = objectPoolInStock[id].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // If no objects are available, log a warning and return null
        var pooledObject = Array.Find(objectsToPool, o => o.id == id);
        var outPut = Instantiate(pooledObject.gameObject, transform);
        outPut.SetActive(true);
        return outPut;
    }

    public void ReturnObject(string id, GameObject obj)
    {
        if (objectPoolInStock.ContainsKey(id))
        {
            obj.SetActive(false);
            objectPoolInStock[id].Enqueue(obj);
        }
        else
        {
            objectPoolInStock.Add(id, new Queue<GameObject>());
            obj.SetActive(false);
            objectPoolInStock[id].Enqueue(obj);
        }
    }
}