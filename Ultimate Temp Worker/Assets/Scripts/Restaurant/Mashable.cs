using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashable : MonoBehaviour
{
    public GameObject replacementPrefab;

    private void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (IsTouchOnObject(touch) && IsTouchStationary(touch))
            {
                OnMash();
                break;
            }
        }
    }

    static bool IsTouchStationary(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                return true;
        }

        return false;
    }

    bool IsTouchOnObject(Touch touch)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject == gameObject)
                return true;    
        }

        return false;
    }

    public GameObject mashEffectPrefab;

    void OnMash()
    {
        var mashEffect = GameObject.Instantiate(mashEffectPrefab, transform.position + mashEffectPrefab.transform.position, transform.rotation, transform.parent);
        Destroy(mashEffect, 1);

        //Play sound
        TryRewardCurrency();
        CreateReplacementPrefab();
        Destroy(this.gameObject);
    }

    void CreateReplacementPrefab()
    {
        var spawnedPrefab = GameObject.Instantiate(replacementPrefab, gameObject.transform.position + replacementPrefab.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        Destroy(spawnedPrefab, 1.5f);
    }

    public GameObject currencyCoinPrefab;

    void TryRewardCurrency()
    {
        var currencyRewardComponent = GetComponent<CurrencyReward>();
        if (!currencyRewardComponent)
            return;

        var spawnedPrefab = GameObject.Instantiate(currencyCoinPrefab, gameObject.transform.position + currencyCoinPrefab.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
        Destroy(spawnedPrefab, 1f);

        //sound effect?
        CurrencyUtils.AddCurrencyForGame(CurrencyUtils.RestaurantGameName, currencyRewardComponent.currencyReward);
    }
}
