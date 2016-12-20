using UnityEngine;

namespace RPG {

  public class EnemyDefaultAttackScript : AttackScript {

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
      if (Anim.GetFloat("direction_x") != 0) {
        if (Anim.GetFloat("direction_x") > 0)
          adjustment.x = +EnemyManagerScript.distance;
        else
          adjustment.x = -EnemyManagerScript.distance;

        adjustment.y = 0;
      } else if (Anim.GetFloat("direction_y") != 0) {

        if (Anim.GetFloat("direction_y") > 0)
          adjustment.y = +EnemyManagerScript.distance;

        else
          adjustment.y = -EnemyManagerScript.distance;

        adjustment.x = 0;
      }
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