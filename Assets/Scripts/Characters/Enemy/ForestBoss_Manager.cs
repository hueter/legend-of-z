using UnityEngine;
using System.Collections;

namespace RPG {
  public class ForestBoss_Manager : Character {

    public int aggroDistance;

    //Timer for when boss can attack
    public int attackTimer;

    //Timer for when to switch waypoints
    public int pointTimer;

    //reference to Player
    public GameObject Player;
    public float distance;
    private Rigidbody2D CharRigidBody;

    //these will regulate the bosses possible positions
    public Vector3[] wayPoints;

    //Used by boss to know which wayPoint to move to
    public Vector3 currentPoint;

    //Tracks current position in wayPoint array
    private int pointTracker;

    ////Tracks whether boss is changing positions
    private bool traveling;

    //Tracks whether player has entered the room
    private bool battleStarted;

    //attack tracker
    public int attackTracker;

    //initial fight countdown
    public int countDown = 200;

    //final Attack
    public int finalAttackTracker;

    //Two states which determine which attack in finalAttack to do
    public bool finalAttack1;
    public bool finalAttack2;

    //tracks players last location
    private Vector3 playerLocation;

    // wait for the intro scene
    public bool IntroComplete = false;
    public bool defeated = false;

    // sounds
    public AudioSource[] BossAudios;
    private AudioSource BossAudio1;
    private AudioSource BossAudio2;
    public AudioClip FinalAttackSound1;

    private AudioSource PlayerAudio;

    void Awake() {
      // Assign the Animator Component.
      CharacterAnimator = GetComponent<Animator>();
      BossAudios = GetComponents<AudioSource>();
      BossAudio1 = BossAudios[0];
      BossAudio2 = BossAudios[1];
    }

    void Start() {
      canAttack = true;
      traveling = false;
      battleStarted = false;
      
      //Set initial waypoint
      currentPoint = wayPoints[0];
      
      //Sets boss to first waypoint
      transform.position = currentPoint;
      pointTracker = 0;
      pointTimer = 240;

      attackTracker = 0;
      finalAttackTracker = 0;
      finalAttack1 = true;
      finalAttack2 = false;

      damage = 1;
      currentHealth = 10;
      currentMoveSpeed = defaultMoveSpeed;

      aggroDistance = 20;
      //Player stuff
      Player = GameObject.Find("Player");
      PlayerAudio = Player.GetComponent<Player_Manager>().PlayerExternalAudio;

    }

    void Update() {
      
      //This should only run until player has entered the room.
      if (Player != null && !battleStarted) {
        distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance < aggroDistance && IntroComplete) {
          battleStarted = true;
        }
      }
      //Runs the battle routine
      if (battleStarted && countDown <= 0) {
        Routine();
      } else if (battleStarted && countDown >= 0){
        countDown--;
      }
    }

    /***************************************************************************************************************
    *  Routine() keeps track if the enemy is aggroed yet via the distance arguement. The aggroDistance is set to default 5
    * but can also be set via unity inspector.  If Player enters the aggroDistance, the patrol script's aggroed bool is set
    * to true and it's forces on the NPC rigidbody are removed. Once aggroed routine tracks if the NPC is chasing or close enough
    * to Attack and calls the appropriate function
    ****************************************************************************************************************/

    public void Routine() {
      //If boss is not traveling to a waypoint, he can attempt an attack
      if (!traveling) {
        //check whether attack timer is finished
        if (canAttack) {
          attackPicker();
          //When boss has low health, he will attack more frequently
          if (currentHealth > 3) {
            attackTimer = 180;
          }else {
            attackTimer = 30;
          }
        } else {
          //reset canAttack if timer is 0
          if (attackTimer <= 0) {
            canAttack = true;
          } else {
            attackTimer--;
          }
        }
        //reset traveling if timer is 0
        if (pointTimer == 0) {
          traveling = true;

        } else {
          pointTimer--;
        }
      } else {
        //Changes locations
        changePoint();
      }
    }

    void changePoint() {
      //Debug.Log("changing position :)");
      //Cycles through available locations. The last waypoint should not be used until the boss is low on health
      if (currentHealth > 3) {
        if (pointTracker + 1 > wayPoints.Length - 2) {
          pointTracker = 0;
          currentPoint = wayPoints[pointTracker];
        } else {
          pointTracker++;
          currentPoint = wayPoints[pointTracker];
        }
      }else {
        currentPoint = wayPoints[wayPoints.Length - 1];
      }


      //teleport to new location, set atWayPoint to true, reset timer
      transform.position = currentPoint;
      traveling = false;
      pointTimer = 240;
    }

    //Decides which attack is to be used. Two attacks at health, lowHealthAttack at low health
    public void attackPicker() {
      if (currentHealth > 3) {
        if (attackTracker == 0) {
          rangeAttack();
          attackTracker = 1;
        } else {
          tentacleAttack();
          attackTracker = 0;
        }
      }else {
        lowHealthAttack();
      }
    }

    //Fires a magic missle starting from the location of the boss and going straight down
    public void rangeAttack() {
      playerLocation = Player.transform.position;
      
      //Instantiate the magic attack. Pass it the location of the player so that it can fire that direction
      GameObject missile = (GameObject)Instantiate(Resources.Load("MagicMissle"), transform.position, Quaternion.identity);
      missile.SendMessage("moveToTarget", playerLocation, SendMessageOptions.DontRequireReceiver);
      Physics2D.IgnoreCollision(missile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

      BossAudio1.clip = RangedAttackSound;
      BossAudio1.Play();

      //reset attacktimer
      canAttack = false;
      
    }

    //Creates a tentacle near the players location
    public void tentacleAttack() {

      BossAudio2.clip = AttackSound;
      BossAudio2.Play();
      playerLocation = Player.transform.position;

      GameObject.Instantiate(Resources.Load("Tentacle"), new Vector3(Random.Range(playerLocation.x-3, playerLocation.x+3),Random.Range(playerLocation.y -3, playerLocation.y+3),0), Quaternion.identity);
      canAttack = false;
    }


    public void lowHealthAttack() {
      playerLocation = Player.transform.position;

      if (attackTracker % 2 == 0) {
        GameObject fireBall = (GameObject)Instantiate(Resources.Load("FireBall"), transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(fireBall.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        fireBall.GetComponent<FireBall>().playerLocation = playerLocation;
      }

      BossAudio1.clip = FinalAttackSound1;
      BossAudio1.Play();

      //lets do something crazy
      if (attackTracker != 15 && finalAttack1) {
        GameObject.Instantiate(Resources.Load("Tentacle"), new Vector3(1.99f + attackTracker, 2.2f, 0), Quaternion.identity);
        GameObject.Instantiate(Resources.Load("Tentacle"), new Vector3(1.99f + attackTracker, -6.71f, 0), Quaternion.identity);
        attackTracker++;
      } else if (attackTracker != 15 && finalAttack2){
        GameObject.Instantiate(Resources.Load("Tentacle"), new Vector3(6.87f, 1.83f - attackTracker, 0), Quaternion.identity);
        GameObject.Instantiate(Resources.Load("Tentacle"), new Vector3(13.68f, 1.83f - attackTracker, 0), Quaternion.identity);
        attackTracker++;
      }

      if (attackTracker == 15) {
        attackTracker = 0;

        if (finalAttack1 == false) {
          finalAttack1 = true;
          finalAttack2 = false;
        }else {
          finalAttack1 = false;
          finalAttack2 = true;
        }
      }
      canAttack = false;
      //Wait..you want more crazy??

    }

    /// <summary>
    /// Removes Health from Character and then checks death condition. Also starts the flash on hit coroutine before going to 
    /// knockback "Hit" function
    /// </summary>
    public void TakeDamage(Character Char) {

      // Remove HP.
      currentHealth -= Char.damage;
      PlayerAudio.clip = TakeDamageSound;
      PlayerAudio.Play();

      if (currentHealth <= 0) {
        Death();
        return;
      }

      //we dont die so we need the proper knockback
      StartCoroutine(Flasher());
      hit(Char.transform, Char.joltAmount);
    }

    /// <summary>
    /// Everything you want to happen when the GameObject dies.
    /// </summary>
    private void Death() {
      // Display Health as 0.
      PlayerAudio.clip = DieSound;
      PlayerAudio.Play();
      currentHealth = 0;
      defeated = true;


      // Play the Die SwingSound here

      // Put getting loot code here like money or Health
      dropLoot();

      //Boss needs to drop a triforce piece and a warp circle

      // Destroy this gameobject.
      Destroy(this.gameObject);
    }

    /// <summary>
    /// Everything you want to happen when the GameObject takes damage but doesnt die.
    /// </summary>
    private void hit(Transform OtherCharacter, float joltAmount) {
      // Play the SwingSound from getting hit.

      // If we have an enemy hit animation

      // IF the Character that we collided with can be knockedback.
      if (canBeJolted) {
        // Get the relative position.
        Vector2 relativePos = gameObject.transform.position - OtherCharacter.position;
        // Get the rigidbody2D
        Rigidbody2D CharRigid = gameObject.GetComponent<Rigidbody2D>();
        // Stop the colliding objects velocity.
        CharRigid.velocity = Vector3.zero;
        // Apply knockback.
        CharRigid.AddForce(relativePos.normalized * joltAmount, ForceMode2D.Impulse);

      }
    }

    // This gives the enemy a flashing effect when they get hit
    protected IEnumerator Flasher() {
      var render = GetComponent<SpriteRenderer>();

      var normalColor = render.color;

      render.material.color = Color.black;
      yield return new WaitForSeconds(.1f);
      render.material.color = normalColor;
      yield return new WaitForSeconds(.1f);
    }

    protected void dropLoot() {

      //Drop Triforce Piece
      GameObject.Instantiate(Resources.Load("TriforcePiece"), new Vector2(10.76278f, 0.5020895f), Quaternion.identity);

      //Place warp point back to village here
      GameObject warp = (GameObject)Instantiate(Resources.Load("OneWaySceneTrigger"), new Vector2(15.76278f, 0.5020895f), Quaternion.identity);
      warp.name = "ToVillageSpawnPoint";
      warp.tag = "Village";
    }
  }
}
