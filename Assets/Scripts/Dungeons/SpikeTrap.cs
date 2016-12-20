using UnityEngine;
using System.Collections;


namespace RPG {

  public class SpikeTrap : Character {

    //We need to give it a set of points to move between
    public Vector3[] wayPoints;
    public float spikeSpeed;
    public Vector3 currentPoint;
    public int pointTracker;
    //I dont think that it necessarily needs to be managed by enemy manager
    // Use this for initialization
    void Start() {
      pointTracker = 0;
      currentPoint = wayPoints[0];
      damage = defaultDamage;
    }

    // Update is called once per frame
    void Update() {

      //The spiketrap has a consistent movement from one waypoint to the other
      movementPattern();
    }


    void movementPattern() {
      if (transform.position == currentPoint) {
        changePoint();
      } else {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint, spikeSpeed);
      }
    }


    //sets up the next way point to be used for movement
    void changePoint() {
      if (pointTracker + 1 > wayPoints.Length - 1) {
        pointTracker = 0;
        currentPoint = wayPoints[pointTracker];
      } else {
        pointTracker++;
        currentPoint = wayPoints[pointTracker];
      }
    }

    void OnCollisionEnter2D(Collision2D collision) {
      //Debug.Log(collision.gameObject);
      if (collision.gameObject.tag == "Player") {
        collision.gameObject.SendMessage("TakeDamage", GetComponent<Character>(), SendMessageOptions.DontRequireReceiver);
      }
    }
  }
}
