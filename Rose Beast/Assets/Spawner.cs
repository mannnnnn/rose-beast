using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
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
        tile.onAgeChanged += AgeChanged;
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
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.down))){
            foundDir = Vector2.down;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.right))){
             foundDir = Vector2.right;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.up))){
             foundDir = Vector2.up;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.left))){
             foundDir = Vector2.left;
        }

        //then check diagonals
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.down + Vector2.right) )){
             foundDir = Vector2.down + Vector2.right;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.up + Vector2.right))){
             foundDir = Vector2.up + Vector2.right;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.up + Vector2.left))){
             foundDir = Vector2.up + Vector2.left;
        }
        if(foundDir == Vector2.zero && IsValidSpawn(ChimeraController.Instance.FindObjectOnTile(transform.position, Vector2.down + Vector2.left))){
             foundDir = Vector2.down + Vector2.left;
        }
       
       if(foundDir != Vector2.zero){
             SpawnOnDirection(foundDir);
       }
       
    }

    public bool IsValidSpawn(GameObject objOnTile){
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
            spawned.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(this.transform.position)+ new Vector3Int((int)dir.x, (int)dir.y, 0));
         }
    }
}