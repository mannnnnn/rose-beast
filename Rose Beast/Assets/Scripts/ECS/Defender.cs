using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class Defender : MonoBehaviour
{
    public int CurrentHealth = 1;
    public int MaxHealth = 1;
    public GameObject Drop;
    private TileBound tile;
    private Tilemap tilemap;

    public bool UseSlider = true;

    public int meat = 0; //number of exp per hit

    void Start()
    {
        tile = GetComponent<TileBound>();
        tilemap = FindObjectOfType<Tilemap>();
    }

    public void TakeDamage(int damageAmt, Attacker attacker){
        Eater eater = GetComponent<Eater>();
        Eater attackerEater = attacker.GetComponent<Eater>();

        if(attackerEater != null && this.GetComponent<Rose>() != null) return; //players can't damage thier own unit

        if(eater != null){
            //health is exp
            eater.LoseEXP(damageAmt);
        } else {
            CurrentHealth -= damageAmt;
        }

         ChimeraController.Instance.PlaySFX("Chomp");
        if(attackerEater != null && meat > 0){
            ChimeraController.Instance.PlaySFX("MeatyChomp", 0.3f);
            attackerEater.GetEXP(meat);
        } 

        if(CurrentHealth<=0){
            Die();
        } else {
            UpdateHealthSlider();
        }
    }

    public void UpdateHealthSlider(){
        if(!UseSlider) return;

        if(GetComponent<Eater>() != null){
            tile.UpdateSlider(CurrentHealth, MaxHealth, Color.Lerp(Color.red/3 + Color.white/2 + Color.blue/3, Color.white, ((float)CurrentHealth/2)/(float)MaxHealth));
        } else {
            tile.UpdateSlider(CurrentHealth, MaxHealth, Color.Lerp(Color.red + Color.white/4 + Color.blue/4, Color.white, ((float)CurrentHealth/2)/(float)MaxHealth));
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
