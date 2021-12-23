using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int wSeconds = 5;
    int wMinutes = 15;
    int bSeconds = 5;
    int bMinutes = 15;
    private Text whiteTimerText;
    private Text blackTimerText;
    static bool WhiteTime = true;
    static bool BlackTime = false;

    void Start()
    {
        whiteTimerText = GameObject.Find("WhiteTimer").GetComponent<Text>();
        blackTimerText = GameObject.Find("BlackTimer").GetComponent<Text>();
        StartCoroutine(Timers());
    }

    IEnumerator Timers()
    {
        WhiteTime = true;
        BlackTime = false;
        while (true)
        { 
            if (WhiteTime)
            {
                wSeconds--;
                whiteTimerText.text = wMinutes.ToString("D2") + " : " + wSeconds.ToString("D2");
                if (wMinutes == 0 && wSeconds == 0)
                {
                    SceneManager.LoadScene("Ending", LoadSceneMode.Single);
                    Ending.SetText("Black wins!");

                }
                if (wSeconds == 0)
                {
                    wMinutes--;
                    wSeconds = 59;
                }
                yield return new WaitForSeconds(1);
            }
            if (BlackTime)
            {
                bSeconds--;
                blackTimerText.text = bMinutes.ToString("D2") + " : " + bSeconds.ToString("D2");
                if (bMinutes == 0 && bSeconds == 0)
                {
                    SceneManager.LoadScene("Ending", LoadSceneMode.Single);
                    Ending.SetText("White wins!");
                }
                if (bSeconds == 0)
                {
                    bMinutes--;
                    bSeconds = 59;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }

    public static void SwitchTimer()
    {
        WhiteTime = !WhiteTime;
        BlackTime = !BlackTime;
    }

}
