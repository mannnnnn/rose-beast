using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class ChimeraController : MonoBehaviour
{
    public bool debug = false;
    
    public TextMeshPro TimerLabel;
    public int Timer = 10;
     
    private bool gameRunning = true;
    private Tilemap tilemap;
    public GameObject instructions;
    
    public static ChimeraController Instance;
    public GameObject MovementLine;

    public List<Vector3Int> MoverPaths = new List<Vector3Int>(); 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
       
    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
        TimerLabel.text = Timer.ToString().PadLeft(2);
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while(gameRunning){

            if(Timer <= 0){
                Timer = 11;
                StartCoroutine(TimesUp());
            }
            yield return new WaitForSeconds(1);
            Timer--;
            TimerLabel.text = Timer.ToString().PadLeft(2,'0');
        }
        
    }

    public IEnumerator TimesUp(){
         
        foreach(TileBound tile in FindObjectsOfType<TileBound>()){
            tile.UpdateAge();
        }

        //move all creatures
        Mover[] movers = FindObjectsOfType<Mover>();
        foreach(Mover mover in movers){
            mover.ExecuteMove();
        }

        yield return new WaitForSeconds(0.2f); //let all walks finish (can't multithread in webGL)
        MoverPaths.Clear();

        //grow any creatures
        Grower[] growers = FindObjectsOfType<Grower>();
        foreach(Grower grower in growers){
            grower.AgeChanged();
        }

        yield return new WaitForSeconds(0.1f); //let things grow properly

        //spawn creatures
        Spawner[] spawners = FindObjectsOfType<Spawner>();
        foreach(Spawner spawner in spawners){
            spawner.AgeChanged();
        }
        yield return new WaitForSeconds(0.05f); //let things spawn properly
       
       //Plan next move
       movers = FindObjectsOfType<Mover>(); //this can change, so recall
        foreach(Mover mover in movers){
            mover.PlanMove();
        }
    }

    public GameObject FindObjectOnTile(Vector3 myPosition, Vector2 dir){
        if(tilemap == null){
            tilemap = FindObjectOfType<Tilemap>();
        }
        Vector3Int nextCell = tilemap.WorldToCell((Vector2)myPosition) + new Vector3Int((int)dir.x, (int)dir.y, 0);
        Collider2D col = Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(nextCell), 0);

        if(col == null){
            return null;
        } else {
            return col.gameObject;
        }
    }

    public GameObject FindObjectOnTile(Vector3Int tile){

        Collider2D col = Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(tile), 1);

        if(col == null){
            return null;
        } else {
            return col.gameObject;
        }
    }
}
