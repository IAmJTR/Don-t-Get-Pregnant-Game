using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public SpermSpawner spermSpawner; // Instantiate SpermSpawner
    public PowerUpManager powerUpManager; // Instantiate PowerUpManager

    public TextMeshProUGUI waveTextTMP; // Wave counter text
    public TextMeshProUGUI highScoreText; // High score text
    public TextMeshProUGUI gameOverTextTMP; // Game over text
    public TextMeshProUGUI loadIncomingTextTMP; // Load incoming text
    public TextMeshProUGUI spermRemainingTextTMP; // Sperm remaining text

    public AudioSource audioSource; // Instantiate Audio Source
    public AudioClip roundSound; // Round end sound
    public AudioClip gameOverSound; // Game over sound

    public Button menuButton; // Instantiate main menu button
    public Button restartButton; // Instantiate resart button

    public float spermSpeed = 4f; // Initalize sperm speed to 4
    private float nextSpawnTime = 0f; // Initalize variable that keeps track of sperm spawn frequency in game
    private float spawnInterval = 0.5f; // Initalize spawn interval to half a second
    private int highScore = 0; // Initalize high score
    private int currentWave = 1; // Initalize inital wave to 1
    private int spermPerWave = 10; // Initalize inital sperm per wave
    private int spermSpawned = 0; // Initalized sperm spawned
    private int spermRemaining = 10; // Initalize sperm remaining

    private void Start() {
        Cursor.visible = false; // Make cursor invisible in game
        gameOverTextTMP.text = ""; // Hide game over text
        spermSpawner.gameOn = true; // Set game as active
        waveTextTMP.text = 1.ToString(); // Display first wave
        menuButton.gameObject.SetActive(false); // Hide main menu button
        restartButton.gameObject.SetActive(false); // Hide restart button
        highScore = PlayerPrefs.GetInt("HighScore", highScore); // Obtain high score
        highScoreText.text = "Highscore: " + highScore.ToString(); // Display high score
        spermRemainingTextTMP.text = "Remaining: " + spermRemaining.ToString(); // Display sperm remaining
    }

    private void Update() {
        // If sperms spawned is less than sperm in wave AND enough time has passed since last spawn
        if (spermSpawned < spermPerWave * currentWave && Time.time > nextSpawnTime) {
            // If wave is at 30% left begin load
            if (spermSpawned == (int)(spermPerWave * currentWave * 0.7f))
                StartCoroutine(LoadIncoming());
            // Stop sperm from spawning if there is a condom
            if (powerUpManager.condomActive) UpdateSpermRemaining(); else spermSpawner.SpawnSperm();
            spermSpawned++; // Increment sperm w/ or w/o condom
            nextSpawnTime = Time.time + spawnInterval; // Set next spawn time
        }

        // Move on to next wave if all sperm in the wave have been spawned and destroyed
        if (spermSpawned >= spermPerWave * currentWave && GameObject.FindGameObjectsWithTag("Sperm").Length == 0) {
            NextWave();
        }
    }

    IEnumerator LoadIncoming() {
        loadIncomingTextTMP.text = "Load Incoming!"; // Display Load Incoming text
        yield return new WaitForSeconds(0.85f); // Wait 0.85 of a second
        loadIncomingTextTMP.text = ""; // Hide Load Incoming text
        PlaySound(roundSound); // Play end of round sound
        spawnInterval = 0.1f; // Increase spawn rate to 10 times per second
    }

    private void NextWave() {   
        currentWave++; // Increment wave
        spermSpawned = 0; // Reset sperm spawned
        spermSpeed += 0.2f; // Increaase sperm speed by 0.2 each wave
        powerUpManager.AddPowers(); // Add one power up after completing a round
        waveTextTMP.text = currentWave.ToString(); // Update wave text display
        spawnInterval = 0.5f - (0.01f * currentWave); // Decrease spawn inteval by 0.01 each wave
        spermRemaining = spermPerWave * currentWave; // Set sperm remaing to the amount of sperm in wave
        spermRemainingTextTMP.text = "Remaining: " + spermRemaining.ToString(); // Update sperm remaining
    }

    private void UpdateHighScore() {
        // Update high score if current wave is greater
        if (currentWave > highScore) {
            highScore = currentWave;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "Highscore: " + highScore.ToString();
        }
    }

    private void RestartGame() {
        Time.timeScale = 1; // Resume game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Load initial scene
    }

    private void GoToMenu() {
        Time.timeScale = 1; // Resume Game
        SceneManager.LoadScene("TitleScreen"); // Load Main Menu
    }

    public void PlaySound(AudioClip sound) {
        // Play sound argument
        audioSource.PlayOneShot(sound);
    }

    public void UpdateSpermRemaining() {
        spermRemaining--; // Decrement sperm remaining
        if (spermRemaining <= 0) spermRemaining = 0; // Ensure sperm remaing isn't < 0
        spermRemainingTextTMP.text = "Remaining: " + spermRemaining.ToString(); // Update display of sperm remaining
    }

    public void GameOver() {
        UpdateHighScore(); // Update high score if possible
        Time.timeScale = 0; // Pause game
        Cursor.visible = true; // Make mouse visible
        PlaySound(gameOverSound); // Play game over sound
        spermSpawner.gameOn = false; // Set game to unactive
        gameOverTextTMP.text = "Game Over!"; // Display Game Over text
        menuButton.gameObject.SetActive(true); // Display main menu button
        restartButton.gameObject.SetActive(true); // Display restart button
    }
}
