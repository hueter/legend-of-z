using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG {
  public class TitleMenu : MonoBehaviour {

    private SceneChanger SC;
    private Player_Manager PM;
    private AudioSource ButtonSound;
    public AudioClip StartSound;
    public AudioClip SelectSound;

    Button NewGameButton;

    Button ForestButton;
    Button ForestDungeonButton;

    Button RiverButton;
    Button RiverDungeonButton;

    Button CastleButton;
    Button CastleDungeonButton;

    Button QuitButton;

    // Use this for initialization
    void Start() {

      PM = GameObject.Find("Player").GetComponent<Player_Manager>();
      SC = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
      ButtonSound = PM.PlayerInternalAudio;

      NewGameButton = GameObject.Find("New Game").GetComponent<Button>();

      ForestButton = GameObject.Find("ForestButton").GetComponent<Button>();
      ForestDungeonButton = GameObject.Find("ForestDungeonButton").GetComponent<Button>();

      RiverButton = GameObject.Find("RiverButton").GetComponent<Button>();
      RiverDungeonButton = GameObject.Find("RiverDungeonButton").GetComponent<Button>();

      CastleButton = GameObject.Find("CastleButton").GetComponent<Button>();
      CastleDungeonButton = GameObject.Find("CastleDungeonButton").GetComponent<Button>();

      QuitButton = GameObject.Find("Quit").GetComponent<Button>();

      //when the corresponding buttons are pressed the appropriate action needs to take place

      NewGameButton.onClick.AddListener(StartGame);
      ForestButton.onClick.AddListener(StartForest);
      ForestDungeonButton.onClick.AddListener(StartForestDungeon);
      RiverButton.onClick.AddListener(StartRiver);
      RiverDungeonButton.onClick.AddListener(StartRiverDungeon);
      CastleButton.onClick.AddListener(StartCastle);
      CastleDungeonButton.onClick.AddListener(StartCastleDungeon);
      QuitButton.onClick.AddListener(QuitGame);

    }

    // Update is called once per frame
    void Update() {
    }

    //Goes directly to the first scene
    public void StartGame() {
      ButtonSound.clip = StartSound;
      ButtonSound.Play();
      PM.StartWithIntro = true;
      SceneManager.LoadScene("Village");
    }

    //Goes to new scene with level select options
    public void StartForest() {
      ButtonSound.clip = SelectSound;
      ButtonSound.Play();
      SC.nextScene = "Forest";
      SC.nextSpawn = "ForestSpawnPoint";
      PM.canSwordAttack = true;
      SceneManager.LoadScene("Forest");
      Debug.Log("Loading Forest Level...");
    }

    public void StartForestDungeon() {
      ButtonSound.clip = SelectSound;
      ButtonSound.Play();
      SC.nextScene = "ForestDungeon";
      SC.nextSpawn = "ForestDungeonSpawnPoint";
      PM.canSwordAttack = true;
      SceneManager.LoadScene("ForestDungeon");
      Debug.Log("Loading ForestDungeon Level...");
    }

    public void StartRiver() {
      ButtonSound.clip = SelectSound;
      ButtonSound.Play();
      SC.nextScene = "River";
      SC.nextSpawn = "RiverSpawnPoint";
      PM.canSwordAttack = true;
      PM.canBowAttack = true;
      PM.triforceCount = 1;
      PM.attackUpgradeNumber = 1;
      PM.speedUpgradeNumber = 1;
      PM.healthUpgradeNumber = 1;

      PM.maxHealth = 6;
      PM.currentHealth = PM.maxHealth;
      PM.damage = 2;
      PM.currentMoveSpeed = 4f;
      SceneManager.LoadScene("River");
      Debug.Log("Loading River Level...");
    }

    public void StartRiverDungeon() {
      ButtonSound.clip = SelectSound;
      ButtonSound.Play();
      SC.nextScene = "RiverCave";
      SC.nextSpawn = "RiverDungeonSpawnPoint";
      PM.canSwordAttack = true;
      PM.canBowAttack = true;
      SceneManager.LoadScene("RiverCave");
      Debug.Log("Loading RiverCave Level...");
      PM.triforceCount = 1;
      PM.attackUpgradeNumber = 1;
      PM.speedUpgradeNumber = 1;
      PM.healthUpgradeNumber = 1;
      PM.maxHealth = 6;
      PM.currentHealth = PM.maxHealth;
      PM.damage = 2;
      PM.currentMoveSpeed = 4f;
    }

    public void StartCastle() {
      ButtonSound.clip = SelectSound;
      ButtonSound.Play();
      SC.nextScene = "Castle";
      SC.nextSpawn = "CastleSpawnPoint";
      PM.canSwordAttack = true;
      PM.canBowAttack = true;
      PM.canBombAttack = true;
      SceneManager.LoadScene("Castle");
      Debug.Log("Loading Castle Level...");
      PM.triforceCount = 2;
      PM.attackUpgradeNumber = 2;
      PM.speedUpgradeNumber = 2;
      PM.healthUpgradeNumber = 2;

      //should be fully upgraded at this.
      PM.maxHealth = 7;
      PM.currentHealth = PM.maxHealth;
      PM.damage = 3;
      PM.currentMoveSpeed = 4.5f;
    }

    public void StartCastleDungeon() {
      ButtonSound.clip = SelectSound;
      ButtonSound.Play();
      SC.nextScene = "CastleDungeon";
      SC.nextSpawn = "CastleDungeonSpawnPoint";
      PM.canSwordAttack = true;
      PM.canBowAttack = true;
      PM.canBombAttack = true;
      SceneManager.LoadScene("CastleDungeon");
      Debug.Log("Loading Castle Dungeon Level...");
      PM.triforceCount = 2;
      PM.attackUpgradeNumber = 2;
      PM.speedUpgradeNumber = 2;
      PM.healthUpgradeNumber = 2;

      PM.maxHealth = 7;
      PM.currentHealth = PM.maxHealth;
      PM.damage = 3;
      PM.currentMoveSpeed = 4.5f;
    }

    //This button will only work when the game is built and run. The editor game Player will not allow Application.Quit() to work
    public void QuitGame() {
      Application.Quit();
    }
  }
}

