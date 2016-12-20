using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Zoning : MonoBehaviour {

  private string currentScene;
  public string zoneDisplayName;
  public bool displayText = false;
  private Text ZText;

	// Use this for initialization
	void Start () {
    ZText = GameObject.Find("ZoneText").GetComponent<Text>();
  }

  // Update is called once per frame
  void Update () {
    if (displayText) {
      currentScene = SceneManager.GetActiveScene().name;
      sceneNameToDisplayName(currentScene);
      ZText.text = zoneDisplayName;
      StartCoroutine(ShowText());
    } else {
      ZText.text = "";
    }
  }

  IEnumerator ShowText() {
    yield return new WaitForSeconds(3.5f);
    displayText = false;
  }

  private void sceneNameToDisplayName(string sceneName) {
    switch(sceneName) {
      case "Village":
        zoneDisplayName = "";
        break;
      case "Forest":
        zoneDisplayName = "The Old Forest";
        break;
      case "ForestDungeon":
        zoneDisplayName = "The Ancient Temple of Doom";
        break;
      case "River":
        zoneDisplayName = "The Sacred River";
        break;
      case "RiverCave":
        zoneDisplayName = "The Cave of Death";
        break;
      case "CastleEntrance":
        zoneDisplayName = "The Castle Entrance";
        break;
      case "GanonsTower":
        zoneDisplayName = "Ganon's Tower";
        break;
      default:
        zoneDisplayName = "";
        break;
    }
  }

}
