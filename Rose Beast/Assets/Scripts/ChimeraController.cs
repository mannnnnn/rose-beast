using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChimeraController : MonoBehaviour
{
    public bool debug = false;
    public bool noTimer = false;
    
    public TextMeshPro TimerLabel;
    public TextMeshPro RetryLabel;
    public TextMeshPro RoselordLabel;
    public GameObject RoselordEnd;
    public int Timer = 10;
     
    public bool gameRunning = true;
    private Tilemap tilemap;
    public GameObject instructions;
    
    public static ChimeraController Instance;
    public GameObject MovementLine;
    public GameObject Arrowhead;
    public GameObject AttackZone;

    public List<Vector3Int> MoverPaths = new List<Vector3Int>(); 
    private Coroutine runningTimer;
    public GameObject sfxPrefab;
    public SoundLookup soundLookup;

    public List<Vector3Int> ReservedSpawns = new List<Vector3Int>(); 

    private PlayerMovement playerMovement;

    void Awake()
    {
       Instance = this;
    }
       
    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
        TimerLabel.text = Timer.ToString().PadLeft(2);
        runningTimer = StartCoroutine(GameTimer());

        playerMovement = FindObjectOfType<PlayerMovement>();

       //Plan next move
        Mover[] movers = FindObjectsOfType<Mover>();
        foreach(Mover mover in movers){
            mover.PlanMove();
        }
    }

    IEnumerator GameTimer()
    {
        while(gameRunning && !noTimer){

            if(Timer <= 0){
                Timer = 11;
                StartCoroutine(TimesUp());
                ChimeraController.Instance.PlaySFX("Tock");
            } else {
                ChimeraController.Instance.PlaySFX("Tick");
            }
            yield return new WaitForSeconds(1);
            Timer--;
            TimerLabel.text = Timer.ToString().PadLeft(2,'0');
        }
        
    }

    public IEnumerator TimesUp(){

        //complete player movement to avoid overlapping tiles
        if(playerMovement != null){
            playerMovement.mover.ForceCompleteMove();
            playerMovement.CanMove = false;
        }

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
        ReservedSpawns.Clear();

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

        //reenable player movement
        if(playerMovement != null && gameRunning == true){
            playerMovement.CanMove = true;
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
        TimerLabel.text = "GAMEOVER";
    }

    public void WinGame(){
        StartCoroutine(FinishGame());
        TimerLabel.text = "VICTORY!";

        if(FindObjectOfType<RoseLord>() != null){
            RoselordEnd.SetActive(true);
        }
    }

    IEnumerator FinishGame(){
        RoselordLabel.gameObject.SetActive(false);
        if(runningTimer != null) StopCoroutine(runningTimer);
        
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


    public void PlaySFX(string clip){
        PlaySFX(clip, -1, float.MaxValue);
    }
    
    public void PlaySFX(string clip, float volume){
        PlaySFX(clip, volume, float.MaxValue);
    }

     public void PlaySFX(string clip, float volume, float pitch){
        GameObject obj = Instantiate(sfxPrefab, GameObject.FindGameObjectWithTag("MusicBox").transform);
        obj.name = "SFX - " + clip;
        AudioSource audioSource = obj.GetComponent<AudioSource>();
        SoundLookup.Sound snd = soundLookup.GetSound(clip);
        if (snd == null)
        {
            Debug.Log($"Warning: Sound Effect {clip} is not registered.");
            return;
        }
        audioSource.clip = snd.clip;

        if(volume > 0){
            audioSource.volume = volume * snd.volume;
        }

        if(pitch < float.MaxValue){
            audioSource.pitch = pitch;
        }

        audioSource.Play();
        Destroy(audioSource.gameObject,  audioSource.clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    } 

    public void RoselordSpawn(){
        RoselordLabel.gameObject.SetActive(true);
    }
}
