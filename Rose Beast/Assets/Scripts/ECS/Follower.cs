using UnityEngine;
using UnityEngine.Tilemaps;

public class Follower : MonoBehaviour {
    /*
    Follows the Leader without adhering to the timer rules
    Follows by occupying the last space the Eater was in, or the last space the higher index follower was in
    */
    
    public Mover mover;
    public Leader leader;

    public void Start(){
        mover = GetComponent<Mover>();
        //tilemap = FindObjectOfType<Tilemap>();
    }

    public void FindNearbyLeader(){
        if(leader == null){
            //check adjacent tiles for Leaders
            //tilemap = FindObjectOfType<Tilemap>();
        }
    }

    
}
