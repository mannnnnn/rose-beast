using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public int DamageAmt = 1;

    private TileBound tile;

    public void Attack(Vector2 targetDir){
        GameObject obj = ChimeraController.Instance.FindObjectOnTile(transform.position, targetDir);
        Attack(obj);
    }

    public void Attack(GameObject obj){
        if(obj != null){
            Defender defender = obj.GetComponent<Defender>();
            if(defender != null){
                defender.TakeDamage(DamageAmt, this);
            }
        }
    }

}
