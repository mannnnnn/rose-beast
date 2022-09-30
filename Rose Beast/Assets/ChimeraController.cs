using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChimeraController : MonoBehaviour
{
    public TextMeshPro TimerLabel;
    public int Timer = 10;

    bool gameRunning = true;

    void Start()
    {
        TimerLabel.text = Timer.ToString().PadLeft(2);
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while(gameRunning){

            if(Timer <= 0){
                Timer = 11;
                TimesUp();
            }
            yield return new WaitForSeconds(1);
            Timer--;
            TimerLabel.text = Timer.ToString().PadLeft(2,'0');
        }
        
    }

    public void TimesUp(){
        UnityEngine.Debug.Log("Go!");
    }
}
