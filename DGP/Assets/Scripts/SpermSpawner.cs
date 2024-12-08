using UnityEngine;
using System.Collections;
using TMPro;

public class SpermSpawner : MonoBehaviour {
    public PowerUpManager powerUpManager; // Instantiate PowerUpManager
    public GameManager gameManager; // Instantiate GameManager
    public GameObject spermPrefab; // Instantiate Sperm Prefab
    public GameObject player; // Instantiate PowerUpManager
    public AudioClip spawnSound; // Sound to play when sperm are spawned

    // Spawn logic will differ if in game vs Title Screen
    public bool gameOn = false; // Assume game to be off

    private Vector2 screenBounds; // Instantiate Vector2 object to hold screen bounds

    private void Start() {
        // Get screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    private void Update() {
        DestroyOffScreenSperms(); // Destroy any sperms off screen
    }

    public void SpawnSperm() {
        int side = Random.Range(0, 4); // Generate random number between 0-3 (inclusive)
        Vector2 spawnPosition = Vector2.zero; // Initialize spawn position
        Vector2 direction = Vector2.zero; // Initialize direction

        // Spawn sperm on corresponding side
        switch (side) {
            case 0: // Top
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y);
                break;
            case 1: // Bottom
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y);
                break;
            case 2: // Left
                spawnPosition = new Vector2(-screenBounds.x, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 3: // Right
                spawnPosition = new Vector2(screenBounds.x, Random.Range(-screenBounds.y, screenBounds.y));
                break;
        }

        GameObject sperm = Instantiate(spermPrefab, spawnPosition, Quaternion.identity); // Create a sperm
        Vector2 playerPosition = player.transform.position; // Get plaer position as a 2D vector
        direction = (playerPosition - spawnPosition).normalized; // Calculate direction as the difference from player's position and sperm's spawn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Find where the sperm is facing
        sperm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Rotate the sperm
        sperm.GetComponent<Rigidbody2D>().velocity = direction * 5f; // Set sperm to default speed
        
        // While game is active
        if (gameOn) {
            gameManager.PlaySound(spawnSound); // Play sound when sperm spawn
            if (!powerUpManager.spermicideActive)
                // If spermicide is not active, scale speed to match wave
                sperm.GetComponent<Rigidbody2D>().velocity = direction * gameManager.spermSpeed;
        }
    }

    private void DestroyOffScreenSperms() {
        GameObject[] sperms = GameObject.FindGameObjectsWithTag("Sperm"); // Get array of all the sperms that are spawned
        foreach (GameObject sperm in sperms) { // Check if each sperm is on screen
            // If off screen, destroy the sperm
            if (IsOffScreen(sperm.transform.position)) {
                Destroy(sperm);
                if (gameOn) gameManager.UpdateSpermRemaining(); // If game is active, update sperm remaining counter
            }
        }
    }

    private bool IsOffScreen(Vector3 position) {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect; // Calculate screen width
        float screenHeight = Camera.main.orthographicSize; // Calculate screen height

        // Check if the asteroid's position exceeds the screen bounds
        return position.x > screenWidth || position.x < -screenWidth || 
               position.y > screenHeight || position.y < -screenHeight; // Check if off-screen
    }
}