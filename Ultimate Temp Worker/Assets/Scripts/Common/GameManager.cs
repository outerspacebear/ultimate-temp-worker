using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<int> miniGameSceneIndices;
    int currentMiniGameIndex = -1;

    public static List<int> loadedMiniGameSceneIndicesInOrder = new List<int>();

    private void Awake()
    {
        MiniGameEvents.miniGameEndedEvent.AddListener(LoadNextMiniGame);

        DontDestroyOnLoad(gameObject);

        //needed for persistence on iOS
        System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    public void StartGame()
    {
        loadedMiniGameSceneIndicesInOrder = new List<int>();
        Shuffle(miniGameSceneIndices);
        CurrencyUtils.ResetAllCurrencies();
        LoadNextMiniGame();
    }

    void LoadNextMiniGame()
    {
        ++currentMiniGameIndex;

        if(currentMiniGameIndex >= miniGameSceneIndices.Count)
        {
            OnGameEnded();
            return;
        }

        loadedMiniGameSceneIndicesInOrder.Add(miniGameSceneIndices[currentMiniGameIndex]);
        
        SceneManager.LoadScene(miniGameSceneIndices[currentMiniGameIndex], LoadSceneMode.Single);
    }

    void OnGameEnded()
    {
        SceneManager.LoadScene("Game End Scene", LoadSceneMode.Single);
    }

    private static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}
