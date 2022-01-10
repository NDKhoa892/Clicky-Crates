using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public List<GameObject> targets;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;

    public Button restartButton;
    public GameObject titleScreen;
    public Image pauseImage;

    public bool isGameActive;
    public bool isGamePause;

    private int score;
    private int lives;
    private float spawnRate = 1;

    private List<Vector3> velocity = new List<Vector3>();
    private List<Vector3> angular = new List<Vector3>(); 

    private void Start() {
    }

    public void StartGame(int difficulty) {
        score = 0;
        lives = 3;
        isGameActive = true;
        spawnRate /= difficulty;

        StartCoroutine(SpawnTarget());

        UpdateScore(0);
        livesText.text = "Lives: " + lives;

        titleScreen.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isGamePause)
                PauseGame();
            else
                UnpauseGame();
        }
    }

    IEnumerator SpawnTarget() {
        yield return new WaitForSeconds(spawnRate);

        while (isGameActive) {
            if (!isGamePause) {
                int index = Random.Range(0, targets.Count);
                Instantiate(targets[index], transform);
            }

            float spawnRateOffset = 1f;
            yield return new WaitForSeconds(spawnRate / spawnRateOffset);
        }
    }

    private void PauseGame() {
        isGamePause = true;
        pauseImage.gameObject.SetActive(true);

        foreach (Transform targetTransform in transform) {
            Rigidbody targetRigidbody = targetTransform.GetComponent<Rigidbody>();

            velocity.Add(targetRigidbody.velocity);
            angular.Add(targetRigidbody.angularVelocity);

            targetRigidbody.isKinematic = true;
        }
    }

    private void UnpauseGame() {
        isGamePause = false;
        pauseImage.gameObject.SetActive(false);

        foreach (Transform targetTransform in transform) {
            Rigidbody targetRigidbody = targetTransform.GetComponent<Rigidbody>();

            targetRigidbody.isKinematic = false;
            targetRigidbody.velocity = velocity[0];
            targetRigidbody.angularVelocity = angular[0];

            velocity.RemoveAt(0);
            angular.RemoveAt(0);
        }
    }

    public void UpdateScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void DecreaseLives() {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives == 0) GameOver();
    }

    public void GameOver() {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
