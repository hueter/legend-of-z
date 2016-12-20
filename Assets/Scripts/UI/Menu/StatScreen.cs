using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {

  public class StatScreen : MonoBehaviour {

    private Animator PlayerAnim;
    public GameObject HudView;
    public GameObject StatsView;
    public GameObject Player;
    private AudioSource PlayerAudio;
    public bool screenOpen;
    private Player_Manager PM;

    // Use this for initialization
    void Start() {
      //set up the screen and set to false so it doesnt show right away
      HudView = GameObject.Find("HUD_Stat");
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      PlayerAnim = Player.GetComponent<Animator>();
      PlayerAudio = PM.PlayerInternalAudio;
      StatsView = HudView.transform.GetChild(0).gameObject;
      StatsView.SetActive(false);

      screenOpen = false;
    }

    // Update is called once per frame
    void Update() {

      if (Input.GetKeyDown(KeyCode.C) && !screenOpen) {
        openScreen();
      } else if (Input.GetKeyDown(KeyCode.C) && screenOpen) {
        closeScreen();
        screenOpen = false;
      }
    }


    protected void openScreen() {
      PlayerAudio.clip = PM.toggleStatSound;
      PlayerAudio.Play();
      screenOpen = true;
      //Disable movement script so Player can walk away while looting
      PM.canMove = false;
      PlayerAnim.SetBool("isWalking", false);

      //Activate Stat Screen
      HudView.transform.GetChild(0).gameObject.SetActive(true);

      //Change text components of children to appropriate attribute 
      StatsView.transform.GetChild(1).GetComponent<Text>().text = "Attack: " + PM.damage;
      StatsView.transform.GetChild(2).GetComponent<Text>().text = "Health: " + PM.currentHealth;
      StatsView.transform.GetChild(3).GetComponent<Text>().text = "Movement Speed: " + PM.currentMoveSpeed;
      StatsView.transform.GetChild(4).GetComponent<Text>().text = "Rupees: " + PM.rupees;
      StatsView.transform.GetChild(5).GetComponent<Text>().text = "Triforce Pieces: " + PM.triforceCount;
    }

    protected void closeScreen() {
      PlayerAudio.clip = PM.toggleStatSound;
      PlayerAudio.Play();
      StatsView.SetActive(false);
      PM.canMove = true;
    }
  }
}