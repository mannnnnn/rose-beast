using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class ChimeraController : MonoBehaviour
{
    public bool debug = false;
    
    public TextMeshPro TimerLabel;
    public TextMeshPro RetryLabel;
    public int Timer = 10;
     
    public bool gameRunning = true;
    private Tilemap tilemap;
    public GameObject instructions;
    
    public static ChimeraController Instance;
    public GameObject MovementLine;
    public GameObject AttackZone;

    public List<Vector3Int> MoverPaths = new List<Vector3Int>(); 
    private Coroutine runningTimer;

    void Awake()
    {
       Instance = this;
    }
       
    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
        TimerLabel.text = Timer.ToString().PadLeft(2);
        runningTimer = StartCoroutine(GameTimer());
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
        //let all creatures attack if primed
        Attacker[] attackers = FindObjectsOfType<Attacker>();
        foreach(Attacker attacker in attackers){
            if(attacker != null) {
                attacker.AttackWithinRange();
            }
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
       
        //let all creatures show their attack range
        attackers = FindObjectsOfType<Attacker>();
        foreach(Attacker attacker in attackers){
            if(attacker != null) attacker.ShowRange();
        }


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

        Collider2D col = Physics2D.OverlapCircle(tilemap.GetCellCenterWorld(tile), 0);

        if(col == null){
            return null;
        } else {
            return col.gameObject;
        }
    }

    public void GameOver(){
        StartCoroutine(FinishGame());
    }

    IEnumerator FinishGame(){
        if(runningTimer != null) StopCoroutine(runningTimer);
        TimerLabel.text = "GAMEOVER";
        PlayerMovement beast = FindObjectOfType<PlayerMovement>();
        if(beast != null) beast.CanMove = false;
        RetryLabel.gameObject.SetActive(true);


        yield return new WaitForSeconds(1);
        gameRunning = false;

    }

    public void StartOver(){
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        
    }
}
