using System;
using System.Collections.Generic;
using UnityEngine;

namespace OneFight.Core
{
    public abstract class GameEventBase<T> : ScriptableObject
    {
        private List<Action> _listeners = new List<Action>();

        public void AddListener(Action call)
        {
            _listeners.Add(call);
        }

        public void RemoveListener(Action call)
        {
            _listeners.Remove(call);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void Invoke()
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke();
            }
        }
    }

    [CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Object/GameEvent/void", order = int.MaxValue)]
    public class GameEvent : ScriptableObject
    {
        private List<Action> _listeners = new List<Action>();

        public void AddListener(Action call)
        {
            _listeners.Add(call);
        }

        public void RemoveListener(Action call)
        {
            _listeners.Remove(call);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void Invoke()
        {
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke();
            }
        }

    }

    public class GameEvent<T> : ScriptableObject
    {
        [SerializeField] private T _lastParam;
        private List<Action<T>> _listeners = new List<Action<T>>();

        public void AddListener(Action<T> call)
        {
            _listeners.Add(call);
        }

        public void RemoveListener(Action<T> call)
        {
            _listeners.Remove(call);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void Invoke(T arg)
        {
            _lastParam = arg;
            var count = _listeners.Count;
            for (int i = 0; i < count; i++)
            {
                _listeners[i].Invoke(arg);
            }
        }
    }

    [CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Object/GameEvent/String", order = int.MaxValue)]
    public class GameEventString : GameEvent<string>
    {

    }
}
