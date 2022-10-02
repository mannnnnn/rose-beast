using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Attacker : MonoBehaviour
{
    public int DamageAmt = 1;
    public int AttackRange = 1;

    private TileBound tile;
    private Tilemap tileMap;

    public bool eatsMeat = false;

    private List<GameObject> attackZones = new List<GameObject>();

    void Start(){
        tile = GetComponent<TileBound>();
        tileMap = FindObjectOfType<Tilemap>();
    }

    public void Attack(Vector2 targetDir){
        GameObject obj = ChimeraController.Instance.FindObjectOnTile(transform.position, targetDir);
        Attack(obj);
    }

    public void Attack(GameObject obj){
        if(obj != null){
            Defender defender = obj.GetComponent<Defender>();
            if(defender != null){
                defender.TakeDamage(DamageAmt, this);
            }
        }
    }

    public void ShowRange(){
        if(GetComponent<PlayerMovement>() != null) return;
        Vector3Int myTile = tileMap.WorldToCell(this.transform.position);
        for(int x = -AttackRange; x<=AttackRange; x++){
            for(int y = -AttackRange; y<=AttackRange; y++){
                //walk on all tiles that aren't this one and put down a danger zone
                if(!(x==0 && y==0)){
                    Vector3Int pickedCell = myTile + new Vector3Int(x, y, 0);
                    GameObject attackZone = Instantiate(ChimeraController.Instance.AttackZone,this.transform.parent);
                    attackZone.transform.position = tileMap.GetCellCenterWorld(pickedCell);
                    attackZone.GetComponentInChildren<SpriteRenderer>().color = new Color(tile.unitColor.r, tile.unitColor.g, tile.unitColor.b, 0.05f);
                    attackZones.Add(attackZone);
                }
            }
        }
    }

    public bool AttackWithinRange(){
        if(GetComponent<PlayerMovement>() != null) return false;
        if(attackZones.Count == 0) return false;
         UnityEngine.Debug.Log(this.name + "is attacking within range");
        bool targetInRange = false;
        Vector3Int myTile = tileMap.WorldToCell(this.transform.position);

        for(int x = -AttackRange; x<=AttackRange; x++){
            for(int y = -AttackRange; y<=AttackRange; y++){
               if(!(x==0 && y==0)){
                    Vector3Int pickedCell = myTile + new Vector3Int(x, y, 0);
                     GameObject foundObj = ChimeraController.Instance.FindObjectOnTile(pickedCell);
                    if(IsValidTarget(foundObj)){
                        UnityEngine.Debug.Log(this.name + "found a valid attack target of " + foundObj.name);
                        targetInRange = true;
                        Attack(foundObj);
                    } else if(foundObj!= null){
                        UnityEngine.Debug.Log(this.name + "found a " + foundObj.name +" was not a valid target");
                    }
                }
            }
        }

        if(targetInRange){
            foreach(GameObject attackZone in attackZones){
                attackZone.GetComponentInChildren<SpriteRenderer>().color = Color.red + Color.white/2;
            }

            ClearAttackRange();
            return true;
        } 
        
        ClearAttackRange();
        return false;
    }

    public void OnDestroy(){
        ClearAttackRange();
    }

    public void ClearAttackRange(){
        for(int i = 0; i<attackZones.Count; i++){
                Destroy(attackZones[i]);
        }
         attackZones.Clear();
    }

    public bool IsValidTarget(GameObject foundObj){
        if(foundObj == null) return false;
        if(foundObj.GetComponent<Defender>()!=null){
            if(foundObj.GetComponent<Defender>().meat > 0){
                if(!eatsMeat) return false;
            }
            return true;
        }
        return false;
    }

}
