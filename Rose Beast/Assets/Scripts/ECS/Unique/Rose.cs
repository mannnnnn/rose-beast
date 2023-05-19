using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Rose : Grower
{
    public List<Sprite> growingSprites = new List<Sprite>();
    public List<int> requiredGrowingAges = new List<int>();
    public int level = 1;
    public int winningLevel = 4;
    void Start()
    {
        tile = GetComponent<TileBound>();
        growingAge = requiredGrowingAges[0];
        tile.UpdateSlider(0, growingAge, Color.white);
    }

    public override void Grow(){
        
        level++;
        GetComponentInChildren<SpriteRenderer>().sprite = growingSprites[level-1];

        if(level == winningLevel){
            ChimeraController.Instance.WinGame(); 
            return;
        }

        FindObjectOfType<BoundsController>().ExpandBounds();
        if(requiredGrowingAges.IndexOf(growingAge) < requiredGrowingAges.Count-1){
            growingAge = requiredGrowingAges[level-1];
            tile.UpdateSlider(0, growingAge, Color.white);
        } else {
            growingAge = -1;
        }
    }

}
