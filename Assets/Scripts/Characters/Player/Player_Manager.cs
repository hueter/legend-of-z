using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG {

  public class Player_Manager : Character {

    //buttons used for certain actions
    public KeyCode InteractionKey = KeyCode.E;
    public KeyCode SwordAttack = KeyCode.Space;
    public KeyCode BowAttack = KeyCode.LeftControl;
    public KeyCode BombAttack = KeyCode.Z;

    //debug cheat mode
    public KeyCode GodModeKey = KeyCode.G;
    public bool godMode = false;

    //Variables that control what attacks we can carry out
    public bool canSwordAttack = false;
    public bool canBowAttack = false;
    public bool canBombAttack = false;


    //Directional variables
    public RaycastHit2D direction;

    //for interactions
    public bool currentlyInteracting = false;

    //used for showing items upon opening a Chest
    public GameObject hudItem;

    //Used for collision based events
    private Collider2D PlayerCollider;

    /** Sound and Audio **/
    public AudioSource[] PlayerAudios;
    public AudioSource PlayerInternalAudio;
    public AudioSource PlayerExternalAudio;

    public AudioClip SwordAttackSound;     //Attack SwingSound
    public AudioClip BowAttackSound;     // Bow Attack Sound
    public AudioClip BombAttackSound;
    public AudioClip BombExplodeSound;
    public AudioClip GetItem;
    public AudioClip WarpSound;
    public AudioClip toggleStatSound;


    //Rupee Count
    public int rupees;

    //Triforce Pieces
    public int triforceCount;
    // for scene changed
    public bool IJustGotHere = false;

    //track updates
    public int attackUpgradeNumber;
    public int healthUpgradeNumber;
    public int speedUpgradeNumber;

    // do we start from the beginning
    public bool StartWithIntro = false;
    public bool Dead = false;

    // used for GameOver
    private SceneChanger SC;

    void Awake() {
      PlayerCollider = GetComponent<Collider2D>();
      CharacterAnimator = GetComponent<Animator>();
      PlayerAudios = GetComponents<AudioSource>();
      PlayerInternalAudio = PlayerAudios[0];
      PlayerExternalAudio = PlayerAudios[1];
      //we dont wnat multiple Player objects to exist
      DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start() {
      SC = GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
      currentHealth = maxHealth;
      damage = defaultDamage;
      rupees = 0;
      triforceCount = 0;
      joltAmount = 1000f;
      speedUpgradeNumber = 0;
      attackUpgradeNumber = 0;
      healthUpgradeNumber = 0;
    }

    // Update is called once per frame
    void Update() {
      //We only allow interactions to occur if the Player is currently not interacting 
      //Since the object the Player interacts with will reset currentlyInteracting for us
      // , this conditional should be safe
      if (Input.GetKeyUp(InteractionKey) && currentlyInteracting == false) {
        Interaction();
      }

      if (currentHealth <= 0 && !Dead) {
        Death();
      }

      //God mode to keep health at full capacity.
      if (Input.GetKeyUp(GodModeKey)) {
        if (!godMode) {
          godMode = true;
        } else {
          godMode = false;
        }
      }
      if (godMode) {
        currentHealth = maxHealth;
      }

    }

    private void Death() {
      Dead = true;
      //set Health to 0, stops movement, removes collider to stop errors
      currentHealth = 0;
      PlayerCollider.enabled = false;
      canMove = false;
      canAttack = false;

      PlayerInternalAudio.clip = DieSound;
      PlayerInternalAudio.Play();
 
      SC.nextScene = "GameOver";
      // the next spawn location is derived from flipping the current name around (see chooseSpawn function above)
      SC.nextSpawn = "PlayerSpawnPoint";
      // call the built-in scene switcher
      SceneManager.LoadScene(SC.nextScene);
      // this will trigger a warp in the SceneChanger 
      SC.currentScene = SC.nextScene;
    }


    //if canAttack is false then we cant allow other attacks to currently work


    private void TakeDamage(Character Char) {
      if (Char != null) {
        currentHealth -= Char.damage;

        //we dont die so we need the proper knockback
        hit(Char.transform, Char.joltAmount);
      }
    }


    private void hit(Transform OtherCharacter, float joltAmount) {
      /********************************************************************
       * Plays the hit SwingSound specified. I didnt bother with a SwingSound manager
       * since it really seems overkill
       * ****************************************************************/
      if (!OtherCharacter) {
        return;
      }
      PlayerInternalAudio.clip = TakeDamageSound;
      PlayerInternalAudio.Play();

      //If Character can be knocked back, we make sure it happens
      if (canBeJolted) {
        knockback(OtherCharacter, joltAmount);
      }
    }

    private void knockback(Transform OtherCharacter, float joltAmount) {
      //Get the position of the Character
      Vector2 position = gameObject.transform.position - OtherCharacter.position;

      //Get rigidbody 
      Rigidbody2D CharRigid = gameObject.GetComponent<Rigidbody2D>();

      CharRigid.velocity = Vector3.zero;
      //Apply the knockback force on the rigidbody
      CharRigid.AddForce(position.normalized * joltAmount, ForceMode2D.Impulse);
    }

    public float GetHealth() {
      return currentHealth;
    }

    public void AddHealth(int amount) {
      if (currentHealth + amount >= maxHealth) {
        currentHealth = maxHealth;
      } else
        currentHealth += amount;
    }

    //animation test. 
    public IEnumerator collectItem(Sprite Item) {
      hudItem = GameObject.Find("HUD_Item");
      //Disable movement script so Player can walk away while looting
      GetComponent<PlayerMovement>().enabled = false;

      //Set bool that starts animation
      CharacterAnimator.SetBool("isCollecting", true);

      //Activate SwingSound
      PlayerInternalAudio.clip = GetItem;
      PlayerInternalAudio.Play();

      //Activate Item collect UI box
      hudItem.transform.GetChild(0).gameObject.SetActive(true);
      var Item_Container = hudItem.transform.GetChild(0).gameObject;


      //There are two children of ItemContainer: one is the image and the other is the text element
      Item_Container.transform.GetChild(0).GetComponent<Image>().sprite = Item;
      // Debug.Log(Item);
      // Debug.Log(Item.name);
      Item_Container.transform.GetChild(1).GetComponent<Text>().text = Item.name;


      //Wait 2 seconds then reverse what we just did
      yield return new WaitForSeconds(2);
      CharacterAnimator.SetBool("isCollecting", false);
      GetComponent<PlayerMovement>().enabled = true;

      //Disactivate box and set 
      hudItem.transform.GetChild(0).gameObject.SetActive(false);

      //Call function that checks if an ability has been unlocked or a stat needs to be increased
      checkAbilityUnlock(Item.name);
      checkUpgrade(Item.name);

      if (Item.name == "Rupees50")
        rupees += 50;


    }

    protected void checkAbilityUnlock(string name) {
      switch (name) {
        case "Sword":
          canSwordAttack = true;
          break;
        case "Bow":
          canBowAttack = true;
          break;
        case "Bomb":
          canBombAttack = true;
          break;
      }
    }


    //Checks the name of the Item passed in from the collectItem function to see if stats should be increased. 
    protected void checkUpgrade(string name) {
      switch (name) {
        case "AttackIncrease":
          attackUpgradeNumber++;
          damage++;
          break;
        case "HealthIncrease":
          healthUpgradeNumber++;
          maxHealth++;
          break;
        case "SpeedIncrease":
          speedUpgradeNumber++;
          currentMoveSpeed = currentMoveSpeed + .5f;
          break;

      }
    }


    public void Interaction() {
      //Debug.Log("inside interaction");

      //This will make a Raycast at a Target in front of the Player with a distance of 1f (so basically the interacting object has to be close)
      if (CharacterAnimator.GetFloat("input_x") != 0) {
        direction = Physics2D.Raycast(transform.position, new Vector2(CharacterAnimator.GetFloat("input_x"), 0), 1f);
      } else {
        direction = Physics2D.Raycast(transform.position, new Vector2(0, CharacterAnimator.GetFloat("input_y")), 1f);
      }

      //We can use the collider to find the game object we are interacting with. 
      // There need to be a few checks so the system doesnt interact with unintended objects
      // These include: 1.) it cant be the Player itself     2.) Uninteractable tagged objects which should include Enemies
      if (direction.collider != null && direction.collider.name != "Player" && (direction.collider.tag == "interactable" || direction.collider.tag == "NPC")) {
        //we can call the targets interact stuff here

        //Debug.Log("found a Target");
        currentlyInteracting = true;

        //we shouldnt be allowed to Attack or move during interactions since that would look just weird
        CharacterAnimator.SetBool("isWalking", false);
        canMove = false;
        canAttack = false;

        //send message to raycast Target to interact with us
        direction.collider.gameObject.SendMessage("PlayerInteraction", null, SendMessageOptions.DontRequireReceiver);
      }
    }
  }
}