using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    private RectTransform canvas;
    private List<GameObject> clouds = new List<GameObject>();

    public float waitTime = 2;
    public float spawnAmount = 2;
    public bool alternativeColors;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("UI Canvas").GetComponent<RectTransform>();

        if (alternativeColors)
            clouds.AddRange(Resources.LoadAll<GameObject>("CloudsAlt"));
        else clouds.AddRange(Resources.LoadAll<GameObject>("Clouds"));
    }

    public void SpawnClouds()
    {
        //Clear spawned clouds
        foreach (Transform child in gameObject.transform)
            Destroy(child.gameObject);

        //Spawn new clouds
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject prefabToSpawn = clouds[Random.Range(0, clouds.Count - 1)];

            //Random position
            float xPos = Random.Range(0, Screen.width);
            float yPos = Random.Range(0, Screen.height);
            Vector3 spawnPosition = new Vector3(xPos, yPos, 0f);

            //Random scale
            float scaleFactor = Random.Range(5f, 8f);
            Vector3 spawnScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            //Spawn clouds
            GameObject spawnObj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity) as GameObject;
            spawnObj.transform.SetParent(gameObject.transform);
            spawnObj.transform.position = spawnPosition;
            spawnObj.transform.localScale = spawnScale;

            //Random rotation
            var euler = transform.eulerAngles;
            euler.z = Random.Range(0f, 360f);
            spawnObj.transform.eulerAngles = euler;
        }

        spawnAmount++;
    }
}
