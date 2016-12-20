using UnityEngine;

namespace RPG {
  public class SpawnPlayer : MonoBehaviour {

    public GameObject Player;

    // Use this for initialization
    void Awake() {

      //this spawns the Player somewhere to the side on the main menu. This was needed in order for certain game objects to not  get null references 
    //  GameObject ActivePlayer = (GameObject)Instantiate(Player, new Vector3(0f, 0, 0), Quaternion.identity);
      GameObject ActivePlayer = (GameObject)Instantiate(Player, new Vector3(100f, 100, 0), Quaternion.identity);
      ActivePlayer.GetComponent<Animator>().SetFloat("input_x", 1);
      ActivePlayer.GetComponent<Player_Manager>().canMove = true;
      //This stops the ActivePlayer object from being referred to as "clone" and instead gives just "Player"
      ActivePlayer.name = "Player";
      //Debug.Log(ActivePlayer.name);
    }

    void Start() {
    }
    // Update is called once per frame
    void Update() {
    }
  }
}