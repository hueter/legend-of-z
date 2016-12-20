using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {

  public class HudHealth : MonoBehaviour {

    public Sprite[] healthSprites;
    public Image[] healthImages;
    public Character PlayerCharacter;

    public int maxHealthAmount = 7;
    //public int startHealthAmount = 5;

    // Use this for initialization
    void Start() {
      PlayerCharacter = GameObject.Find("Player").GetComponent<Character>();
      checkHealthAmount();
    }

    // Update is called once per frame
    void Update() {
      if (PlayerCharacter != null) {
        checkHealthAmount();
      }
    }

    /*
     * Enables or disables the appropriate amount of hearts for the player
     * */
    void checkHealthAmount() {
      for (int i = 0; i < maxHealthAmount; i++) {
        if (PlayerCharacter.maxHealth <= i) {
          healthImages[i].enabled = false;
        }else {
          healthImages[i].enabled = true;
        }
      }
      updateHearts();
    }


    /*
     * Loops through available hearts and sets them to either full or empty based on players health
     * */
    void updateHearts() {
      bool empty = false;
      int i = 0;

      foreach (Image image in healthImages) {
        if (empty) {  
          image.sprite = healthSprites[0];
        }else {
          i++;
          if (PlayerCharacter.currentHealth >= i) {
            image.sprite = healthSprites[healthSprites.Length - 1];
          }else {
            image.sprite = healthSprites[0];
            empty = true;
          }
        }
      }
    }
  }
}