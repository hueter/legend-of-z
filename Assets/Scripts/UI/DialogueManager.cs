using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG {

  public class DialogueManager : MonoBehaviour {

    public GameObject DialogueBox;
    public Text DialogueName;
    public Text DialogueText;
    public Image Npc;
    public AudioClip DialogueNextSound;
    public AudioClip DialogueDoneSound;

    public bool DialogueActive;

    // Use this for initialization
    void Start() {
      DialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
    }

    public void ShowBox(string npcName, string dialogue, Sprite npcImage) {
      //Activates dialogue box
      DialogueActive = true;
      DialogueBox.SetActive(true);
      DialogueName.text = npcName;
      DialogueText.text = dialogue;
      Npc.sprite = npcImage;
    }

    public void HideBox() {
      DialogueActive = false;
      DialogueBox.SetActive(false);
      DialogueName.text = "";
      DialogueText.text = "";
    }
  }
}