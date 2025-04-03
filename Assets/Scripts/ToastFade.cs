using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastFade : MonoBehaviour
{
    public Image tp;
    public TextMeshProUGUI text;
    public float a;
    public int pressCount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (a >= 0)
        {
            a -= Time.deltaTime * .4f;
            tp.color = new Color(tp.color.r,tp.color.g,tp.color.b,a);
            text.color = new Color(text.color.r,text.color.g,text.color.b,a);
        }
        if(pressCount > 15)
        {
            text.text = "stop";
        }
    }
}
