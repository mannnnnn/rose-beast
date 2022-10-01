using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour
{
    private int currentEXP = 0;
    public List<Sprite> growingSprites = new List<Sprite>();
    public List<int> requiredGrowingEXPLevels = new List<int>();
    private int nextRequiredGrowingEXPLevel;
    private int lastRequiredGrowingEXPLevel = 0;
   
    private TileBound tile;

    void Start()
    {
        tile = GetComponent<TileBound>();
        nextRequiredGrowingEXPLevel = requiredGrowingEXPLevels[0];
        tile.UpdateSlider(currentEXP-lastRequiredGrowingEXPLevel, nextRequiredGrowingEXPLevel-lastRequiredGrowingEXPLevel);
    }

    public void GetEXP(int expGain){
        currentEXP += expGain;
        if(currentEXP>=nextRequiredGrowingEXPLevel){
            tile.UpdateSlider(0, nextRequiredGrowingEXPLevel);
            Evolve();
        } else {
            tile.UpdateSlider(currentEXP-lastRequiredGrowingEXPLevel, nextRequiredGrowingEXPLevel-lastRequiredGrowingEXPLevel);
        }
    }

    public void Evolve(){
        GetComponentInChildren<SpriteRenderer>().sprite = growingSprites[growingSprites.IndexOf(GetComponentInChildren<SpriteRenderer>().sprite)+1];
    }
}
