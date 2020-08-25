using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime;

    private Transform player;
    private GameObject obj;
    private Vector3 spawnPosition = new Vector3();
    private float throwForce = 1100f;
    private float timer = 0f;
    private bool counting = false;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartCounting()
    {
        counting = true;
    }

    void Update()
    {
        if (counting && timer % 60 < 2)
        {
            timer += Time.deltaTime;
        }
        else if (counting && timer % 60 >= 2)
        {
            counting = false;
            StartSpawning();
        }

        if (!GameController.instance.decisions["enemy_appeared"] && obj != null && obj.gameObject.GetComponent<Lifespawn>().OnScreen)
        {
            LoadEnemyDialogue();
        }
    }

    public void LoadEnemyDialogue()
    {
        if (GameController.instance.GetState() != "dialogue")
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("BallsAppear");
            GameController.instance.decisions["enemy_appeared"] = true;
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (GameController.instance.GetState() == "dialogue")
                yield return null;
            else
            {
                yield return new WaitForSeconds(spawnTime);
                if (!GameController.instance.decisions["enemy_appeared"])
                    ThrowBalls(2);
                else ThrowBalls(0);
            }
        }
    }

    void ThrowBalls(float offset)
    {

        if (GameController.instance.GetState() != "dialogue")
        {
            //Random book spawn position
            float xPos = Random.Range(-48, 35.1f); //[min,max[
            float zPos = Random.Range(-80, 40.1f); //[min,max[

            spawnPosition = new Vector3(xPos, 5, zPos);
            obj = ObjectPooler.instance.GetPooledObject("Enemy");
            obj.transform.SetParent(gameObject.transform);
            obj.transform.position = spawnPosition;
            obj.SetActive(true);

            //Throw at player position
            Vector3 dir = (player.position + new Vector3(offset, 0, offset)) - obj.transform.position;

            obj.GetComponent<Rigidbody>().AddForce(dir.normalized * throwForce);
        }
    }
}
