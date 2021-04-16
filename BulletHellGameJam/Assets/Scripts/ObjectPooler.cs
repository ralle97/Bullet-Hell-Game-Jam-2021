using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool shouldExpand = true;
    }

    #region Singleton

    public static ObjectPooler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("MORE THAN ONE ObjectPooler INSTANCE");
        }
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectList = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                objectList.Add(obj);
            }

            poolDictionary.Add(pool.tag, objectList);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        List<GameObject> list = poolDictionary[tag];

        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                return list[i];
            }
        }

        foreach (Pool pool in pools)
        {
            if (pool.tag == tag && pool.shouldExpand)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                list.Add(obj);
                return obj;
            }
        }

        return null;
    }
}
