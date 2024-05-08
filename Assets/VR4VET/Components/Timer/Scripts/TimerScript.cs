using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
namespace Task {
    public class TimerScript : MonoBehaviour
    {
        private IEnumerator _startTimer (int Timer, string TimerDiplay, Step Step) {
            if (Timer > 0){
                for (int i = Timer; i >= 0; i--){
                    TimeSpan RemainingTime = new TimeSpan(0, 0, i);
                    TimerDiplay = RemainingTime.ToString(@"mm\:ss");
                    step.Counter = TimerDiplay;
                    yield return new WaitForSeconds(1f);
                }
            }else if(Timer == 0) {
                TimeSpan timer = new TimeSpan(0, 0, 0);
                while(Timer != null) {
                    timer += new TimeSpan(0, 0, 1);
                    TimerDiplay = timer.ToString(@"mm\:ss");
                    Debug.Log(TimerDiplay);
                    step.Counter = TimerDiplay;
                    yield return new WaitForSeconds(1f);
                }
            }else {
                yield return null;
            }
        }

        public void startTimer(int Timer, string TimerDiplay, Step step) {
            StartCoroutine(_startTimer(Timer, TimerDiplay, step));
        }






#if UNITY_EDITOR
        [SerializeField] private int Timer;
        [SerializeField] private string TimerDiplay;
        [SerializeField] private Step step;
        public void startTimer(){
            StartCoroutine(_startTimer(Timer, TimerDiplay, step));
        }
        
#endif    
    }
}
