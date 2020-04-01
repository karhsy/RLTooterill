using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
      public int maximum;
      public int minimum;

      public Count (int min, int max)
      {
        minimum = min;
        maximum = max;
      }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count (5,9);
    public Count foodCount = new Count (1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    // Note to self: List is "fancy" and array[] syntax is primitive
    private List <Vector3> gridPositions = new List <Vector3> ();

    void InitializeList()
    {
      gridPositions.Clear();
      for (int x = 1; x < columns - 1; x++)
      {
        for (int y = 1; y < rows - 1; y++)
        {
          gridPositions.Add(new Vector3(x,y,0f));
        }
      }
      Debug.Log(string.Format("Created a list of grid positions: {0}", gridPositions));
    }

    void BoardSetup()
    {
      boardHolder = new GameObject("Board").transform;
      for (int x = -1; x < columns + 1; x++)
      {
        for (int y = -1; y < rows + 1; y++)
        {
          GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
          if ((x == -1) || (y == -1) || (y == rows) || (x == rows))
            toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

          GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;

          instance.transform.SetParent(boardHolder);
        }
      }
    }

    Vector3 RandomPosition()
    {
      int gridPositionsPosition = Random.Range(0,gridPositions.Count);
      Vector3 position = gridPositions[gridPositionsPosition];
      // Prevent multiple things from spawning on same position
      gridPositions.RemoveAt(gridPositionsPosition);
      return position;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max )
    {
      int objectCount = Random.Range(min, max+1);
      for (int i = 0; i < objectCount; i++)
      {
        Vector3 position = RandomPosition();
        GameObject tile = tileArray[Random.Range(0, tileArray.Length)];
        Instantiate(tile, position, Quaternion.identity);
      }
      Debug.Log(string.Format("Instanced {0} items from {1}.", objectCount, tileArray));
    }

    public void SetupScene(int level)
    {
      BoardSetup();
      InitializeList();
      LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
      LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
      int enemyCount = (int)Mathf.Log(level, 2f);
      Debug.Log(string.Format("Instancing {0} enemies.", enemyCount));
      LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
      Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
    }
}
