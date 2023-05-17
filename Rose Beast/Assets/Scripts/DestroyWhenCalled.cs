using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenCalled : MonoBehaviour
{
   public void DestroyMe(){
        Destroy(this.gameObject);
   }
}
