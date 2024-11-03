using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenManager : MonoBehaviour {
    public SpermSpawner spermSpawner; // Instantiate SpermSpawner

    public AudioSource audioSource; // Audio Source for Title Screen elements
    public AudioClip hoverSound; // Sound for when you hover over buttons

    private Color hoverTextColor = new Color(0.35f,0.35f,0.35f,1f);  // Color to change to on hover
    private Color normalTextColor = Color.white;  // Normal color

    private float nextSpawnTime = 0f; // Keeps track of sperm spawn frequency in Title Screen

    private void Update() {
        // Spawn a sperm every half a second if in the title screen
        if (Time.time > nextSpawnTime && !spermSpawner.gameOn) {
            spermSpawner.SpawnSperm();
            nextSpawnTime = Time.time + 0.5f;
        }
    }
    
    private void StartGame() {
        SceneManager.LoadScene("MainGame"); // Switch scene to main game
    }

    private void ExitGame() {
        Application.Quit(); // This will quit the game when built
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // This is for quitting in the editor
        #endif
    }

    public void PlayHoverSound() {
        audioSource.PlayOneShot(hoverSound); // Play button hover sound
    }

    public void OnHoverEnter(TextMeshProUGUI buttonText) {
            buttonText.color = hoverTextColor; // Change color on hover
    }

    public void OnHoverExit(TextMeshProUGUI buttonText) {
            buttonText.color = normalTextColor; // Revert back to the original color
    }
}
