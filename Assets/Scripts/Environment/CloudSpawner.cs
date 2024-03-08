using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] clouds;

    public Vector2 randomSpawnInterval;
    public Vector2 randomSpawnRange;
    public Vector2 randomSpeed;
    public Vector2 randomScale;

    private float timer;


    // Update is called once per frame
    void Update()
    {

        if(timer <= 0)
        {
            Cloud newCloud = GameObject.Instantiate(clouds[Random.Range(0, clouds.Length - 1)], transform).GetComponent<Cloud>();
            newCloud.transform.localScale *= Random.Range(randomScale.x, randomScale.y);
            newCloud.transform.position = new Vector3(newCloud.transform.position.x, newCloud.transform.position.y, Random.Range(0f, 1f));
            newCloud.transform.Translate(new Vector2(0, Random.Range(randomSpawnRange.x, randomSpawnRange.y)));
            newCloud.speed = Random.Range(randomSpeed.x, randomSpeed.y);

            timer += Random.Range(randomSpawnInterval.x, randomSpawnInterval.y);
        }

        timer -= Time.deltaTime;
    }
}
