using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLoader : MonoBehaviour
{
    public GameObject gameManager;
    // Start is called before the first frame update
    void Awake()
    {
      if(MyManger.instance==null)
        Instantiate(gameManager);
    }
}
