using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public List<GameObject> itemPrefabs;
    public Transform[] spawnPoints;

    public float minDelay = .1f;
    public float maxDelay = 1f;

    void Start()
    {
        StartCoroutine(SpawnPrefabs());
    }

    IEnumerator SpawnPrefabs()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];


            int itemIndex = Random.Range(0, itemPrefabs.Count);
            GameObject spawnedItem = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnPoint.rotation);

            RotateItem(spawnedItem);
            PushItemUpwards(spawnedItem);

            Destroy(spawnedItem, 5f);
        }
    }

    static void PushItemUpwards(GameObject item)
    {
        float forceMultiplier = Random.Range(650, 850);

        Rigidbody2D rigidbody = item.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(item.transform.up * forceMultiplier, ForceMode2D.Force);
    }

    static void RotateItem(GameObject item)
    {
        float rotationAngle = Random.Range(-40, 40);

        Vector3 rotationVector = new Vector3(0, 0, rotationAngle);
        item.transform.Rotate(rotationVector);
    }
}
