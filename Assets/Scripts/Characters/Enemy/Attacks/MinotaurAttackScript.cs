using UnityEngine;


namespace RPG {

  public class MinotaurAttackScript : MonoBehaviour {
    public AudioClip ChargeMoo;

    private Animator Anim;

    public GameObject DamageBox;
    public GameObject MinotaurVerticalChargeDamageBox;
    public GameObject MinotaurHorizontalChargeDamageBox;

    public Vector3 playerPos;
    public Vector3 adjustment;

    private Minotaur_Enemy_Manager MEM;

    private GameObject Player;

    private float magicNumber;

    public bool charging = false;

    public Rigidbody2D MinotaurRigidbody;
    public float chargeSpeed = 5f;
    GameObject damage;


    // Use this for initialization
    void Start() {
      MinotaurRigidbody = GetComponent<Rigidbody2D>();
      Anim = GetComponent<Animator>();
      MEM = GetComponent<Minotaur_Enemy_Manager>();
      Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
    }

/***************************************************************************************************************
* attack() sets the animator to the attack animations. It also bases where the damage box should appear based on the 
* the players position from the boss's transform.
****************************************************************************************************************/
      public void attack() {

      
      playerPos = transform.position;
      //Set attack animation
      Anim.SetBool("isAttacking", true);

      //adjustments use the EnemyManagerScript distance float which is players vector minus This NPC's vector to adjust when where damage box appears             
      if (Anim.GetFloat("direction_x") != 0) {
        if (Anim.GetFloat("direction_x") > 0)
          adjustment.x = +MEM.distance;
        else
          adjustment.x = -MEM.distance;

        adjustment.y = 0;
      } else if (Anim.GetFloat("direction_y") != 0) {

        if (Anim.GetFloat("direction_y") > 0)
          adjustment.y = +MEM.distance;

        else
          adjustment.y = -MEM.distance;

        adjustment.x = 0;
      }
    }

    /***************************************************************************************************************
   * makeDamageBoxApper() is called from animator controller. It instantiates a damage box that could collider with 
   * player's rigidbody.
   ****************************************************************************************************************/
    private void makeDamageBoxApper() {
      damage = (GameObject)Instantiate(DamageBox, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation);
      damage.GetComponent<EnemyDamage>().Char = GetComponent<Character>();
      Physics2D.IgnoreCollision(damage.GetComponent<Collider2D>(), GetComponent<Collider2D>());

    }
       
/***************************************************************************************************************
 * makeChargeDamageBoxApper() is called from tryToCharge() below.  A chargeDamageBox is instatiated based on 
 * where the player is. 
****************************************************************************************************************/
 public void makeChargeDamageBoxAppear() {
      playerPos = MinotaurRigidbody.position;


      if (Anim.GetFloat("direction_x") != 0)
            {
                if (Anim.GetFloat("direction_x") > 0)
                {
                    adjustment.x = 1.7f;
                }
                else
                {
                    adjustment.x = -1.7f;
                }
            adjustment.y = 0;
            GameObject damage = (GameObject)Instantiate(MinotaurHorizontalChargeDamageBox, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation);
            Physics2D.IgnoreCollision(damage.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        else//(Anim.GetFloat("direction_y") != 0)
            {

                if (Anim.GetFloat("direction_y") > 0)
                {
                    adjustment.y = 1.7f;
                }
                else
                {
                    adjustment.y = -1.2f;
                }
             adjustment.x = 0;
             GameObject damage = (GameObject)Instantiate(MinotaurVerticalChargeDamageBox, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation);
             Physics2D.IgnoreCollision(damage.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }

    /***************************************************************************************************************
    * tryToCharge() is called from the Minotaur_Enemy_Manager script. It checks if the player is in lined up for a charge
    * on both the x and y axises. If the player is lined up it plays the charge moo Sound and calls makeChargeDamageBoxAppear().
    ****************************************************************************************************************/
    public void tryToCharge() {
      //find player 
      playerPos = transform.position;

      if (Anim.GetFloat("direction_x") != 0) {


        if ((playerPos.y - Player.transform.position.y < 1.5) && (playerPos.y - Player.transform.position.y > -1.5)) {

          charging = true;
          MEM.AttackNoise.clip = ChargeMoo;
          MEM.AttackNoise.Play();
          makeChargeDamageBoxAppear();
        }
      } else if (Anim.GetFloat("direction_y") != 0) {

        if ((playerPos.x - Player.transform.position.x < 1.5) && (playerPos.x - Player.transform.position.x > -1.5)) {

          charging = true;
          MEM.AttackNoise.clip = ChargeMoo;
          MEM.AttackNoise.Play();
          makeChargeDamageBoxAppear();
        }
      }
    }



    /***************************************************************************************************************
    * charge() sets the animator bool isCharging to true then moves the rigidbody according to what ever direction the
    * boss is facing.
    ****************************************************************************************************************/
    public void charge() {
      Anim.SetBool("isCharging", true);
      if (Anim.GetFloat("direction_x") > 0) {
        MinotaurRigidbody.velocity = new Vector2(chargeSpeed, 0);
      } else if (Anim.GetFloat("direction_x") < 0) {
        MinotaurRigidbody.velocity = new Vector2(-chargeSpeed, 0);
      } else if (Anim.GetFloat("direction_y") > 0) {
        MinotaurRigidbody.velocity = new Vector2(0, chargeSpeed);

      } else if (Anim.GetFloat("direction_y") < 0) {
        MinotaurRigidbody.velocity = new Vector2(0, -chargeSpeed);

      } else {
        Debug.Log("REALLY BAD IN TRAILING ELSE");
      }
    }
  }
}