using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEverything : MonoBehaviour
{
    public GameObject[] myTiles;

    void Awake()
    {
      int x = -myTiles.Length/2;
      int y = 0;
      for (int i=0; i<myTiles.Length; i++)
      {
        // Instantiate the tile at a part of the spiral
        Instantiate(myTiles[i], new Vector3(x, y, 0f), Quaternion.identity);
        Debug.Log(string.Format("Instantiated {0} at {1}, {2} just now.", myTiles[i], x, y), myTiles[i]);
        x++;
      }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
