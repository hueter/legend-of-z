using UnityEngine;

namespace RPG {

  public class Item_Manager : MonoBehaviour {

    public Player_Manager Player;
    //the sprite of the Chest after it has been opened
    public Sprite ChestOpen;
    //the sprite that will be used during collect Item animation
    public Sprite InsideItem;
    //controls whether object is capable of interaction
    public bool interactable;

    // Use this for initialization
    void Start() {
      Player = GameObject.Find("Player").GetComponent<Player_Manager>();
      interactable = true;
    }

    // Update is called once per frame
    void Update() {
    }

    public void PlayerInteraction() {
      if (interactable) {
        //Call function that gives approprach loot/action

        //change the sprite to the open Chest
        GetComponent<SpriteRenderer>().sprite = ChestOpen;

        
        //call the players collectitem function
        Player.SendMessage("collectItem", InsideItem);

        //the intial interaction of the Player causes his Attack and movement to stop working. This resets those values
        Player.canMove = true;
        Player.canAttack = true;
        Player.currentlyInteracting = false;

        //Chest has been opened and can never be used again
        interactable = false;

        return;
      }
      //if the object isnt interactable we still need regain control of Player
      Player.canMove = true;
      Player.canAttack = true;
      Player.currentlyInteracting = false;
    }
  }
}