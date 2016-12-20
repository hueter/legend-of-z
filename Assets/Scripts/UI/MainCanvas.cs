using UnityEngine;
using System.Collections;

namespace RPG {
  public class MainCanvas : MonoBehaviour {
    public static bool created = false;
    void Awake() {

      //This was added in because of the nastiness that happens when you use persistent objects across scenes
      //In particular this solves some issues when going back to previous scenes that certain game objects were set up in

      //In this case it stops the UI Canvas component from duplicating itself leading to all kinds of bad things
      if (!created) {
        DontDestroyOnLoad(this.gameObject);
        created = true;
      } else {
        Destroy(this.gameObject);
      }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
  }
}