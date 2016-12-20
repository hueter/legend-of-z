using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {

  public class NPC_Manager : Character {

    private Animator Anim;
    private Animator PlayerAnim;

    //public bool interactable;
    public bool interacting;

    //this is where we store all the dialogue. Another option would be to just use strings 
    public Text[] Dialogue;

    public DialogueManager DialogueBox;
    public DialogueManager DialogueScript;
    public int DialogueCount;

    public Player_Manager PlayerManager;

    public NpcPatrol PatrolScript;

    private AudioSource[] NPCAudios;
    public AudioSource NPCInternal;
    public AudioSource NPCExternal;


    // Use this for initialization
    void Start() {
      interacting = false;
      DialogueCount = 0;
      DialogueScript = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
      PlayerManager = GameObject.Find("Player").GetComponent<Player_Manager>();
      PatrolScript = GetComponent<NpcPatrol>();
      NPCAudios = GetComponents<AudioSource>();
      NPCInternal = NPCAudios[0];
      NPCExternal = NPCAudios[1];
    }

    // Update is called once per frame
    void Update() {
      //This is used to keep the dialogue manager going until the end of possible dialogue
      if (interacting == true && Input.GetKeyDown(KeyCode.Space)) {
        //Debug.Log(DialogueCount);
        PlayerInteraction();
      }

      if (PatrolScript != null && interacting == false) {
        PatrolScript.patrol();
      }
    }


    public void PlayerInteraction() {
      Anim = GetComponent<Animator>();
      PlayerAnim = GameObject.Find("Player").GetComponent<Animator>();

      var playerDirection = PlayerAnim.GetFloat("lastMove_x");

      //If there is dialogue available to display
      if (Dialogue.Length != 0) {
        interacting = true;

        if (PatrolScript) {
          PatrolScript.walking = false;
          if (playerDirection != 0) {
            Anim.SetFloat("direction_x", -playerDirection);
            // PatrolScript.lastDirection = -playerDirection;
          }
          Anim.SetBool("isWalking", false);
        }

        //DialogueScript.ShowBox(this.name, Dialogue[DialogueCount].text);

        //if we are not at the end of the dialogue array
        if (DialogueCount < Dialogue.Length) {
          DialogueScript.ShowBox(this.name, Dialogue[DialogueCount].text, this.GetComponent<SpriteRenderer>().sprite);
          NPCExternal.clip = DialogueScript.DialogueNextSound;
          NPCExternal.Play();
          DialogueCount++;
          return;
        } else {

          //End of the dialogue. Reset counter, set interacting to false, hide dialogue box can give control back to Player
          DialogueCount = 0;

          DialogueScript.HideBox();
          NPCExternal.clip = DialogueScript.DialogueDoneSound;
          NPCExternal.Play();
          PlayerManager.canMove = true;
          PlayerManager.canAttack = true;
          PlayerManager.currentlyInteracting = false;

          interacting = false;

          return;
        
        }
      }
    }
  }
}