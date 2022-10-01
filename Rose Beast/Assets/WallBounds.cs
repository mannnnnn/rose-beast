using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounds : MonoBehaviour
{
    public void RemoveBounds(){
        foreach(Animator anim in transform.GetComponentsInChildren<Animator>()){
            anim.SetBool("On", false);
        }
    }

}
