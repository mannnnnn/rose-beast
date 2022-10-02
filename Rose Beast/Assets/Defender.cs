using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Defender : MonoBehaviour
{
    public int CurrentHealth = 1;
    public int MaxHealth = 1;
    public GameObject Drop;
    private TileBound tile;
    private Tilemap tilemap;

    public int meat = 0; //number of exp per hit

    void Start()
    {
        tile = GetComponent<TileBound>();
        tilemap = FindObjectOfType<Tilemap>();
    }

    public void TakeDamage(int damageAmt, Attacker attacker){
        CurrentHealth -= damageAmt;
        if(CurrentHealth<=0){
            Die();
        } else {
            tile.UpdateSlider(CurrentHealth, MaxHealth, Color.red + Color.white/4 + Color.blue/4);
        }

        Eater eater = attacker.GetComponent<Eater>();
        if(eater != null && meat > 0){
            eater.GetEXP(meat);
        }
    }

    public void Die(){
        Eater eater = GetComponent<Eater>();
        if(eater != null){
            //knock down a peg and respawn
            eater.Devolve();
            eater.transform.position = FindObjectOfType<Rose>().transform.position;
        } else {
            if(Drop != null) {
                GameObject droppedSpawn = Instantiate(Drop, this.transform.parent);
                droppedSpawn.transform.position = this.transform.position;
            }
            Destroy(this.gameObject);
        }


       
    }
}
