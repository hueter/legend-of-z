using UnityEngine;
using System.Collections;


namespace RPG {
  public class Minotaur_Enemy_Manager : Character {

    public GameObject Minotaur_Boss_Room_Door_Cover;

    /* Sounds */
    public AudioClip HitButDidntTakeDamageSound;
    public AudioClip ChargingSound;
    public AudioClip StepSound;
    public AudioSource[] BossAudios;
    public AudioSource StepNoise;
    public AudioSource ChargeNoise;
    public AudioSource AttackNoise;
    public AudioSource HitNoise;
    private AudioSource PlayerAudio;

    //public int Experience = 0;

    public int numberOfHits = 0;
    public bool Patrol;
    public float aggroDistance = 18f;
    public int attackTimer;

    //I made these public so that we can change the scripts for individual enemies so we have some variation
    public Enemy4DirectionPatrol PatrolScript;
    public EnemyChase ChaseScript;
    public MinotaurAttackScript AttackScript;
    public bool isDazed = false;
    public float dazedTimer = 0;
    public float lengthOfBombStun = 200;

    //reference to player
    public GameObject Player;
    public float distance;
    private Rigidbody2D CharRigidBody;
    private Animator Anim;
    private bool IsPlayerInRoom = false;
    public bool IntroComplete = false;
    public bool defeated = false;

    void Awake() {
      // Assign the Animator Component.
      CharacterAnimator = GetComponent<Animator>();
    }

    void Start() {
      //Set the basic stats. Also attach the appropriate scripts. These can be changed in the inspector for individual enemies
      BossAudios = GetComponents<AudioSource>();
      StepNoise = BossAudios[0];
      StepNoise.clip = StepSound;
      ChargeNoise = BossAudios[1];
      ChargeNoise.clip = ChargingSound;
      AttackNoise = BossAudios[2];
      AttackNoise.clip = AttackSound;
      HitNoise = BossAudios[3];
      HitNoise.clip = TakeDamageSound;
      canAttack = true;
      Anim = GetComponent<Animator>();
      ChaseScript = GetComponent<EnemyChase>();
      AttackScript = GetComponent<MinotaurAttackScript>();
      currentDamage = damage;
      currentHealth = maxHealth;
      currentMoveSpeed = defaultMoveSpeed;
      CharRigidBody = GetComponent<Rigidbody2D>();
      //Player stuff
      Player = GameObject.Find("Player");
      PlayerAudio = Player.GetComponent<Player_Manager>().PlayerInternalAudio;

    }

    void Update() {
      //This is used for the routine To check direction of player compared to enemy

      if (Player) {
        distance = Vector3.Distance(transform.position, Player.transform.position); //distance is enemy distance from player

        if (IsPlayerInRoom == false)        //based on phyiscall distance fro boss to determine in the room, didn't see the need for patrol script
        {
          IsPlayerInRoom = checkIfPlayerIsInBossRoom(distance);
          if (IsPlayerInRoom == true) //spawn door blocking entrance if player is in the room
          {
            GameObject.Instantiate(Minotaur_Boss_Room_Door_Cover, new Vector3(22.78f, -186.28f, 0), transform.rotation);

          }
        } else  //if player is in boss room
          {
          if ((AttackScript.charging == true) && (isDazed == false)) {
            
            ChargeNoise.Play();
            AttackScript.charge(); //moves charge box and minotaur rigidbodies if charging
          } else {
            routine(); //routine chases player and attacks based on distance
          }
        }
      }
    }

    /***************************************************************************************************************
    *  Routine()  The aggroDistance is set via unity inspector.  If player enters the aggroDistance, . Once aggroed routine tracks if the NPC is chasing or close enough
    * to attack and calls the appropriate function
    ****************************************************************************************************************/

    public void routine() {

      if (isDazed == true)  //dazed happens when boss charge hits a wall and when boss is hit by player's bomb
      {
        if (dazedTimer <= 0) {
          isDazed = false;
          AttackScript.charging = false;
          Anim.SetBool("isCharging", false);
        } else {
          CharRigidBody.Sleep();
          dazedTimer--;
        }
      } else //if not dazed
        {
        if (distance > 5.5f)    //chase player and try to charge if player is close to x or y axis of boss
        {
          ChargeNoise.Play();
          ChaseScript.Chase();
          AttackScript.tryToCharge();

        } else {
          StepNoise.Play();
          ChaseScript.Chase();
          if (distance <= 2.6f) //melee attack if close enough
          {
            AttackNoise.Play();
            AttackScript.attack();
          }
        }
      }
    }
    /// <summary>
    /// Removes health from character and then checks death condition. Also starts the flash on hit coroutine before going to 
    /// knockback "Hit" function. If boss is stunned he takes damage, if not sound is played.
    /// </summary>
    public void TakeDamage(Character Char) {

      // Remove HP.
      if (isDazed == true) {
        HitNoise.clip = TakeDamageSound;
        HitNoise.Play();
        currentHealth -= Char.damage;
        //we dont die so we need the proper knockback
        StartCoroutine(Flasher());
        Hit(Char.transform, Char.joltAmount);
      } else {
        HitNoise.clip = HitButDidntTakeDamageSound;
        HitNoise.Play();
      }

      if (currentHealth <= 0) {
        GameObject door = GameObject.FindWithTag("MinotaurBossSpawnedDoor");
        Destroy(door); // removes door that spawns in the start of boss fight
        Death();
        return;
      }
    }

    /// <summary>
    /// Everything you want to happen when the GameObject dies.
    /// </summary>
    private void Death() {
      // Display health as 0.
      defeated = true;
      currentHealth = 0;
      PlayerAudio.clip = DieSound;
      PlayerAudio.Play();

      // Play the Die Sound here

      // Put getting loot code here like money or health
      dropLoot();

      // insert death effect here

      // Destroy this gameobject.
      Destroy(gameObject);
    }

    /// <summary>
    /// Everything you want to happen when the GameObject takes damage but doesnt die.
    /// </summary>
    private void Hit(Transform otherCharacter, float joltAmount) {
      // Play the Sound from getting hit.

      // If we have an enemy hit animation

      // IF the character that we collided with can be knockedback.
      if (canBeJolted) {

        // Get the rigidbody2D
        Rigidbody2D CharRigid = gameObject.GetComponent<Rigidbody2D>();
        // Stop the colliding objects velocity.
        CharRigid.velocity = Vector3.zero;
        // Apply knockback.
        // CharRigid.AddForce(relativePos.normalized, ForceMode2D.Impulse);
      }
    }


    // This gives the enemy a flashing effect when they get hit
    IEnumerator Flasher() {
      var render = GetComponent<SpriteRenderer>();

      var normal_color = render.color;

      render.material.color = Color.black;
      yield return new WaitForSeconds(.1f);
      render.material.color = normal_color;
      yield return new WaitForSeconds(.1f);
    }

    void dropLoot() {
      //random roll. seed 
      Random.InitState((int)(Time.deltaTime));
      if (Random.Range(0, 10) % 2 == 0) {
        //gets heart from the resource folder and drops it at enemies position
        GameObject.Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
      }


      //Drop Triforce Piece
      GameObject.Instantiate(Resources.Load("TriforcePiece"), new Vector2(22.1675f, -172.86f), Quaternion.identity);

      //spawn a warp to village
      GameObject warp = (GameObject)Instantiate(Resources.Load("OneWaySceneTrigger"), new Vector2(22.1675f, -175.0f), Quaternion.identity);
      warp.name = "ToVillageSpawnPoint";
      warp.tag = "Village";
    }


    /*************************************************************************************************
    * OnTriggerEnter2D tracks if a rigidbody enters the Minotaur's circle collider that is set to a trigger.
    * If it's a bomb rigidbody then stunn boss
    ************************************************************************************************/
    void OnTriggerEnter2D(Collider2D coll) {
      if (coll.gameObject.tag == "BombDamageBox") {
        GameObject ThisMinotaurChargeDamageBox = GameObject.FindWithTag("MinotaurChargeDamageBox");
        if (ThisMinotaurChargeDamageBox != null) {
          Destroy(ThisMinotaurChargeDamageBox);
        }

        isDazed = true;
        dazedTimer = lengthOfBombStun;
        Anim.SetBool("isWalking", false);
        Anim.SetBool("isCharging", false);
        Anim.SetBool("isAttacking", false);
      }
    }

    /*************************************************************************************************
   * checkIfPlayerIsInBossRoom simply returns a bool if player is in the boss room. This is based on 
   * the aggroDistance set via inspector and the player's transfom posisiton
    ************************************************************************************************/

    bool checkIfPlayerIsInBossRoom(float distance) {
      if ((distance < aggroDistance) && IntroComplete) {
        return true;
      }
      return false;
    }
  }
}
