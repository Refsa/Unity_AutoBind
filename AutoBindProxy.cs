using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100000)]
public class AutoBindProxy : MonoBehaviour
{
    bool isBound = false;

    void Awake() 
    {
        if (!isBound)
        {
            AutoBindComponent.AutoBind(this.gameObject);
            isBound = true;
        }
    }
}
