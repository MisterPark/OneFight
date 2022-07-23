using OneFight.Core;
using UnityEngine;

namespace OneFight
{
    public class SplashLogic : MonoBehaviour
    {
        [SerializeField]
        private GameEvent _splashTimeoutEvent;
        [SerializeField] private float duration;
        private float tick = 0f;
        private bool isTimeout;


        private void Awake()
        {
            isTimeout = false;
        }

        void Update()
        {
            if (isTimeout) return;

            tick += Time.deltaTime;
            if (tick > duration)
            {
                isTimeout = true;
                _splashTimeoutEvent.Invoke();
            }
        }
    }
}

