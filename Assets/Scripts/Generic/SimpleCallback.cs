using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCallback : MonoBehaviour
{
    private Action greetingAction;              //액션 선언 
  
    void Start()
    {
        greetingAction = SayHello;              //Action 함수 할당
        PerformGreeting(greetingAction);
    }

    void SayHello()
    {
        Debug.Log("Hello, world!");
    }

    void PerformGreeting(Action greetingFunc)
    {
        greetingFunc?.Invoke();
    }  
}
