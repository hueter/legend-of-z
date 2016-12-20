using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {

  public class ShopManager : MonoBehaviour {


    public Player_Manager player;

    //this variable stops multiple objects from spawning on top of each other
    public bool creationEvent;


    // Use this for initialization
    void Start() {
      player = GameObject.Find("Player").GetComponent<Player_Manager>();
      creationEvent = false;
    }

    // Update is called once per frame
    void Update() {
      createItems();
    }

    /*
     * Creates the appropriate items based on how far the player is progressed. It isnt fool proof though and requires that the player
     * basically buys all the items at once or else the player might lose out on other upgrades
     * **/
    public void createItems() {
      if (player.triforceCount == 1 && !creationEvent) {

        if (player.attackUpgradeNumber == 0) {
         GameObject attackIncrease1 = (GameObject)Instantiate(Resources.Load("AttackIncrease"), new Vector3(109.2f, -53.37f, 0f), Quaternion.identity);
          attackIncrease1.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        }
        if (player.healthUpgradeNumber == 0) {
          GameObject healthIncrease1 = (GameObject)Instantiate(Resources.Load("HealthIncrease"), new Vector3(112.74f, -53.37f, 0f), Quaternion.identity);
          healthIncrease1.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        }
        if (player.speedUpgradeNumber == 0) {
          GameObject speedIncrease1 = (GameObject)Instantiate(Resources.Load("SpeedIncrease"), new Vector3(115.8f, -53.37f, 0f), Quaternion.identity);
          speedIncrease1.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        }
        creationEvent = true;
      } else if (player.triforceCount == 2 && !creationEvent) {
        /**
        if (player.attackUpgradeNumber == 1) {
          GameObject attackIncrease2 = (GameObject)Instantiate(Resources.Load("AttackIncrease"), new Vector3(109.2f, -53.37f, 0f), Quaternion.identity);
          attackIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";
        }
        if (player.healthUpgradeNumber == 1) {
          GameObject healthIncrease2 = (GameObject)Instantiate(Resources.Load("HealthIncrease"), new Vector3(112.74f, -53.37f, 0f), Quaternion.identity);
          healthIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        }
        if (player.speedUpgradeNumber == 1) {
          GameObject speedIncrease2 = (GameObject)Instantiate(Resources.Load("SpeedIncrease"), new Vector3(115.8f, -53.37f, 0f), Quaternion.identity);
          speedIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        }
        **/
        if (player.attackUpgradeNumber == 0) {
          GameObject attackIncrease2 = (GameObject)Instantiate(Resources.Load("AttackIncrease"), new Vector3(109.2f, -53.37f, 0f), Quaternion.identity);
          attackIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";

          GameObject attackIncrease1 = (GameObject)Instantiate(Resources.Load("AttackIncrease"), new Vector3(109.2f, -52.1f, 0f), Quaternion.identity);
          attackIncrease1.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        } else if (player.attackUpgradeNumber == 1){
          GameObject attackIncrease2 = (GameObject)Instantiate(Resources.Load("AttackIncrease"), new Vector3(109.2f, -53.37f, 0f), Quaternion.identity);
          attackIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";
        }
        if (player.healthUpgradeNumber == 0) {
          GameObject healthIncrease2 = (GameObject)Instantiate(Resources.Load("HealthIncrease"), new Vector3(112.74f, -53.37f, 0f), Quaternion.identity);
          healthIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";

          GameObject healthIncrease1 = (GameObject)Instantiate(Resources.Load("HealthIncrease"), new Vector3(112.74f, -52.1f, 0f), Quaternion.identity);
          healthIncrease1.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        } else if (player.healthUpgradeNumber == 1){
          GameObject healthIncrease2 = (GameObject)Instantiate(Resources.Load("HealthIncrease"), new Vector3(112.74f, -53.37f, 0f), Quaternion.identity);
          healthIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";
        }
        if (player.speedUpgradeNumber == 0) {
          GameObject speedIncrease2 = (GameObject)Instantiate(Resources.Load("SpeedIncrease"), new Vector3(115.8f, -53.37f, 0f), Quaternion.identity);
          speedIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";

          GameObject speedIncrease1 = (GameObject)Instantiate(Resources.Load("SpeedIncrease"), new Vector3(115.8f, -52.1f, 0f), Quaternion.identity);
          speedIncrease1.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "15";
        } else if (player.speedUpgradeNumber == 1) {
          GameObject speedIncrease2 = (GameObject)Instantiate(Resources.Load("SpeedIncrease"), new Vector3(115.8f, -53.37f, 0f), Quaternion.identity);
          speedIncrease2.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "25";
        }







        creationEvent = true;
      }
    }

  }
}