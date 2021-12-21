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
            TryRewardCurrency();
            CreateReplacementObjects();
            DestroyCurrentObject();
        }
    }

    void CreateReplacementObjects()
    {
        var currentPosition = gameObject.transform.position;

        const float offset = 0.9f;

        var positionToSpawn1At = new Vector3(currentPosition.x - offset, currentPosition.y, currentPosition.z);

        var spawnedPrefab1 = GameObject.Instantiate(replacementObject1, positionToSpawn1At, gameObject.transform.rotation, gameObject.transform.parent);

        float forceMultiplier = Random.Range(200, 300);

        if (spawnedPrefab1.GetComponent<Rigidbody2D>())
        {
            spawnedPrefab1.GetComponent<Rigidbody2D>().AddForce(-transform.right * forceMultiplier);
        }

        Destroy(spawnedPrefab1, 4f);
        

        if(replacementObject2)
        {
            var positionToSpawn2At = new Vector3(currentPosition.x + offset, currentPosition.y, currentPosition.z);
            var spawnedPrefab2 = GameObject.Instantiate(replacementObject2, positionToSpawn2At, gameObject.transform.rotation, gameObject.transform.parent);

            if (spawnedPrefab2.GetComponent<Rigidbody2D>())
            {
                spawnedPrefab2.GetComponent<Rigidbody2D>().AddForce(transform.right * forceMultiplier);
            }
            Destroy(spawnedPrefab2, 4f);
        }
    }

    void DestroyCurrentObject()
    {
        Destroy(gameObject);
    }

    public GameObject currencyCoinPrefab;

    void TryRewardCurrency()
    {
        var spawnedPrefab = GameObject.Instantiate(currencyCoinPrefab, gameObject.transform.position + currencyCoinPrefab.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        Destroy(spawnedPrefab, 1f);

        //Show currency in UI? Or sound effect?
        var currencyRewardComponent = GetComponent<CurrencyReward>();
        if(currencyRewardComponent)
        {
            CurrencyUtils.AddCurrencyForGame(CurrencyUtils.RestaurantGameName, currencyRewardComponent.currencyReward);
        }
    }
}
