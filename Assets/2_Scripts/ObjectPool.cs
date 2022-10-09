using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool;
    public List<GameObject> pooledObjects;
    
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    private int GetActiveObjectNum()
    {
        var count = 0;
        
        foreach (var item in pooledObjects)
        {
            if (item.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
    
    private GameObject InstantiateExtraObject()
    {
        var tmp = Instantiate(objectToPool, transform);
        tmp.SetActive(false);
        pooledObjects.Add(tmp);
        return tmp;
    }

    public void DisablesObjects()
    {
        foreach (var item in pooledObjects)
        {
            if (item.activeInHierarchy)
            {
                item.SetActive(false);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        if (GetActiveObjectNum() < pooledObjects.Count)
        {
            Debug.Log(pooledObjects.Count);
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
        }
        else
        {
            return InstantiateExtraObject();
        }

        return null;
    }
}
