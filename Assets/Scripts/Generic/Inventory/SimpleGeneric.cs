using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGeneric : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PrintValue(42);
        PrintValue("Hello, Generics!");
        PrintValue(3.14f);
    }

    void PrintValue<T>(T value)
    {
        Debug.Log($"Value : {value}, Type : {typeof(T)}");
    }
    
}
