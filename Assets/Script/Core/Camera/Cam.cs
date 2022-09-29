using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public static Cam Instance;

    [SerializeField] private Vector3 shakeRange;
    [SerializeField] private float shakeDuration = 0.2f;
    private float shakeTick = 0f;
    private bool shakeFlag = false;
    private Vector3 shakeOffset;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        ShakeProcess();
    }

    private void LateUpdate()
    {
        if(Player.Self != null)
        {
            var playerPosition = Player.Self.transform.position;
            var playerOffset = new Vector3(playerPosition.x, playerPosition.y + 0.5f, transform.position.z);
            transform.position = playerOffset + shakeOffset;
        }
    }

    private void ShakeProcess()
    {
        if (shakeFlag)
        {
            shakeTick += Time.deltaTime;
            if (shakeTick < shakeDuration)
            {
                var half = shakeRange * 0.5f;
                float x = Random.Range(0, shakeRange.x) - half.x;
                float y = Random.Range(0, shakeRange.y) - half.y;
                shakeOffset = new Vector3(x, y, 0);
            }
            else
            {
                shakeTick = 0;
                shakeFlag = false;
            }
        }
        else
        {
            shakeOffset = Vector3.zero;
        }
    }

    public void Shake()
    {
        shakeFlag = true;
        shakeTick = 0;
    }
}
