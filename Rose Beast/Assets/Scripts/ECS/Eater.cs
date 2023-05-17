using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    private int currentEXP = 0;
    public int expRequiredBase;
    private int lastRequiredGrowingEXPLevel = 0;
   
    private int expRequired = 0;
    private TileBound tile;
    public int level = 1;
    public int maxLevel = 5;

    void Start()
    {
        tile = GetComponent<TileBound>();
        expRequired = expRequiredBase;
    }

    public void GetEXP(int expGain){
        currentEXP += expGain;
        if(currentEXP>=expRequired){
            lastRequiredGrowingEXPLevel = expRequiredBase;
            Evolve();
        } else {
            tile.UpdateSlider(currentEXP-lastRequiredGrowingEXPLevel, expRequiredBase-lastRequiredGrowingEXPLevel, Color.white);
        }
    }

    public void Evolve(){
        if(level < maxLevel){
            level++;
            GetComponent<Animator>().SetTrigger("Grow");
            GetComponent<Defender>().MaxHealth = level*2;
            GetComponent<Defender>().CurrentHealth = level*2;
            GetComponent<Attacker>().DamageAmt = level;
            expRequired = expRequiredBase*(level*2);
            tile.UpdateSlider(0, expRequiredBase, Color.white);
        }
       
    }

    public void Devolve(){
        if(level > 1){
            level--;
            GetComponent<Defender>().MaxHealth = level*2;
            GetComponent<Defender>().CurrentHealth = level*2;
            GetComponent<Attacker>().DamageAmt = level;
            GetComponent<Animator>().SetTrigger("Shrink");
            expRequired = expRequiredBase*(level*2);
        }
        
    }
}
