using UnityEngine;
using System.Collections;

namespace RPG {
  public class IntroCinematic : MonoBehaviour {

    // Animators
    private Animator Anim;
    private Animator PlayerAnim;

    // Game Objects
    private GameObject Player;
    public GameObject Girl;
    private GameObject GirlSpawnPoint;

    // Managers
    private NPC_Manager GNPC;
    private Player_Manager PM;

    // Waypoints
    private Transform girlPoint1;
    private Transform girlPoint2;
    private Transform playerPoint1;
    private Transform target;  // current thing to walk to
    private Transform playerTarget;  // current thing for Player to walk to


    // Flags
    private bool shouldMove = false;
    private bool shouldPlayerMove = false;

  // Use this for initialization
  void Awake() {
      GirlSpawnPoint = GameObject.Find("GirlSpawnPoint");
      Girl = (GameObject)Instantiate(Resources.Load("Pink_Hair_Girl"), GirlSpawnPoint.transform.position, Quaternion.identity);
      Girl.name = "Z";
    }

    void Start() {
      /* Initialize Player Stuff */
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
  
      /* CRUCIAL CHECK: Are we spawning from 'New Game' or not */
      if (PM.StartWithIntro) {
        PlayerAnim = Player.GetComponent<Animator>();
        PM.canMove = false;

        /* Initialize Girl Stuff */
        GNPC = Girl.GetComponent<NPC_Manager>();
        Anim = Girl.GetComponent<Animator>();
        StartCoroutine(InitiateScene());
        girlPoint1 = GameObject.Find("GirlWaypoint1").GetComponent<Transform>();
        girlPoint2 = GameObject.Find("GirlWaypoint2").GetComponent<Transform>();
        playerPoint1 = GameObject.Find("PlayerPoint1").GetComponent<Transform>();
        PM.StartWithIntro = false;
      } else {
        Destroy(GameObject.Find("DoNotExitHouse"));
        Destroy(GameObject.Find("CinematicTrigger"));
        Destroy(Girl);
        Destroy(this.gameObject);
      }
    }
    // Update is called once per frame
    void Update() {
      if (shouldMove) {
        move();
      }
      if (shouldPlayerMove) {
        playerMove();
      }
    }

    IEnumerator InitiateScene() {
      yield return new WaitForSeconds(1);
      wakeUp();
      StartCoroutine(WaitForKeyDown(KeyCode.Space));
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode) {
      while (!Input.GetKeyDown(keyCode))
        yield return null;
      walkToPoint1();
    }

    IEnumerator WaitForMove1() {
      while (Girl.transform.position != target.position)
        yield return null;
      walkToPoint2();
    }

    IEnumerator WaitForMove2() {
      while (Girl.transform.position != target.position)
        yield return null;
      speak();
    }

    private void wakeUp() {
      // get background music to silence it
      GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().Stop();
      /* Say "Link..." and play sound / increment dialogue */
      GNPC.DialogueScript.ShowBox(GNPC.name, GNPC.Dialogue[0].text, GNPC.GetComponent<SpriteRenderer>().sprite);
      GNPC.NPCExternal.clip = GNPC.DialogueScript.DialogueDoneSound;
      GNPC.NPCExternal.Play();
      GNPC.DialogueCount = 1;
    }

    private void walkToPoint1() {
      PlayerAnim.SetFloat("input_x", 0f);
      PlayerAnim.SetFloat("input_y", -1f);
      GNPC.DialogueScript.HideBox();
      Anim.SetBool("isWalking", true);
      target = girlPoint1;
      move();
      StartCoroutine(WaitForMove1());
    }

    private void walkToPoint2() {

      PlayerAnim.SetBool("isWalking", true);
      Anim.SetFloat("direction_x", -1f);
      Anim.SetFloat("direction_y", 0);
      Anim.SetBool("isWalking", true);
      target = girlPoint2;
      playerTarget = playerPoint1;
      move();
      playerMove();
      StartCoroutine(WaitForMove2());
    }


    private void move() {
      shouldMove = true;
      float step = 3f * Time.deltaTime;
      Girl.transform.position = Vector3.MoveTowards(Girl.transform.position, target.position, step);
      if (Girl.transform.position == target.position) {
        shouldMove = false;
        Anim.SetBool("isWalking", false);
      }
    }

    private void playerMove() {
      shouldPlayerMove = true;
      float step = 2f * Time.deltaTime;
      Player.transform.position = Vector3.MoveTowards(Player.transform.position, playerTarget.position, step);
      if (Player.transform.position == playerTarget.position) {
        shouldPlayerMove = false;
        PlayerAnim.SetBool("isWalking", false);
      }
    }

    private void speak() {
      Anim.SetFloat("direction_x", 0f);
      Anim.SetFloat("direction_y", 1f);
    }
  }
}

