using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    public int CurrentHealth = 1;
    public int MaxHealth = 1;
    public GameObject Drop;
    private TileBound tile;

    public int meat = 0; //number of exp per hit

    void Start()
    {
        tile = GetComponent<TileBound>();
        tile.UpdateSlider(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(int damageAmt, Attacker attacker){
        CurrentHealth -= damageAmt;
        if(CurrentHealth<=0){
            Die();
        } else {
            tile.UpdateSlider(CurrentHealth, MaxHealth);
        }

        Eater eater = attacker.GetComponent<Eater>();
        if(eater != null && meat > 0){
            eater.GetEXP(meat);
        }
    }

    public void Die(){
        if(Drop != null) Instantiate(Drop, this.transform.parent);
        Destroy(this.gameObject);
    }
}
