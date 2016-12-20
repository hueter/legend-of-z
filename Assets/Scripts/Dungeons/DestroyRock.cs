using UnityEngine;
using System.Collections;

namespace RPG {
  public class DestroyRock : MonoBehaviour {

    private Player_Manager Player;
    public int triforceNumber;

    // Use this for initialization
    void Start() {

      Player = GameObject.Find("Player").GetComponent<Player_Manager>();

    }

    // Update is called once per frame
    void Update() {
      checkTriforceCount();
    }

    private void checkTriforceCount() {

      if (Player.triforceCount >= triforceNumber) {
        Destroy(this.gameObject);
      }

    }
  }
    
}