using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RPG {
  public class EndCinematic : MonoBehaviour {

    // Animators
    private Animator Anim;
    private Animator PlayerAnim;

    // Game Objects
    private GameObject Player;
    public GameObject Girl;

    // Managers
    private NPC_Manager GNPC;
    private Player_Manager PM;

    // Waypoints
    private Transform girlPoint1;
    private Transform playerPoint1;
    private Transform triforcePoint;

    private Transform target;  // current thing to walk to
    private Transform playerTarget;  // current thing for Player to walk to

    // Flags
    public bool sceneStarted = false;

    // sound
    public AudioSource Music;
    public AudioClip EndMusic;

    private SceneChanger SC;
    public ScreenFader SF;

    // Use this for initialization
    void Awake() {
      Girl = (GameObject)Instantiate(Resources.Load("Pink_Hair_Girl"), new Vector2(32f, -218f), Quaternion.identity);
      Girl.name = "Z";
      Music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
    }

    void Start() {
      SC = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
      /* Initialize Player Stuff */
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      PM.canAttack = false;
      PM.canSwordAttack = false;
      PM.canBombAttack = false;
      PM.canBowAttack = false;
      SF = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();

      /* CRUCIAL CHECK: Are we spawning from 'New Game' or not */
      PlayerAnim = Player.GetComponent<Animator>();

      /* Initialize Girl Stuff */
      GNPC = Girl.GetComponent<NPC_Manager>();
      Anim = Girl.GetComponent<Animator>();
      Anim.SetFloat("direction_x", 0f);
      Anim.SetFloat("direction_y", -1f);
      girlPoint1 = GameObject.Find("GirlWaypoint1").GetComponent<Transform>();
      playerPoint1 = GameObject.Find("PlayerPoint1").GetComponent<Transform>();
      triforcePoint = GameObject.Find("TriforcePoint").GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update() {
      if ((PM.triforceCount == 3) && !sceneStarted) {
        sceneStarted = true;
        StartCoroutine(InitiateScene());
      }
    }

    IEnumerator InitiateScene() {

      yield return StartCoroutine(SF.FadeToBlack());
      Player.transform.position = playerPoint1.transform.position;
      PlayerAnim.SetFloat("input_x", 0f);
      PlayerAnim.SetFloat("input_y", 1f);
      PM.canMove = false;
      yield return new WaitForSeconds(2);

      Music.clip = EndMusic;
      Music.Play();
      Girl.transform.position = girlPoint1.transform.position;
      GameObject.Instantiate(Resources.Load("Triforce"), triforcePoint.position, Quaternion.identity);

      yield return StartCoroutine(SF.FadeToClear());
      GNPC.DialogueScript.ShowBox(GNPC.name, GNPC.Dialogue[0].text, GNPC.GetComponent<SpriteRenderer>().sprite);
      GNPC.NPCExternal.clip = GNPC.DialogueScript.DialogueDoneSound;
      GNPC.NPCExternal.Play();
      GNPC.DialogueCount = 1;
      StartCoroutine(WaitForKeyDown(KeyCode.Space));
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode) {
      while (!Input.GetKeyDown(keyCode))
        yield return null;
      GNPC.DialogueScript.HideBox();
      yield return new WaitForSeconds(5);
      SC.nextScene = "Congratulations";
      // the next spawn location is derived from flipping the current name around (see chooseSpawn function above)
      SC.nextSpawn = "PlayerSpawnPoint";
      // call the built-in scene switcher
      SceneManager.LoadScene(SC.nextScene);
      // this will trigger a warp in the SceneChanger 
      SC.currentScene = SC.nextScene;
    }
  }
}
