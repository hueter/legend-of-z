using UnityEngine;
using System.Collections;

namespace RPG {

  public class TriforceTracker : MonoBehaviour {

    public Player_Manager Player;
    public Sprite triforcePiece;

    void Start() {
      Player = GameObject.Find("Player").GetComponent<Player_Manager>();
    }

    public void PlayerInteraction() {
      
      //call the players collectitem function
      Player.SendMessage("collectItem", triforcePiece);
      Player.triforceCount++;

      //the intial interaction of the Player causes his Attack and movement to stop working. This resets those values
      Player.canMove = true;
      Player.canAttack = true;
      Player.currentlyInteracting = false;
      Destroy(this.gameObject);
      return;
    }
    
  }
}