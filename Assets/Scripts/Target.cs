using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    private GameManager gameManager;
    private MouseTrail mouseTrail;

    private Rigidbody targetRigidbody;
    public ParticleSystem explosionParticle;

    public int pointValue;

    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -2;

    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        mouseTrail = GameObject.Find("Mouse Trail").GetComponent<MouseTrail>();

        targetRigidbody = GetComponent<Rigidbody>();

        targetRigidbody.AddForce(RandomForce(), ForceMode.Impulse);
        targetRigidbody.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        transform.position = RandomSpawnPos();
    }

    private void OnMouseDown() {
        if (gameManager.isGameActive && !gameManager.isGamePause)
            DestroyGameObject();
    }

    private void OnMouseEnter() {
        if (mouseTrail.isMouseDragged && gameManager.isGameActive && !gameManager.isGamePause)
            DestroyGameObject();
    }

    private void DestroyGameObject() {
        Destroy(gameObject);

        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        gameManager.UpdateScore(pointValue);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);

        if (gameManager.isGameActive && !gameObject.CompareTag("Bad"))
            gameManager.DecreaseLives();
    }

    private Vector3 RandomForce() {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    private float RandomTorque() {
        return Random.Range(-maxTorque, maxTorque);
    }

    private Vector3 RandomSpawnPos() {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
