using UnityEngine;
using System.Collections;


public enum CharacterType { Player, Enemy, Neutral, All };

namespace RPG {

  public abstract class Character : MonoBehaviour {

    public bool canMove = true;
    public bool canAttack = true;
    public bool interactable = false;

    //Animator stuff
    public Animator CharacterAnimator;

    //Knockback variables
    public bool canBeJolted;
    public bool currentlyJolted = false;

    //Character stats
    public int maxHealth = 5;
    public int currentHealth;

    public int currentDamage;
    public int defaultDamage = 1;
    public int damage;
    //public int currentDamage;
    public float joltAmount = 2;

    //Movement speed
    public float defaultMoveSpeed = 3f;
    public float currentMoveSpeed;

    //Audio effects
    public AudioClip AggroSound;
    public AudioClip AttackSound;
    public AudioClip RangedAttackSound;
    public AudioClip TakeDamageSound;
    public AudioClip DieSound;

  }
}