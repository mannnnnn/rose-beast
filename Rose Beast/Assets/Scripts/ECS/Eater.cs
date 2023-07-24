using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Eater : Trait
{
    private int currentEXP = 0;
    public int expRequiredBase;
    private int lastRequiredGrowingEXPLevel = 0;
   
    private int expRequired = 0;
    private TileBound tile;
    public int level = 1;
    public int maxLevel = 5;

    private Defender defender;
    private Attacker attacker;
    private Animator animator;

    void Start()
    {
        tile = GetComponent<TileBound>();
        defender = GetComponent<Defender>();
        attacker = GetComponent<Attacker>();
        animator = GetComponent<Animator>();

        expRequired = expRequiredBase;
    }

    public void GetEXP(int expGain){
        currentEXP += expGain;
        defender.MaxHealth = expRequired;
        defender.CurrentHealth = currentEXP;
        if(currentEXP>=expRequired){
            lastRequiredGrowingEXPLevel = expRequiredBase;
            Evolve();
        } else {
           defender.UpdateHealthSlider();
        }
    }

    public void LoseEXP(int expLoss){
        currentEXP -= expLoss;
        defender.MaxHealth = expRequired;
        defender.CurrentHealth = currentEXP;
        if(currentEXP < lastRequiredGrowingEXPLevel){
            expRequiredBase = lastRequiredGrowingEXPLevel;
            Devolve();
        } else {
            defender.UpdateHealthSlider();
        }
    }

    public void Evolve(){
        if(level < maxLevel){
            level++;
            animator.SetTrigger("Grow");
            attacker.DamageAmt = level;
            expRequired = expRequiredBase*(level*2);
            defender.MaxHealth = expRequired;
            defender.CurrentHealth = currentEXP;
            defender.UpdateHealthSlider();
        }
       
    }

    public void Devolve(){
        if(level > 1){
            level--;
            attacker.DamageAmt = level;
            animator.SetTrigger("Shrink");
            expRequired = expRequiredBase*(level*2);
            defender.MaxHealth = expRequired;
            defender.CurrentHealth = currentEXP;
            defender.UpdateHealthSlider();
        }
        
    }
}
