using UnityEngine;
using System.Collections;


namespace RPG {
  public class CameraFollow : MonoBehaviour {

    //References the Player object
    public Transform Target;
    public float camSpeed = 0.1f;
    Camera mainCam;

    GameObject Player;
    // Use this for initialization
    void Start() {
      mainCam = GetComponent<Camera>();
      Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // LateUpdate is called after updating each frame
    void Update() {
      mainCam.orthographicSize = (Screen.height / 100f) / 0.8f;

      if (Target) {
        transform.position = Vector3.Lerp(transform.position, Target.position, 0.1f) + new Vector3(0, 0, -10);
      }
    }
  }
}