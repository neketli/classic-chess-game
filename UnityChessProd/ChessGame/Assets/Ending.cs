using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{

    private Text text;
    static string textString;

    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("Text").GetComponent<Text>();
        text.text = textString;
    }

    void Update()
    {
        text.text = textString;
    }

    public static void SetText(string str)
    {
        textString = str;
    }

}
