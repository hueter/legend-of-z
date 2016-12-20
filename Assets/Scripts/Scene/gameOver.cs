using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace RPG {
  public class gameOver : MonoBehaviour {

    Button ContinueButton;
    Button QuitButton;

    private AudioSource ButtonSound;
    public AudioClip ContinueSound;

    private GameObject Player;
    private Player_Manager PM;

    private SceneChanger SC;


    // Use this for initialization
    void Start() {

      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      SC = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
      ButtonSound = PM.PlayerInternalAudio;
      ContinueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
      QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
      ContinueButton.onClick.AddListener(ContinueGame);
      QuitButton.onClick.AddListener(QuitGame);

    }

    // Update is called once per frame
    void Update() {

    }
    public void ContinueGame() {
      ButtonSound.clip = ContinueSound;
      ButtonSound.Play();
      PM.Dead = false;
      //set Health to 0, stops movement, removes collider to stop errors
      PM.currentHealth = PM.maxHealth;
      PM.GetComponent<Collider2D>().enabled = true;
      PM.canMove = true;
      PM.canAttack = true;
      SC.nextScene = "Village";
      // the next spawn location is derived from flipping the current name around (see chooseSpawn function above)
      SC.nextSpawn = "VillageSpawnPoint";
      // call the built-in scene switcher
      SceneManager.LoadScene(SC.nextScene);
      // this will trigger a warp in the SceneChanger 
      SC.currentScene = SC.nextScene;
    }
    public void QuitGame() {
      Application.Quit();
    }
  }
}

