using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG {
  public class TwoWaySceneWarp : MonoBehaviour {

    // We will need to access the SceneChanger and Player Manager objects
    private SceneChanger SC;
    private GameObject Player;
    private Player_Manager PlayerManager;

    // Use this for initialization
    void Start() {
      SC = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
      Player = GameObject.Find("Player");
      PlayerManager = Player.GetComponent<Player_Manager>();
    }

    // Update is called once per frame
    void Update() {
    }

    /*
     * This function reverses the name of the exit point to get the entrance point of the next zone.
     * For example, "VillageToForest" warps to "ForestToVillage".
     * You MUST name each SceneChangeTrigger Prefab to conform to this standard.
     * Also, you MUST tag each SceneChangeTrigger to be the scene that it warps to.
     */
    private string chooseSpawn(string exitPoint) {
      // split the string into an array of strings on the word "To"
      string[] strArr = exitPoint.Split(new string[] { "To" }, System.StringSplitOptions.None);
      // rebuild the string with string interpolation from "AToB" -> "BToA"
      string enterPoint = string.Format("{0}To{1}", strArr[1], strArr[0]);

      // Debug.Log(enterPoint);

      return enterPoint;
    }

    private void OnTriggerEnter2D(Collider2D ExitBox) {
      // When a player enters an exit box and they didn't "just" arrive on the scene
      if (!PlayerManager.IJustGotHere) {
        if (ExitBox.gameObject.CompareTag("Player")) {
          // the next scene is based on the SceneChangeTrigger tag, as noted above
          SC.nextScene = gameObject.tag; 
          // the next spawn location is derived from flipping the current name around (see chooseSpawn function above)
          SC.nextSpawn = chooseSpawn(gameObject.name);

          // hey bro I just got here can you give me a minute before you warp me back
          PlayerManager.IJustGotHere = true;
          // call the built-in scene switcher
          SC.currentScene = SC.nextScene;

          SceneManager.LoadScene(SC.nextScene);

          // this will trigger a warp in the SceneChanger 
        }
      }
    }

    private void OnTriggerExit2D(Collider2D ExitBox) {
      // Once you step out of the landing box, it's OK to go back again. 
      PlayerManager.IJustGotHere = false;
    }
  }
}

