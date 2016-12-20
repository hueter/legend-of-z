using UnityEngine;
using System.Collections;

namespace RPG {
  public class PhantomManager : Character {

    
 
    //Timer for when boss can attack
    public int attackTimer;

    public Transform[] movementPattern;
    
    //reference to Player
    public GameObject Player;
    public float distance;

    //Get Hit Sound
    public AudioSource[] EnemyAudios;
    public AudioSource EnemyAttackAudio;
    public AudioSource EnemyHitAudio;
    private AudioSource PlayerAudio;
    public AudioClip ReflectSound;
    public AudioClip Tappedsound;

    //tracks players last location
    private Vector3 downSpot = new Vector3(15.45f, -181.35f, 0);

    public Vector3 startPosition;

    //Varaibles that are used to carry out his movement pattern
    float angle = 0;
    // float speed = (2f * Mathf.PI) / 15;
    float speed = .2f;
    float radius = .25f;

    public bool currentlyDown;
    public GameObject currentMissle;

    public int hitCounter;
    public int downTimer;

    public Animator anim;

    public GameObject movementHolder;
    public Transform currentPoint;
    public int pointTracker;

    void Awake() {
      anim = GetComponent<Animator>();
    }

    void Start() {
      movementHolder = GameObject.Find("MovementHolder");
      movementPattern = new Transform[9];
      for (int i = 0; i < movementHolder.transform.childCount; i++) {
        movementPattern[i] = movementHolder.transform.GetChild(i).transform;
      }

      EnemyAudios = GetComponents<AudioSource>();
      EnemyAttackAudio = EnemyAudios[0];
      EnemyHitAudio = EnemyAudios[1];

      canAttack = true;

      // transform.position = startPosition;
      transform.position = movementPattern[0].position;
      currentPoint = movementPattern[1].transform;
      pointTracker = 0;

      damage = 1;
      currentHealth = 27;
      currentMoveSpeed = defaultMoveSpeed;

      //Player stuff
      Player = GameObject.Find("Player");
      attackTimer = 240;
      downTimer = 400;

      PlayerAudio = Player.GetComponent<Player_Manager>().PlayerExternalAudio;

      EnemyHitAudio.clip = AggroSound;
      EnemyHitAudio.Play();

      hitCounter = 0;
      //The boss can freely fly over the landscape
      Physics2D.IgnoreLayerCollision(0, 12,true);
      //Physics2D.IgnoreLayerCollision(0, 12);

    }

    void Update() {
      Routine();

    }

    /***************************************************************************************************************
    *  Routine() keeps track if the enemy is aggroed yet via the distance arguement. The aggroDistance is set to default 5
    * but can also be set via unity inspector.  If Player enters the aggroDistance, the patrol script's aggroed bool is set
    * to true and it's forces on the NPC rigidbody are removed. Once aggroed routine tracks if the NPC is chasing or close enough
    * to Attack and calls the appropriate function
    ****************************************************************************************************************/

    public void Routine() {
      
      //If boss is floating, he is able to attack as long as no other attack is flying around
      if (!currentlyDown) {
        //MoveToPosition();
        Physics2D.IgnoreLayerCollision(0, 12, true);
        followPattern();
        //check to see if phantom missile still exists
        if (currentMissle == null && canAttack) {
          rangeAttack();
        }
        if (attackTimer <= 0) {
          canAttack = true;
        } else {
          attackTimer--;
        }
      } else {
        //stay down for a certain period of time
        if (downTimer >= 0) {
          GetComponent<Rigidbody2D>().velocity = Vector3.zero;
          Physics2D.IgnoreLayerCollision(0, 12, false);
          downTimer--;
          //GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        } else {
          currentlyDown = false;
          downTimer = 400;
          anim.SetBool("isHit", false);          
        }
      }

    }

    public void MoveToPosition() {
      angle += speed * Time.deltaTime;
      transform.position = new Vector3(Mathf.Cos(angle) * radius + transform.position.x, Mathf.Sin(angle) * radius + transform.position.y, 0);
    }
    
    public void followPattern() {
      //travel between spots
      if (transform.position == currentPoint.transform.position) {
        changePoint();
      }else {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.transform.position, speed);
      }
    }

    public void changePoint() {

      if (currentHealth >= 10) {

        if (pointTracker < movementPattern.Length - 1) {
          pointTracker++;
          currentPoint = movementPattern[pointTracker];
        } else {
          pointTracker = 0;
          currentPoint = movementPattern[pointTracker];
        }
      }else {
        if (pointTracker > 0) {
          pointTracker--;
          currentPoint = movementPattern[pointTracker];
        } else {
          pointTracker = movementPattern.Length - 1;
          currentPoint = movementPattern[pointTracker];
        }

      }
    }

    public void rangeAttack() {
      anim.SetTrigger("isAttacking");
      EnemyAttackAudio.clip = RangedAttackSound;
      EnemyAttackAudio.Play();
      //Instantiate the magic attack. Pass it the location of the player so that it can fire that direction
      GameObject missile = (GameObject)Instantiate(Resources.Load("PMissle"), transform.position, Quaternion.identity);
      Physics2D.IgnoreCollision(missile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
      
      //reset attacktimer
      canAttack = false;
      attackTimer = 240;
    }

    public void fallDown() {
      hitCounter++;

      if (hitCounter == 5) {
        EnemyHitAudio.clip = TakeDamageSound;
        EnemyHitAudio.Play();
        anim.SetBool("isHit", true);
        hitCounter = 0;
        currentlyDown = true;
        transform.position = downSpot;
      }
    }

    

    /// <summary>
    /// Removes Health from Character and then checks death condition. Also starts the flash on hit coroutine before going to 
    /// knockback "Hit" function
    /// </summary>
    public void TakeDamage(Character Char) {

      // Remove HP.
      currentHealth -= Char.damage;
      EnemyHitAudio.clip = TakeDamageSound;
      EnemyHitAudio.Play();

      //GetComponent<AudioSource>().clip = getHit;
      //GetComponent<AudioSource>().Play();

      if (currentHealth <= 0) {
        Death();
        return;
      }
      StartCoroutine(Flasher());
    }

    /// <summary>
    /// Everything you want to happen when the GameObject dies.
    /// </summary>
    private void Death() {
      // Display Health as 0.
      currentHealth = 0;
      PlayerAudio.clip = DieSound;
      PlayerAudio.Play();
      // Play the Die SwingSound here
      GameObject.Find("MiniBossMusicTrigger").GetComponent<ChangeBackgroundMusic>().RevertBackToOldMusic();
      // Put getting loot code here like money or Health
      dropLoot();

      Destroy(GameObject.Find("DoNotPassRock1"));
      Destroy(GameObject.Find("DoNotPassRock2"));

      // Destroy this gameobject.
      Destroy(gameObject);
    }

    

    // This gives the enemy a flashing effect when they get hit
    protected IEnumerator Flasher() {
      var render = GetComponent<SpriteRenderer>();

      var normalColor = render.color;

      render.material.color = Color.red;
      yield return new WaitForSeconds(.1f);
      render.material.color = normalColor;
      yield return new WaitForSeconds(.1f);
    }

    protected IEnumerator ShieldFlasher() {
      var render = GetComponent<SpriteRenderer>();

      var normalColor = render.color;

      render.material.color = Color.blue;
      yield return new WaitForSeconds(.1f);
      render.material.color = normalColor;
      yield return new WaitForSeconds(.1f);
    }
    protected void dropLoot() {

     for (int i = 0; i < Player.GetComponent<Player_Manager>().maxHealth; i++) {
        GameObject.Instantiate(Resources.Load("Heart"), transform.position, Quaternion.identity);
      }
    }
  }
}
