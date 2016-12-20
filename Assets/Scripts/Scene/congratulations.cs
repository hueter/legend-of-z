using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {
  public class congratulations : MonoBehaviour {

    Button QuitButton;


    // Use this for initialization
    void Start() {
      QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
      QuitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update() {

    }

    public void QuitGame() {
      Application.Quit();
    }
  }
}

