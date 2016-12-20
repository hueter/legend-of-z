using UnityEngine;
using System.Collections;

namespace RPG {

  public class EnemySpawner : MonoBehaviour {

    //Pick a few spots in the room you want Enemies to spawn from 
    public Vector2[] spawnPoints;
    //the types of Enemies you want. Specify via inspector
    public GameObject[] Enemies;
    //keeps track of Enemies after they spawned
    public GameObject[] AliveEnemies;
    // we dont want the wave to keep spawning. just once
    public bool waveSpawned = false;
    // we need to check to make sure everything is clear so we can stop updating
    public bool roomCleared = false;
    //the spawner works based on the location of the Player
    public Player_Manager PlayerManager;
    //represents the object the Player warps to upon entering the room. Assign this via the inspector
    public GameObject WarpTarget;
    //the Prefab used for the doors
    public GameObject Door;
    //these represent the doors that spawn as soon as Player enters the room
    public GameObject NewDoorNorth;
    public GameObject NewDoorSouth;

    //Chest 
    public GameObject Chest;

    public AudioClip ChestSound;

    private GameObject MiniBossMusicTrigger;

    //tracker of Enemies. should be equal to number of spawn points
    public int number;

    // Use this for initialization
    void Start() {
      PlayerManager = GameObject.Find("Player").GetComponent<Player_Manager>();
      Chest = GameObject.Find("ChestBow");
      Chest.SetActive(false);
      number = spawnPoints.Length;
      //Debug.Log(number);
      AliveEnemies = new GameObject[number];
      MiniBossMusicTrigger = GameObject.Find("MiniBossMusicTrigger");
    }

    // Update is called once per frame
    void Update() {
      //keep updating this value
      // number = AliveEnemies.Length;
      if (!waveSpawned) {
        spawnEnemies();
      }
      if (!roomCleared) {
        enemyCheck();
      }
    }

    protected void spawnEnemies() {
      if (PlayerManager.transform.position == WarpTarget.transform.position && waveSpawned == false) {
        //spawn the two doors via instantiate
        NewDoorNorth = (GameObject)Instantiate(Door, new Vector2(9.92f, -54.19f), Quaternion.identity);
        NewDoorSouth = (GameObject)Instantiate(Door, new Vector2(9.07f, -75.25f), Quaternion.identity);
        NewDoorSouth.GetComponent<SpriteRenderer>().flipY = true;

        //spawn the 4 Enemies via instantiate and the locations specified 
        for (int i = 0; i < Enemies.Length; i++) {
          //Debug.Log(Enemies.Length);
          GameObject newEnemy = (GameObject)Instantiate(Enemies[i], spawnPoints[i], Quaternion.identity);
          AliveEnemies[i] = newEnemy;
        }
        waveSpawned = true;
      }
    }

    protected void enemyCheck() {

      if (waveSpawned) {
        int deathCounter = 0;
        for (int i = 0; i < AliveEnemies.Length; i++) {
          //Debug.Log(deathCounter);
          //Debug.Log("Enemy Health: " + AliveEnemies[i].GetComponent<Enemy_Manager>().currentHealth);
          //using Enemies[i] == null or Enemies[i].Equals(null) doesnt work because the array will always hold them
          // thus it is better to just check if their Health is 0
          if (AliveEnemies[i].Equals(null) || AliveEnemies[i] == null) {

            deathCounter++;
            //Debug.Log("Death counter increased: " + deathCounter);

          }

          if (deathCounter == 4) {
            //Debug.Log("Destroying the doors");
            Destroy(NewDoorNorth);
            Destroy(NewDoorSouth);
            enemiesKilled();
          }
        }
      }
    }

    protected void enemiesKilled() {
      //spawn the Chest and play GetItemSound
      PlayerManager.PlayerExternalAudio.clip = ChestSound;
      PlayerManager.PlayerExternalAudio.Play();
      Chest.SetActive(true);
      roomCleared = true;
      MiniBossMusicTrigger.GetComponent<ChangeBackgroundMusic>().RevertBackToOldMusic();
    }
  }
}