using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksController : MonoBehaviour
{
    public Vector3 offset; //Where to spawn relative to player
    public float spawnTime; //Interval between book thrown
    private int throwForce = 800;
    public int ThrowForce
    {
        get { return throwForce; }
        set { throwForce = value; }
    }

    private Transform player;
    private GameObject obj;
    private Material[] textures;
    private Vector3 spawnPosition = new Vector3();
    private string bookTag = "MG4Book";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        textures = Resources.LoadAll<Material>("Minigame4/Materials");
    }

    void OnEnable()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            ThrowBooks();
        }
    }

    void ThrowBooks()
    {
        //Random book spawn position
        float xPos = Random.Range(-offset.x, offset.x + .1f); //[min,max[
        spawnPosition = new Vector3(player.position.x + xPos, player.position.y + offset.y, player.position.z + offset.z);
        obj = ObjectPooler.instance.GetPooledObject(bookTag);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.position = spawnPosition;

        //Random texture
        int mat = Random.Range(0, textures.Length);
        obj.GetComponentInChildren<MeshRenderer>().material = textures[mat];

        obj.SetActive(true);

        //Throw Object
        obj.GetComponent<Rigidbody>().AddForce(Vector3.back * throwForce);
    }
}
