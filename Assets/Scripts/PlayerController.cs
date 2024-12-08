using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameManager gameManager; // Instantiate GameManager
    public PowerUpManager powerUpManager; // Instantiate PowerUpManager

    private float moveSpeed = 8f; // Set player speed

    private void Update() {
        // Get mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate screen bounds in world space
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float screenHalfHeight = Camera.main.orthographicSize;

        // Clamp the position to stay within the screen bounds
        mousePosition.x = Mathf.Clamp(mousePosition.x, -screenHalfWidth, screenHalfWidth);
        mousePosition.y = Mathf.Clamp(mousePosition.y, -screenHalfHeight, screenHalfHeight);

        // Move the player to the mouse position smoothly
        transform.position = Vector3.Lerp(transform.position, mousePosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // If player collides with sperm
        if (other.CompareTag("Sperm"))
            if (powerUpManager.birthControlActive)
                powerUpManager.BirthControlHit(); // Remove shield if shielded
            else
                gameManager.GameOver(); // End game if no shield
    }
}
