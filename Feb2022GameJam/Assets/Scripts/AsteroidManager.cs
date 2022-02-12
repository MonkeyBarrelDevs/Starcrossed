using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AsteroidManager : MonoBehaviour
{   
    GameController controller;
    [SerializeField] GameObject[] asteroids;
    [SerializeField] float[] velocities;
    [SerializeField] double variationAngle;
    [SerializeField] float radius;
    [SerializeField] float destroyOffset;
    [SerializeField] float playSpaceSize;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector2 PickSpawnPoint() {
        return Random.insideUnitCircle.normalized * radius;
    }

    Vector3 PickTarget(Vector3 asteroidLocation) {
        float playRange = playSpaceSize/2;
        return new Vector3(Random.Range(-playRange, playRange), Random.Range(-playRange, playRange), 0) - asteroidLocation;
    }

    public void Spawn() {
        int asteroidIndex = Random.Range(0, asteroids.Length);
        // Condition check occurs in the GameController script.
        GameObject asteroidInstance = Instantiate(asteroids[asteroidIndex], PickSpawnPoint(), Quaternion.identity);
        asteroidInstance.transform.rotation.SetLookRotation(Vector3.forward, PickTarget(asteroidInstance.transform.position));
        asteroidInstance.GetComponent<Rigidbody2D>().velocity = PickTarget(asteroidInstance.transform.position).normalized * velocities[asteroidIndex];
        Debug.Log("yo");
    }

    public float GetDestroyRadius() {
        return destroyOffset + radius;
    }
}
