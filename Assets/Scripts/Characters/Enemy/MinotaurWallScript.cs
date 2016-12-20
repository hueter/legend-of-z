using UnityEngine;
using System.Collections;

namespace RPG {
  public class MinotaurWallScript : MonoBehaviour {

        /**********************************************************************************************
         * OnTriggerEnter2D is triggered when a rigidbody enters the trigger colliders in the minotaur's
         *room. If the tag is Minotaur, he has charged hit a wall and will be stunned. If it's a charge damage box then it is destroyed
         * *******************************************************************************************/
        void OnTriggerEnter2D(Collider2D other) {

      if ((other.tag == "Minotaur") && (other.GetComponent<MinotaurAttackScript>().charging == true))
            {
            other.GetComponent<Minotaur_Enemy_Manager>().isDazed = true;
            other.GetComponent<Minotaur_Enemy_Manager>().dazedTimer = 20;
            }
      else if(other.tag == "MinotaurChargeDamageBox")
            {
                Destroy(other.gameObject);
            }

        }
  }
}