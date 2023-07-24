using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class Mover : Trait
{

    public MovementType movementType;
    public int tilesPerCycle;


    private Tilemap tilemap;
    private TileBound tile;
   
    private Coroutine movingAction = null;
    private Vector3Int cellToMoveTo = Vector3Int.zero;
    
    public enum MovementType{
        NONE,
        RANDOM,
    }

    private bool isMoving = false;
    float movementDelay = 0.2f;

    public void Start(){
        tilemap = FindObjectOfType<Tilemap>();
        tile = FindObjectOfType<TileBound>();
    }

    List<GameObject> lines = new List<GameObject>();
    public List<Vector2> plannedDirections = new List<Vector2>();
    public void PlanMove(){
        if(tile == null || tilemap == null) return;
        if(movementType == MovementType.NONE) return;
        Vector3Int lastCell =  tilemap.WorldToCell(this.transform.position);
        Vector2 lastDir = Vector2.zero;

        for(int i = 0; i < tilesPerCycle; i++){
            Vector2 nextDir = Vector2.right;
            switch(movementType){
                case MovementType.RANDOM: 
                default:
                    nextDir = FindRandomMove(lastCell, lastDir);
                break;
            }
            plannedDirections.Add(nextDir);
            if(nextDir == Vector2.zero) continue;
            //Create arrow
            LineRenderer pathSegment = Instantiate(ChimeraController.Instance.MovementLine, this.transform).GetComponent<LineRenderer>();
            lines.Add(pathSegment.gameObject);
            Vector3Int nextCell = lastCell + new Vector3Int((int)nextDir.x, (int)nextDir.y, 0);
            pathSegment.SetPosition(0, tilemap.GetCellCenterWorld(lastCell));
            pathSegment.SetPosition(1, tilemap.GetCellCenterWorld(nextCell));
            pathSegment.startColor = tile.unitColor;
            pathSegment.endColor = tile.unitColor;
            


            if(i == tilesPerCycle-1 && nextDir != Vector2.zero){
                //last segment, spawn arrowhead
                GameObject arrowhead = Instantiate(ChimeraController.Instance.Arrowhead);
                arrowhead.GetComponent<SpriteRenderer>().color = tile.unitColor;
                arrowhead.transform.position= tilemap.GetCellCenterWorld(nextCell);
                if(nextDir == Vector2.up){
                    arrowhead.transform.transform.eulerAngles = new Vector3(0,0,90);
                } else if(nextDir == Vector2.left){
                    arrowhead.transform.transform.eulerAngles = new Vector3(0,0,180);
                } else if(nextDir == Vector2.down){
                    arrowhead.transform.transform.eulerAngles = new Vector3(0,0,270);
                } else if(nextDir == Vector2.right){
                    arrowhead.transform.transform.eulerAngles = new Vector3(0,0,0);
                }
                lines.Add(arrowhead);
            }


            lastCell = nextCell;
            lastDir = nextDir;
        }
    }

    public Vector2 FindRandomMove(Vector3Int lastPos, Vector2 lastDir){
        List<Vector2> possibleMoves = new List<Vector2>(){Vector2.up, Vector2.down, Vector2.left, Vector2.right};

        //don't undo the last move
        if(lastDir == Vector2.up) possibleMoves.Remove(Vector2.down);
        if(lastDir == Vector2.down) possibleMoves.Remove(Vector2.up);
        if(lastDir == Vector2.left) possibleMoves.Remove(Vector2.right);
        if(lastDir == Vector2.right) possibleMoves.Remove(Vector2.left);

        Vector2 nextDir = Vector2.zero;
        
        while(possibleMoves.Count > 0 && nextDir == Vector2.zero){
            nextDir = possibleMoves[Random.Range(0, possibleMoves.Count)];
            possibleMoves.Remove(nextDir);
            if(!CanMove(ChimeraController.Instance.FindObjectOnTile(tilemap.GetCellCenterWorld(lastPos),nextDir))){
                nextDir = Vector2.zero;
            }
            if(ChimeraController.Instance.MoverPaths.Contains(lastPos+ new Vector3Int((int)nextDir.x, (int)nextDir.y, 0))){
                //another unit wants to move here!
                nextDir = Vector2.zero;
            }
        }
        ChimeraController.Instance.MoverPaths.Add(lastPos+ new Vector3Int((int)nextDir.x, (int)nextDir.y, 0));
        return nextDir;
    }

    
    public void ExecuteMove(){
        movingAction = StartCoroutine(ExecuteMoveOnDelay());
    }

    void OnDestroy() {
        for(int i = 0; i<lines.Count;)
        {
            Destroy(lines[0].gameObject);
            lines.Remove(lines[0]);
        } 
        if(movingAction != null) StopCoroutine(movingAction);
    }

    IEnumerator ExecuteMoveOnDelay(){
        for(int i = 0; i<plannedDirections.Count; i++){
           Vector2 step = plannedDirections[i];
            if(!Move(step, true)){
                //end
                i = plannedDirections.Count;
                continue;
            }
            yield return new WaitForSeconds(movementDelay/2+0.01f);
        }
        plannedDirections.Clear();
        for(int i = 0; i<lines.Count;)
        {
            Destroy(lines[0].gameObject);
            lines.Remove(lines[0]);
        } 
    }

    public bool Move(Vector2 dir, bool instant = false, bool allowOverlap = false){
        Vector3Int currentCell = tilemap.WorldToCell((Vector2)transform.position);
        Vector3Int nextCell = tilemap.WorldToCell((Vector2)transform.position) + new Vector3Int((int)dir.x, (int)dir.y, 0);
        GameObject foundObj = ChimeraController.Instance.FindObjectOnTile(nextCell);
        
        bool canMoveIntoSpot = CanMove(foundObj, allowOverlap);
        Leader leader = GetComponent<Leader>();
        
        if(canMoveIntoSpot && !isMoving){
            movingAction = StartCoroutine(Moving(dir,instant));

           
            if(leader != null){
                leader.OnMoved(currentCell);
            }

            return true;
        } else if(!CanMove(foundObj, allowOverlap) && !isMoving){
            StartCoroutine(Blocked(dir,instant));

            if(this.GetComponent<PlayerMovement>() != null){
                //this is the beast, allow other actions
                PlayerInteraction(foundObj);
            }
        }
        return false;
    }

    public void ForceCompleteMove(){
        //bug from the demo: player animation delays can result in overlapping on tiles
        //to fix, make sure we complete anims before the timer actions happen
         if(movingAction != null){
            StopCoroutine(movingAction);
            transform.position = tilemap.GetCellCenterWorld(cellToMoveTo);
            isMoving = false;
         } 
    }

    public bool CanMove(GameObject obj, bool allowOverlap = false){
        return obj == null || (allowOverlap || obj.GetComponent<Blocker>() == null);
    }

    public void PlayerInteraction(GameObject hitObject){
        if(hitObject.GetComponent<Defender>()){
            this.GetComponent<Attacker>().Attack(hitObject);
        } else {
            ChimeraController.Instance.PlaySFX("Block");
        }
    }


    
    IEnumerator Moving(Vector2 dir, bool instant)
    {
        if(!instant){
           // ChimeraController.Instance.PlaySFX("Move", 0.2f);
        }

       isMoving = true;
       float origZ = transform.position.z;
       Vector3Int origPosition = tilemap.WorldToCell(transform.position);

       cellToMoveTo = tilemap.WorldToCell((Vector2)transform.position) + new Vector3Int((int)dir.x, (int)dir.y, 0);

       float elapsedTime = 0;
        while(elapsedTime < movementDelay){
            transform.position = Vector2.Lerp(tilemap.GetCellCenterWorld(origPosition), tilemap.GetCellCenterWorld(cellToMoveTo), elapsedTime/movementDelay);
            elapsedTime += Time.deltaTime * (instant?2:1);
            yield return null;
        }

      transform.position = tilemap.GetCellCenterWorld(cellToMoveTo);
       isMoving = false;
       yield return null;
    }


    IEnumerator Blocked(Vector2 dir, bool instant)
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
            elapsedTime += Time.deltaTime * (instant?2:1);
            yield return null;
        }

      transform.position = tilemap.GetCellCenterWorld(origPosition);
       isMoving = false;
       yield return null;
    }


}
