using UnityEngine;

namespace RPG {
  public class MinotaurChargeDamageBox : MonoBehaviour {
    public int countdown;
    public Character Char;
    private GameObject ThisMinotaurChargeDamageBox;
    public Rigidbody2D chargeDamageBoxRigidbody;
    private GameObject Minotaur;

        // Use this for initialization
        void Start() {
            Minotaur = GameObject.FindWithTag("Minotaur");

            chargeDamageBoxRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

            ThisMinotaurChargeDamageBox = GameObject.FindWithTag("MinotaurChargeDamageBox");
      
            if(ThisMinotaurChargeDamageBox != null)
            {
                moveDamageBoxWithCharge();
            }


    }
        /***************************************************************************************************************
       * OnCollisionEnter2D() tracks the if the player's rigidbody enters the circle collider on the charge box. If the player
       * does enter they take damage 
       ****************************************************************************************************************/
        void OnCollisionEnter2D(Collision2D collision) {
      if (collision.gameObject.tag == "Player")
      {
         collision.gameObject.SendMessage("TakeDamage", Char, SendMessageOptions.DontRequireReceiver);
         // destroyMinotaurDamageBox();
         GetComponent<CircleCollider2D>().enabled = false; 
      }
    }
        //destroys gameobject
    public void destroyMinotaurDamageBox() {
      Destroy(this.gameObject);

    }
        /***************************************************************************************************************
       * moveDamageBoxWithCharge() moves the chargeDamageBox with the minotaur's chargespeed, with the same speed and direction
       * of Minotuar boss 
       ****************************************************************************************************************/
        public void moveDamageBoxWithCharge() {
      Animator Anim = Minotaur.GetComponent<Animator>();

      float chargeSpeed = Minotaur.GetComponent<MinotaurAttackScript>().chargeSpeed;

      if (Anim.GetFloat("direction_x") > 0) {
        chargeDamageBoxRigidbody.velocity = new Vector2(chargeSpeed, 0);
      } else if (Anim.GetFloat("direction_x") < 0) {
        chargeDamageBoxRigidbody.velocity = new Vector2(-chargeSpeed, 0);
      } else if (Anim.GetFloat("direction_y") > 0) {
        chargeDamageBoxRigidbody.velocity = new Vector2(0, chargeSpeed);

      } else if (Anim.GetFloat("direction_y") < 0) {
        chargeDamageBoxRigidbody.velocity = new Vector2(0, -chargeSpeed);

      } else {
                destroyMinotaurDamageBox();
        Debug.Log("REALLY BAD IN TRAILING ELSE");
      }
    }
  }
}
