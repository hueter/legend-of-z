using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {
  public class ItemCanvas : MonoBehaviour {
    public Image ItemContainer;
    // Use this for initialization
    void Start() {
      ItemContainer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
    }
  }
}