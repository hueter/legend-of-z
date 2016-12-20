using UnityEngine;
using System.Collections;

namespace RPG {
  public class Enemy_Manager : Character {

    //	public int Experience = 0;

    public bool patrol;
    public float aggroDistance = 5f;

    public int attackTimer;

    //I made these public so that we can change the scripts for individual Enemies so we have some variation
    public Enemy4DirectionPatrol PatrolScript;
    public EnemyChase ChaseScript;
    public AttackScript Attack;
    public int TimeBetweenEnemyAttacks;


    public float attackRange;

    //reference to player
    public GameObject Player;
    private Player_Manager PM;
    public float distance;
    private Rigidbody2D CharRigidBody;
    public RaycastHit2D[] hits;
    public Collider2D enemyCollider;

    /*** Sound FX and Audio ***/
    public AudioSource[] EnemyAudios;
    public AudioSource EnemyAttackAudio;
    public AudioSource EnemyHitAudio;
    private AudioSource PlayerAudio;





    void Awake() {
      // Assign the Animator Component.
      CharacterAnimator = GetComponent<Animator>();
      EnemyAudios = GetComponents<AudioSource>();
      EnemyAttackAudio = EnemyAudios[0];
      EnemyHitAudio = EnemyAudios[1];
    }

    void Start() {
      //Set the basic stats. Also attach the appropriate scripts. These can be changed in the inspector for individual enemies
      patrol = true;
      canAttack = true;

      joltAmount = 5f;

      PatrolScript = GetComponent<Enemy4DirectionPatrol>();
      ChaseScript = GetComponent<EnemyChase>();
      Attack = GetComponent<AttackScript>();

      currentDamage = damage;
      currentHealth = maxHealth;
      currentMoveSpeed = defaultMoveSpeed;

      CharRigidBody = GetComponent<Rigidbody2D>();

      enemyCollider = GetComponent<Collider2D>();

      canBeJolted = true;

      //Player stuff
      Player = GameObject.Find("Player");
      PM = Player.GetComponent<Player_Manager>();
      PlayerAudio = PM.PlayerExternalAudio;
    }

    void Update() {
      //This is used for the routine To check direction of Player compared to enemy
      if (Player != null) {
        distance = Vector3.Distance(transform.position, Player.transform.position);
        routine(distance);
      }
    }



    /***************************************************************************************************************
    *  Routine() keeps track if the enemy is aggroed yet via the distance arguement. The aggroDistance is set to default 5
    * but can also be set via unity inspector.  If player enters the aggroDistance, the patrol script's aggroed bool is set
    * to true and it's forces on the NPC rigidbody are removed. Once aggroed routine tracks if the NPC is chasing or close enough
    * to attack and calls the appropriate function
    ****************************************************************************************************************/

    public void routine(float distance) {

      // Vector3 raycastDir = transform.position - Player.transform.position;
      Vector3 fromPosition = transform.position;
      Vector3 toPosition = Player.transform.position;

      //Adjusts the posistion the ray is being shot from to avoid hiting enemy's collider
      if (transform.position.y > Player.transform.position.y) {
        fromPosition.y -= 1;
      } else {
        fromPosition.y += 1;
      }
      if (transform.position.x > Player.transform.position.x) {
        fromPosition.x -= 1;
      } else {
        fromPosition.x += 1;
      }

      Vector3 direction = toPosition - fromPosition;
      Debug.DrawRay(fromPosition, direction, Color.white, 1.0f);

      //hit records the first collider hit by the ray
      RaycastHit2D hit = Physics2D.Raycast(fromPosition, direction);

      //Debug.Log(hit.collider.gameObject.tag);

      if ((distance <= aggroDistance) && (hit.collider.gameObject.tag == "Player") && (PatrolScript.aggroed == false)) {
        PatrolScript.aggroed = true;
        EnemyHitAudio.clip = AggroSound;
        EnemyHitAudio.Play();
        CharRigidBody.Sleep();//sleep resets the forces from the patrol script on the rigidbody 2d sorry for the hack
        PatrolScript.enabled = false;
      }

      if (PatrolScript.enabled == false) //Player has aggroed enemy
      {
        if (distance >= attackRange) {
          ChaseScript.Chase();  //if aggroed keep moving
        }

        attackTimer -= 1;     //always decrease attack timer
        if (attackTimer <= 0) {
          canAttack = true;
        }

        if ((distance <= attackRange) && (canAttack)) //if in range for attack then attack
        {
          Attack.Attack();
          if (RangedAttackSound) {
            EnemyAttackAudio.clip = RangedAttackSound;
          } else {
            EnemyAttackAudio.clip = AttackSound;
          }
          EnemyAttackAudio.Play();
          attackTimer = TimeBetweenEnemyAttacks;
          canAttack = false;
        }
      }
    }




    public void TakeDamage(Character Char) {

      if (PatrolScript.aggroed == false) {
        PatrolScript.aggroed = true;
        CharRigidBody.Sleep();//sleep resets the forces from the patrol script on the rigidbody 2d sorry for the hack
        PatrolScript.enabled = false;
      }

      // Remove HP.
      currentHealth -= Char.damage;
      EnemyHitAudio.clip = TakeDamageSound;
      EnemyHitAudio.Play();

      if (currentHealth <= 0) {
        // Death sound effects have to come through player because we destroy the enemy too fast
        PlayerAudio.clip = DieSound;
        PlayerAudio.Play();
    
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
      currentHealth = 0;

      // Put getting loot code here like money or Health
      dropLoot();

      // Destroy this gameobject.
      Destroy(gameObject);
    }

    /// <summary>
    /// Everything you want to happen when the GameObject takes damage but doesnt die.
    /// </summary>
    private void hit(Transform OtherCharacter, float joltAmount) {
      // Play the SwingSound from getting hit.

      // If we have an enemy hit animation

      // IF the Character that we collided with can be knockedback.
      if (canBeJolted) {
     
        // Get the rigidbody2D
        Rigidbody2D CharRigid = gameObject.GetComponent<Rigidbody2D>();
        // Stop the colliding objects velocity.
        CharRigid.velocity = Vector3.zero;
        // Apply knockback.

        //add fix to knockback here
        if (GetComponent<Animator>().GetFloat("direction_x") != 0) {
          if (GetComponent<Animator>().GetFloat("direction_x") > 0) {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
          } else {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
          }

        } else {
          if (GetComponent<Animator>().GetFloat("direction_y") > 0) {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
          } else {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
          }
        }
        // CharRigid.AddForce(relativePos.normalized * joltAmount, ForceMode2D.Impulse);

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
      //random roll. seed
      Random.InitState(System.DateTime.Now.Millisecond);
      //Random.InitState((int)(Time.deltaTime));
      if (Random.Range(0, 100) > 50) {
        //gets heart from the resource folder and drops it at Enemies position
        GameObject.Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
      } else
        GameObject.Instantiate(Resources.Load("Rupee"), transform.position, Quaternion.identity);
      //Debug.Log("drop loot roll");
      //drop hearts or Rupees. for now just hearts are implemented
    }
  }
}
