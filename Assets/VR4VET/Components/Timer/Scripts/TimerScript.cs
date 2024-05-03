using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class TimerScript : MonoBehaviour
{

    public IEnumerator StartTimer (int Timer, TMP_Text TimerDiplay) {
        for (int i = Timer; i >= 0; i--){
            TimeSpan RemainingTime = new TimeSpan(0, 0, i);
            TimerDiplay.text = RemainingTime.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1f);
        }
    }
    public IEnumerator StartTimer (TMP_Text TimerDiplay, bool CountUp) {
        TimeSpan Timer = new TimeSpan(0, 0, 0);
        while(CountUp) {
            Timer += new TimeSpan(0, 0, 1);
            TimerDiplay.text = Timer.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1f);
        }
    }

#if UNITY_EDITOR
    [SerializeField] private TMP_Text TimerDiplay;
    [SerializeField] private int Timer;
    [SerializeField] private bool CountUp;
    public void StartTimer(){
        if (!CountUp) {
            StartCoroutine(StartTimer(Timer, TimerDiplay));
        } else {
            StartCoroutine(StartTimer(TimerDiplay, CountUp));
        }
    }
#endif    
}
