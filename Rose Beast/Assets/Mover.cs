using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mover : MonoBehaviour {

    private Tilemap tilemap;
    private bool isMoving = false;
    float movementDelay = 0.2f;

    public void Start(){
        tilemap = FindObjectOfType<Tilemap>();
    }

    public void Move(Vector2 dir){
        if(CanMove(dir) && !isMoving){
            StartCoroutine(Moving(dir));
        } else if(!CanMove(dir)){
            StartCoroutine(Blocked(dir));
        }
    }

    public bool CanMove(Vector2 dir){
        Vector3Int nextCell = tilemap.WorldToCell((Vector2)transform.position) + new Vector3Int((int)dir.x, (int)dir.y, 0);
        return ChimeraController.Instance.FindObjectOnTile(nextCell) == null || ChimeraController.Instance.FindObjectOnTile(nextCell).GetComponent<Blocker>() == null;
    }

    IEnumerator Moving(Vector2 dir)
    {
       isMoving = true;
       float origZ = transform.position.z;
       Vector3Int origPosition = tilemap.WorldToCell(transform.position);

       Vector3Int nextCell = tilemap.WorldToCell((Vector2)transform.position) + new Vector3Int((int)dir.x, (int)dir.y, 0);

       float elapsedTime = 0;
        while(elapsedTime < movementDelay){
            transform.position = Vector2.Lerp(tilemap.GetCellCenterWorld(origPosition), tilemap.GetCellCenterWorld(nextCell), elapsedTime/movementDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

      transform.position = tilemap.GetCellCenterWorld(nextCell);
       isMoving = false;
       yield return null;
    }

    IEnumerator Blocked(Vector2 dir)
    {
       isMoving = true;
       float origZ = transform.position.z;
       Vector3Int origPosition = tilemap.WorldToCell(transform.position);
       Vector3Int nextCell = tilemap.WorldToCell((Vector2)transform.position) + new Vector3Int((int)dir.x, (int)dir.y, 0);

       float elapsedTime = 0;
        while(elapsedTime < movementDelay/2){
            transform.position = Vector2.Lerp(tilemap.GetCellCenterWorld(origPosition), tilemap.GetCellCenterWorld(nextCell), elapsedTime/movementDelay/2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Vector3 halfPos = transform.position;
         while(elapsedTime < movementDelay){
            transform.position = Vector2.Lerp(halfPos, tilemap.GetCellCenterWorld(origPosition), elapsedTime/movementDelay/2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

      transform.position = tilemap.GetCellCenterWorld(origPosition);
       isMoving = false;
       yield return null;
    }


}
