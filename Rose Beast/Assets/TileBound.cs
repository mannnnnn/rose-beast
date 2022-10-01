using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBound : MonoBehaviour
{ 
    private Tilemap tilemap;
    void Start()
    {
         tilemap = FindObjectOfType<Tilemap>();
         transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(this.transform.position));
    }

    
}
