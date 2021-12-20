using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    public GameObject replacementObject1;
    public GameObject replacementObject2;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Blade")
        {
            //play sound
            CreateReplacementObjects();
            DestroyCurrentObject();
        }
    }

    void CreateReplacementObjects()
    {
        var currentPosition = gameObject.transform.position;

        const float offset = 0.9f;

        var positionToSpawn1At = new Vector3(currentPosition.x - offset, currentPosition.y, currentPosition.z);
        var positionToSpawn2At = new Vector3(currentPosition.x + offset, currentPosition.y, currentPosition.z);

        var spawnedPrefab1 = GameObject.Instantiate(replacementObject1, positionToSpawn1At, gameObject.transform.rotation, gameObject.transform.parent);
        var spawnedPrefab2 = GameObject.Instantiate(replacementObject2, positionToSpawn2At, gameObject.transform.rotation, gameObject.transform.parent);

        float forceMultiplier = Random.Range(200, 300);
        spawnedPrefab1.GetComponent<Rigidbody2D>().AddForce(-transform.right * forceMultiplier);
        spawnedPrefab2.GetComponent<Rigidbody2D>().AddForce(transform.right * forceMultiplier);

        Destroy(spawnedPrefab1, 4f);
        Destroy(spawnedPrefab2, 4f);
    }

    void DestroyCurrentObject()
    {
        Destroy(gameObject);
    }
}