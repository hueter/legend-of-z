using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG {

  public class SceneChanger : MonoBehaviour {


    private GameObject Player;
    private GameObject NextSpawnPoint;
    private static bool created = false;
    public string currentScene;
    public string nextScene = "Village";
    public string nextSpawn = "VillageSpawnPoint";
    private bool sceneJustChanged;
    private Zoning Z;


    void Awake() {
      if (!created) {
        DontDestroyOnLoad(this.gameObject);
        created = true;
      } else
        Destroy(this.gameObject);
    }

    void Start() {
      Player = GameObject.Find("Player");
      currentScene = SceneManager.GetActiveScene().name;
    }

    /* 
     * OnRenderObject gets called every time a scene loads and is much more useful
     * than Update() when dealing with scene changes because it won't get called a million times
     */
    void OnRenderObject() {

      // Identify the current scene by name
      currentScene = SceneManager.GetActiveScene().name;

      // Current == Next Scene indicates we need to spawn the player somewhere
      if (currentScene == nextScene) {
        // Debug.Log(nextSpawn);
        NextSpawnPoint = GameObject.Find(nextSpawn);
        // Debug.Log(NextSpawnPoint);

        nextScene = ""; // nullify next scene so this doesn't get called again
        spawnPlayerInNewScene(Player, NextSpawnPoint); // actually move the player to the spawn point
        sceneJustChanged = true;
      }
    }

    void Update() {
      if (sceneJustChanged) {
        sceneJustChanged = false;
        Z = GameObject.Find("ZoneText").GetComponent<Zoning>();
        Z.displayText = true;
      }
    }

    private void spawnPlayerInNewScene(GameObject Player, GameObject PlayerSpawnPoint) {
      // Debug.Log(PlayerSpawnPoint.transform.position.x);
      // Debug.Log(PlayerSpawnPoint.transform.position.y);

      Player.transform.position = PlayerSpawnPoint.transform.position;
    }
  }
}