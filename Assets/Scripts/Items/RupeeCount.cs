using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {

  public class RupeeCount : MonoBehaviour {

    public Player_Manager Player;

    // Use this for initialization
    void Start() {
      Player = GameObject.Find("Player").GetComponent<Player_Manager>();
      GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update() {
      GetComponent<Text>().text = "" + Player.rupees;
    }

    //Function to increase rupee count. Will be called when Rupees get picked up
    public void increaseCount(int amount) {
      Player.rupees += amount;
    }
 
    //Function to decease rupee count. Think shop or Upgrade store
    public bool decreaseCount(int amount) {
      //we cant have negative Rupees so just abort
      if (Player.rupees - amount < 0) {
        return false;
      } else {
        Player.rupees -= amount;
        return true;
      }
    }
  }
}