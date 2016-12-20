using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace RPG {

  public class ShopItem : MonoBehaviour {

    public Player_Manager Player;
    public RupeeCount Rupees;

    //the sprite of the Item Upgrade
    public Sprite Upgrade;
    public string inspectorName;

    //child object 
    public GameObject Canvas;


    //controls whether object is capable of interaction
    public bool interactable;

    // Use this for initialization
    void Start() {
      Player = GameObject.Find("Player").GetComponent<Player_Manager>();
      Upgrade = GetComponent<SpriteRenderer>().sprite;
      Rupees = GameObject.Find("RupeeCount").GetComponent<RupeeCount>();
      Canvas = transform.GetChild(0).gameObject;
      interactable = true;
      Upgrade.name = inspectorName;
    }

    // Update is called once per frame
    void Update() {
    }

    public void PlayerInteraction() {
      //checks if Item is interactable and if the the "cost" text (converted to int) is an acceptable amount to reduce from Player rupee count
      //I'll probably rewrite this so its not a super long conditional
      if (interactable && Rupees.decreaseCount(System.Convert.ToInt32(Canvas.transform.GetChild(1).GetComponent<Text>().text))) {
        //Debug.Log("someone interacted with me :)");

        //Call function that gives approprach loot/action

        //Destroy the object
        Destroy(this.gameObject);


        //call the players collectitem function
        Player.SendMessage("collectItem", Upgrade);

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