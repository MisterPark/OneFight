using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Text text;
    [SerializeField] private float duration = 2f;
    [SerializeField] private string totalOutput;
    [SerializeField] private Unit unit;
    [SerializeField] private Vector3 offset = new Vector3(0, 280, 0);

    private int current = 0;
    private float tick = 0f;

    public Unit Unit { get { return unit; } set { unit = value; } }
    public Vector3 Offset { get { return offset; } set { offset = value; } }

    public string TotalOutput { get { return totalOutput; } set { totalOutput = value; } }

    private void OnValidate()
    {
        if (text == null) text = GetComponent<Text>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        tick = 0f;
        current = 0;
        var canvas = FindObjectOfType<Canvas>();
        if(canvas != null)
        {
            transform.parent = canvas.transform;
            transform.localScale = Vector3.one;
        }
    }

    private void Update()
    {
        float unitTime = duration / totalOutput.Length;
        float currentTime = tick / unitTime;

        current = Mathf.RoundToInt(currentTime);

        text.text = totalOutput.Substring(0, current);

        rectTransform.sizeDelta = new Vector2((float)text.preferredWidth + 10f, rectTransform.sizeDelta.y);
        tick += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (Unit == null) return;
        transform.position = Camera.main.WorldToScreenPoint(Unit.transform.position) + offset;
    }
}
