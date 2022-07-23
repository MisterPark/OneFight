using UnityEngine;
using UnityEditor;

namespace OneFight.Core
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameEvent gameEvent = (GameEvent)target;
            if (GUILayout.Button("Invoke")) 
            {
                gameEvent.Invoke();
            }
        }
    }

}
