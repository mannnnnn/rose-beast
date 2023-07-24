using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Follower;

[DisallowMultipleComponent]
public class Leader : Trait
{
    /*
    Follows the Leader whenever it moves
    Follows by occupying the last space the Leader was in, or the last space the higher index follower was in
    */
    
    public List<Follower> followers;
    public Mover mover;
    private Tilemap tilemap;

    public void Start(){
        mover = GetComponent<Mover>();
        tilemap = FindObjectOfType<Tilemap>();
    }

    public void OnMoved(Vector3Int lastPos){
        //Update the first follower, and it'll inform the others
        foreach(Follower follower in followers){
            if(follower == null) return;
            Vector3Int nextCell = lastPos;
            lastPos = tilemap.WorldToCell((Vector2)follower.transform.position);
            follower.mover.Move(new Vector2(nextCell.x, nextCell.y)-new Vector2(lastPos.x, lastPos.y) , false, true);
        }
    }

   
}
