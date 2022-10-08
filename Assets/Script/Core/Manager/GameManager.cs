using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

#if UNITY_EDITOR
    [ArrayElementTitle("kind")]
#endif
    public SkillElement[] skillDatas = new SkillElement[(int)SkillKind.End];

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
        }

        Screen.SetResolution(300, 195, true);
    }
}
