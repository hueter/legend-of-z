using UnityEngine;
using UnityEngine.SceneManagement;


namespace RPG {
  public class OneWaySceneWarp : MonoBehaviour {

    private SceneChanger SC;
    private Player_Manager PM;
    private AudioSource PlayerAudio;

    // Use this for initialization
    void Start() {
      PM = GameObject.Find("Player").GetComponent<Player_Manager>();
      PlayerAudio = PM.PlayerInternalAudio;
      SC = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update() {
    }

    private string chooseSpawn(string exitPoint) {
      // split the string into an array of strings on the word "To"
      string[] strArr = exitPoint.Split(new string[] { "To" }, System.StringSplitOptions.None);
      // we want to go to the place specified "To<Place>" which is strArr[1], i.e. the second object in the array
      string enterPoint = strArr[1];

      // Debug.Log(enterPoint);

      return enterPoint;
    }

    private void OnTriggerEnter2D(Collider2D PlayerCollider) {
      // When a player enters an exit box and they didn't "just" arrive on the scene
      if (PlayerCollider.gameObject.CompareTag("Player")) {
        PlayerAudio.clip = PM.WarpSound;
        PlayerAudio.Play();
        // the next scene is based on the SceneChangeTrigger tag, as noted above
        SC.nextScene = gameObject.tag;
        // the next spawn location is derived from flipping the current name around (see chooseSpawn function above)
        SC.nextSpawn = chooseSpawn(gameObject.name);
        // call the built-in scene switcher
        // this will trigger a warp in the SceneChanger 
        SC.currentScene = SC.nextScene;
        SceneManager.LoadScene(SC.nextScene);
      }
    }
  }
}
