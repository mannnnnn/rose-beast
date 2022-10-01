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
                TimesUp();
            }
            yield return new WaitForSeconds(1);
            Timer--;
            TimerLabel.text = Timer.ToString().PadLeft(2,'0');
        }
        
    }

    public void TimesUp(){
        foreach(TileBound tile in FindObjectsOfType<TileBound>()){
            UnityEngine.Debug.Log("Tile aged" + tile.name);
            tile.age++;
        }
        UnityEngine.Debug.Log("Go!");
    }

    public GameObject FindObjectOnTile(Vector3 myPosition, Vector2 dir){

        Vector3Int nextCell = tilemap.WorldToCell((Vector2)transform.position) + new Vector3Int((int)dir.x, (int)dir.y, 0);
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
}
