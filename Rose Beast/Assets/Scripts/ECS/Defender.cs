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

        if(attacker.GetComponent<Eater>() != null && this.GetComponent<Rose>() != null) return; //players can't damage thier own unit


        CurrentHealth -= damageAmt;

        Eater eater = attacker.GetComponent<Eater>();
         ChimeraController.Instance.PlaySFX("Chomp");
        if(eater != null && meat > 0){
            ChimeraController.Instance.PlaySFX("MeatyChomp", 0.3f);
            eater.GetEXP(meat);
        } 

        if(CurrentHealth<=0){
            Die();
        } else {
            tile.UpdateSlider(CurrentHealth, MaxHealth, Color.red + Color.white/4 + Color.blue/4);
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
                if(droppedSpawn.GetComponent<RoseLord>() != null){
                    ChimeraController.Instance.RoselordSpawn();
                }
            }

            Destroy(this.gameObject);

            Rose rose = GetComponent<Rose>();
            if(rose != null){
                ChimeraController.Instance.GameOver();
            }

            RoseLord roseLord = GetComponent<RoseLord>();
            if(roseLord != null){
                ChimeraController.Instance.WinGame();
            }
        }


       
    }
}
