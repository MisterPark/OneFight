using UnityEngine;

namespace OneFight.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (_instance == null)
                    {
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        _instance.name = typeof(T).Name;

                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }
}

