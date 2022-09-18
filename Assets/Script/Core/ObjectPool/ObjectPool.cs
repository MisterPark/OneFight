using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Instance { get { return instance; } }

    [SerializeField] private ObjectPoolObject poolData;

    private Dictionary<string, Stack<GameObject>> pool = new Dictionary<string, Stack<GameObject>>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    private void Initialize()
    {
        int length = poolData.pool.Length;
        for(int i = 0; i < length; i++)
        {
            var key = poolData.pool[i].Key;

            if (pool.TryGetValue(key, out var stack)) continue;

            pool.Add(key, new Stack<GameObject>());
        }
    }

    private GameObject GetPrefab(string key)
    {
        var pair = poolData.pool.SingleOrDefault(x => x.Key == key);
        if (pair == null)
        {
            Debug.LogError($"{key} does not exist.");
            return null;
        }

        return pair.Value;
    }

    public GameObject Allocate(string key)
    {
        if(pool.TryGetValue(key, out var stack))
        {
            if(stack.Count == 0)
            {
                var prefab = GetPrefab(key);
                var poolObject = Instantiate(prefab);
                poolObject.name = key;
                poolObject.SetActive(true);

                return poolObject;
            }

            var obj = stack.Pop();
            obj.SetActive(true);
            
            return obj;
        }

        Debug.LogError($"{key} does not exist.");
        return null;
    }

    public void Free(GameObject gameObject)
    {
        if(pool.TryGetValue(gameObject.name, out var stack))
        {
            gameObject.SetActive(false);
            stack.Push(gameObject);
            return;
        }

        Debug.LogError($"{gameObject.name} does not exist.");
    }
}
