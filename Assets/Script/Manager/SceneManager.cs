using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneFight.Core
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameEvent _applicationStartEvent;
        [SerializeField] private GameEvent _splashTimeoutEvent;

        private void OnEnable()
        {
            _applicationStartEvent.AddListener(OnApplicationStart);
            _splashTimeoutEvent.AddListener(OnSplashTimeout);
        }

        private void OnDisable()
        {
            _applicationStartEvent.RemoveListener(OnApplicationStart);
            _splashTimeoutEvent.RemoveListener(OnSplashTimeout);
        }

        public void OnApplicationStart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
        }

        public void OnSplashTimeout()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }
    }
}

