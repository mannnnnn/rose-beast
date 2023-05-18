using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOptions : MonoBehaviour
{
    public bool debug = false;
    public bool noTimer = false;

    public void ToggleTimer(){
        noTimer = !noTimer;
    }
}
