using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : MonoBehaviour
{
    protected TileBound tile;
    public GameObject nextForm;
    public int growingAge;
    public bool useSlider = false;

    void Start()
    {
        tile = GetComponent<TileBound>();
        if(useSlider)tile.UpdateSlider(0, growingAge, Color.white);
    }

    public void AgeChanged(){
        if(growingAge >= 0){
            if(useSlider)tile.UpdateSlider(tile.age, growingAge, Color.white);
            if(tile.age >= growingAge){
                Grow();
            } 
        } else {
            tile.UpdateSlider(0,1, Color.white);
        }
    }

    public virtual void Grow(){
        //replace
        if(nextForm != null) {
            GameObject spawned = null;
            spawned = Instantiate(nextForm, this.transform.parent);
            spawned.transform.position = this.transform.position;
         }
        Destroy(this.gameObject);
    }
}
