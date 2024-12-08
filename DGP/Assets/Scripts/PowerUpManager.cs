using UnityEngine;
using System.Collections;
using TMPro;

public class PowerUpManager : MonoBehaviour {
    public GameObject player; // Instantiate Player
    public GameManager gameManager; // Instantiate GameManager
    
    public Sprite egg; // Instantiate default egg sprite
    public Sprite eggSheild; // Instantiate sheilded egg sprite

    public TextMeshProUGUI condomCounterTMP; // Text for available Condoms
    public TextMeshProUGUI spermicideCounterTMP; // Text for available Spermicide
    public TextMeshProUGUI birthControlCounterTMP; // Text for available Birth Control

    public AudioClip condomSound; // Sound to play when Condom is activated
    public AudioClip spermicideSound; // Sound to play when Spermicide is activated
    public AudioClip birthControlSound; // Sound to play when Birth Control is activated
    public AudioClip birthControlHitSound; // Sound to play when Birth Control is deactivated

    public bool condomActive = false; // Condom power up is not active
    public bool spermicideActive = false; // Spermicide power up is not active
    public bool birthControlActive = false; // Birth Control power up is not active

    private KeyCode condomKey = KeyCode.Alpha1; // Set Condom to the 1 key
    private KeyCode spermicideKey = KeyCode.Alpha2; // Set Spermicide to the 2 key
    private KeyCode birthControlKey = KeyCode.Alpha3; // Set Birth Bontrol to the 3 key

    private int condomCount = 0; // Begin game with 0 Condoms
    private int spermicideCount = 0; // Begin game with 0 Spermicide
    private int birthControlCount = 1; // Begin game with 0 Birth Control

    private void Start() {
        UpdatePowerUpCounters(); // Reset power up counters when game begins
    }

    private void Update(){
        // Check if any power up keys are being pressed
        if (Input.GetKeyDown(condomKey))
            UseCondom(); // Use Condom if 1 key is pressed
        if (Input.GetKeyDown(spermicideKey))
            UseSpermicide(); // Use Spermicide if 2 key is pressed
        if (Input.GetKeyDown(birthControlKey))
            UseBirthControl(); // Use Birth Control if 3 key is pressed
    }

    private void UseCondom() {
        if (condomCount > 0 && !condomActive) { // Only use if availible and not active
            condomCount--; // Decrement Condom count
            UpdatePowerUpCounters(); // Update visual of Comdom count
            StartCoroutine(Condom()); // Activate Condom logic
            gameManager.PlaySound(condomSound); // Play Condom sound
        }
    }

    private IEnumerator Condom() {
        // Set Condom to active for 5 seconds
        condomActive = true;
        yield return new WaitForSeconds(5f);
        condomActive = false;
    }

    private void UseSpermicide() {
        if (spermicideCount > 0 && !spermicideActive) { // Only use if availible and not active
            spermicideCount--; // Decrement Condom count
            UpdatePowerUpCounters(); // Update visual of Spermicide count
            StartCoroutine(Spermicide()); // Activate Spermicide logic
            gameManager.PlaySound(spermicideSound); // Play Spermicide sound
        }
    }

    private IEnumerator Spermicide() {
        // Set Spemicide to active for 10 seconds
        spermicideActive = true;
        yield return new WaitForSeconds(10f);
        spermicideActive = false;
    }

    private void UseBirthControl() {
        if (birthControlCount > 0 && !birthControlActive) { // Only use if availible and not active
            birthControlCount--; // Decrement Condom count
            UpdatePowerUpCounters(); // Update visual of Spermicide count
            birthControlActive = true; // Set Birth Control to active until hit
            gameManager.PlaySound(birthControlSound); // Play Birth Control sound
            player.GetComponent<SpriteRenderer>().sprite = eggSheild; // Update sprite to shielded egg
        }
    }

    public void BirthControlHit() {
        birthControlActive = false; // Set Birth Control to unactive
        gameManager.PlaySound(birthControlHitSound); // Play Birth Control Hit sound
        player.GetComponent<SpriteRenderer>().sprite = egg; // Update sprite to default egg
    }

    public void AddPowers() {
        int power = Random.Range(0, 3); // Generate random number between 0-3 (inclusive)
        // Randomly Picks one power up to increment
        switch (power) {
            case 0:
                condomCount++;
                break;
            case 1:
                spermicideCount++;
                break;
            case 2:
                birthControlCount++;
                break;
        }
        UpdatePowerUpCounters(); // Update visual of power counts
    }

    private void UpdatePowerUpCounters() {
        condomCounterTMP.text = condomCount.ToString(); // Update Condom Text amount
        spermicideCounterTMP.text = spermicideCount.ToString(); // Update Spermicide Text amount
        birthControlCounterTMP.text = birthControlCount.ToString(); // Update Birth Control Text amount
    }

}
