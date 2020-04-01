using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyManger : MonoBehaviour
{
  public float turnDelay = 0.1f;
  public float levelStartDelay = 2f;
  public static MyManger instance = null;
  public BoardManager boardScript;
  public int playerFoodPoints = 100;
  [HideInInspector] public bool playersTurn = true;

  private Text levelText;
  private GameObject levelImage;
  private bool doingSetup;

  private int level = 0;
  private List<Enemy> enemies;
  private bool enemiesMoving;

  // BLOCK from docs
  //This is called each time a scene is loaded.
  void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
  {
  //Add one to our level number.
  level++;
  //Call InitGame to initialize our level.
  InitGame();
  }

  void OnEnable()
  {
  SceneManager.sceneLoaded += OnLevelFinishedLoading;
  }

  void OnDisable()
  {
  SceneManager.sceneLoaded -= OnLevelFinishedLoading;
  }
  // END BLOCK from docs

  void Awake()
  {
    if (instance==null)
      instance = this;
    else if (instance!=this)
      Destroy(gameObject);
    DontDestroyOnLoad(gameObject);
    enemies = new List<Enemy>();
    boardScript = GetComponent<BoardManager>();
  }

  void InitGame()
  {
    doingSetup = true;
    levelImage = GameObject.Find("LevelImage");
    levelText = GameObject.Find("LevelText").GetComponent<Text>();
    levelText.text = "Day " + level;
    levelImage.SetActive(true);
    Invoke("HideLevelImage", levelStartDelay);

    enemies.Clear();
    boardScript.SetupScene(level);
  }
  public void HideLevelImage()
  {
    levelImage.SetActive(false);
    doingSetup = false;
  }

  public void GameOver()
  {
    levelText.text = "After " + level + " days, you starved.";
    levelImage.SetActive(true);
    enabled = false;
  }

  IEnumerator MoveEnemies()
  {
    enemiesMoving = true;
    yield return new WaitForSeconds(turnDelay);
    if (enemies.Count == 0)
    {
      yield return new WaitForSeconds(turnDelay);
    }
    for (int i = 0; i < enemies.Count; i++)
    {
      enemies[i].MoveEnemy();
      yield return new WaitForSeconds(enemies[i].moveTime);
    }
    playersTurn = true;
    enemiesMoving = false;
  }

  public void AddEnemyToList(Enemy script)
  {
    enemies.Add(script);
  }

  // Update is called once per frame
  void Update()
  {
    if (playersTurn || enemiesMoving || doingSetup)
      return;
    StartCoroutine(MoveEnemies());
  }

}
