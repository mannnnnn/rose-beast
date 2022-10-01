using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsController : MonoBehaviour
{

    public List<WallBounds> bounds = new List<WallBounds>();
    public int unlockedBounds = 1;

    public void ExpandBounds()
    {
        unlockedBounds++;
        bounds[unlockedBounds-1].gameObject.SetActive(true);
        bounds[unlockedBounds-2].RemoveBounds();
    }
}
