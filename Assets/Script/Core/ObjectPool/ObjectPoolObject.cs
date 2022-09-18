using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Pool Object", menuName = "Scriptable Object/Object Pool Object", order = int.MaxValue)]
public class ObjectPoolObject : ScriptableObject
{
#if UNITY_EDITOR
    [ArrayElementTitle("Key")]
#endif
    public Pair<string, GameObject>[] pool = new Pair<string, GameObject>[1];
}

[System.Serializable]
public class Pair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}