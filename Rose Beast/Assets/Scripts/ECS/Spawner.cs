using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : Trait
{
    private TileBound tile;
    private Tilemap tilemap;
    public int spawnEveryMultipleOf = 1;
    public GameObject spawn;
    public bool useSlider = true;

    void Start()
    {
        tile = GetComponent<TileBound>();
         tilemap = FindObjectOfType<Tilemap>();
        if(useSlider) tile.UpdateSlider(0, spawnEveryMultipleOf, Color.green);
    }

    public void AgeChanged(){
         if(useSlider) tile.UpdateSlider(tile.age % spawnEveryMultipleOf, spawnEveryMultipleOf, Color.green);
        if(tile.age > 0 && tile.age % spawnEveryMultipleOf == 0){
            TrySpawn();
        }
    }

    public void TrySpawn(){

        //try all adjacent spaces going counter-clockwise
        //check cardinals first
        Vector2 foundDir = Vector2.zero;
        Vector3Int myTile = tilemap.WorldToCell((Vector2)this.transform.position);
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.down), myTile+Vector3Int.down)){
            foundDir = Vector2.down;
        }
        if(foundDir == Vector2.zero &&IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.right), myTile+Vector3Int.right)){
             foundDir = Vector2.right;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.up), myTile+Vector3Int.up)){
             foundDir = Vector2.up;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.left), myTile+Vector3Int.left)){
             foundDir = Vector2.left;
        }

        //then check diagonals
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.down+Vector3Int.right), myTile+Vector3Int.down+Vector3Int.right)){
             foundDir = Vector2.down + Vector2.right;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.up+Vector3Int.right), myTile+Vector3Int.up+Vector3Int.right)){
             foundDir = Vector2.up + Vector2.right;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.up+Vector3Int.left), myTile+Vector3Int.up+Vector3Int.left)){
             foundDir = Vector2.up + Vector2.left;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(myTile+Vector3Int.down+Vector3Int.left), myTile+Vector3Int.down+Vector3Int.left)){
             foundDir = Vector2.down + Vector2.left;
        }
       
       if(foundDir != Vector2.zero){
             SpawnOnDirection(foundDir);
       }
       
    }

    public bool IsValidSpawn(GameObject objOnTile, Vector3Int tile){
        
        if(ChimeraController.Instance.ReservedSpawns.Contains(tile)){
          return false; //something wants to spawn here
        }
        
        if(objOnTile == null) {
            return true;
        }
        if(objOnTile.GetComponent<Blocker>() != null) {
            return false;
        }
        
        return true;
    }

    public void SpawnOnDirection(Vector2 dir){
         if(spawn != null) {
            GameObject spawned = null;
            spawned = Instantiate(spawn, this.transform.parent);
            ChimeraController.Instance.ReservedSpawns.Add(tilemap.WorldToCell(this.transform.position)+ new Vector3Int((int)dir.x, (int)dir.y, 0));
            spawned.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(this.transform.position)+ new Vector3Int((int)dir.x, (int)dir.y, 0));
         }
    }
}
