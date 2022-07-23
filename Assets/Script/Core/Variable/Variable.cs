using System;
using UnityEngine;

namespace OneFight.Core
{
    [CreateAssetMenu(fileName = "Variable", menuName = "Scriptable Object/Variable", order = int.MaxValue)]
    public class Variable<T> : ScriptableObject
    {
        [SerializeField] private T _value;
        private GameEvent<T> _changedValueEvent;

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _changedValueEvent?.Invoke(_value);
            }
        }

        public void AddListener(Action<T> call)
        {
            _changedValueEvent.AddListener(call);
        }

        public void RemoveListener(Action<T> call)
        {
            _changedValueEvent.RemoveListener(call);
        }

        public void RemoveAllListener()
        {
            _changedValueEvent.RemoveAllListeners();
        }
    }
}
