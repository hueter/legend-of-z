using UnityEngine;

namespace RPG {

  public class EnemyRangeAttackScript : AttackScript {

    public Rigidbody2D Arrow;

    // Use this for initialization
    void Start() {
      Anim = GetComponent<Animator>();
      EnemyManagerScript = GetComponent<Enemy_Manager>();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void Attack() {
      //Debug.Log("This is the overridden default");

      playerPos = transform.position;
      //Set Attack animation
      Anim.SetBool("isAttacking", true);
      
      //adjustments use the EnemyManagerScript distance float which is players vector minus This NPC's vector to adjust when where damage box appears             
      if ((Anim.GetFloat("direction_x") > .7) && (Anim.GetFloat("direction_y") > .7)) {
        adjustment.x = Anim.GetFloat("direction_x") - .59f;
        adjustment.y = Anim.GetFloat("direction_y") - .59f;
      } else if ((Anim.GetFloat("direction_x") < -.7) && (Anim.GetFloat("direction_y") > .7)) {
        adjustment.x = Anim.GetFloat("direction_x") + .59f;
        adjustment.y = Anim.GetFloat("direction_y") - .59f;
      } else if ((Anim.GetFloat("direction_x") < -.7) && (Anim.GetFloat("direction_y") < -.7)) {
        adjustment.x = Anim.GetFloat("direction_x") + .59f;
        adjustment.y = Anim.GetFloat("direction_y") + .59f;
      } else if ((Anim.GetFloat("direction_x") > .7) && (Anim.GetFloat("direction_y") < -.7)) {
        adjustment.x = Anim.GetFloat("direction_x") - .59f;
        adjustment.y = Anim.GetFloat("direction_y") + .59f;
      } else if (Anim.GetFloat("direction_x") != 0) {
        if (Anim.GetFloat("direction_x") > 0)
          adjustment.x = Anim.GetFloat("direction_x") + .5f;
        else
          adjustment.x = Anim.GetFloat("direction_x") - .5f;
        adjustment.y = 0;
      } else if (Anim.GetFloat("direction_y") != 0) {

        if (Anim.GetFloat("direction_y") > 0)
          adjustment.y = Anim.GetFloat("direction_y") + .5f;
        else
          adjustment.y = Anim.GetFloat("direction_y") - .5f;
        adjustment.x = 0;
      }

      Rigidbody2D arrowInstance = Instantiate(Arrow, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation) as Rigidbody2D;
      arrowInstance.GetComponent<EnemyArrow>().Char = GetComponent<Character>();
      arrowInstance.GetComponent<EnemyArrow>().anim = GetComponent<Animator>();
      Physics2D.IgnoreCollision(arrowInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());


    }
    /***************************************************************************************************************
     * makeDamageBoxApper() is called from animator controller. Uses 
     ****************************************************************************************************************/
    protected override void makeDamageBoxApper() {
      GameObject damage = (GameObject)Instantiate(DamageBox, new Vector3(playerPos.x + adjustment.x, playerPos.y + adjustment.y, playerPos.z), transform.rotation);
      damage.GetComponent<EnemyDamage>().Char = GetComponent<Character>();
      Physics2D.IgnoreCollision(damage.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
  }
}