using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassManager : MonoBehaviour
{
    [Serializable]
    public struct GlassPosition
    {
        public string name;
        public Transform position;
        public bool isSpawnable;
    }

    public List<GlassPosition> spawnablePositions;
    public Dictionary<string, Transform> test;
    public GameObject prefabGlass;

    private GameObject currentGlass;
    private GlassPosition currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        InitGlassAtRandom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGlassAtRandom() 
    {
        bool isSpawnable = false;
        int spawnIndex;
        Transform spawnPoint = spawnablePositions[0].position;

        while (!isSpawnable)
        {
            spawnIndex = UnityEngine.Random.Range(0, spawnablePositions.Count);
            isSpawnable = spawnablePositions[spawnIndex].isSpawnable;
            spawnPoint = spawnablePositions[spawnIndex].position;
            currentPosition = spawnablePositions[spawnIndex];
        }

        currentGlass = Instantiate(prefabGlass, spawnPoint.position, spawnPoint.rotation);

        foreach (Transform child in currentGlass.transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }
    }

    public void PourIce()
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Ice" && currentPosition.name == "Ice")
            {
                renderer.enabled = true;
            }
        }
    }

    public void FillWithRed()
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail" && currentPosition.name == "Red")
            {
                renderer.material.color = Color.red;
                renderer.enabled = true;
            }
        }
    }

    public void FillWithYellow()
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail" && currentPosition.name == "Yellow")
            {
                renderer.material.color = Color.yellow;
                renderer.enabled = true;
            }
        }
    }

    public void FillWithBlue()
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail" && currentPosition.name == "Blue")
            {
                renderer.material.color = Color.blue;
                renderer.enabled = true;
            }
        }
    }

   
    public void FillWithWhite()
    {
        foreach (var renderer in currentGlass.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == "Cocktail" && currentPosition.name == "White")
            {
                renderer.material.color = Color.white;
                renderer.enabled = true;
            }
        }
    }
}
