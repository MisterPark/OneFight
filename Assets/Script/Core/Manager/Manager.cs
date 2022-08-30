using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneFight.Core
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance; 
        [SerializeField] private GameEvent _applicationStartEvent;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _applicationStartEvent.Invoke();
        }
    }
}
