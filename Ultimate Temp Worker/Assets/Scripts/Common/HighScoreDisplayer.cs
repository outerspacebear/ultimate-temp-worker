using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplayer : MonoBehaviour
{
    public List<Text> scoreTexts;

    private void Start()
    {
        var allScores = ScoreSaver.LoadSavedScores();
        int i = 0;

        for(; i < allScores.scores.Count && i < scoreTexts.Count; ++i)
        {
            scoreTexts[i].text = allScores.scores[i].score.ToString();
        }

        for(; i < scoreTexts.Count; ++i)
        {
            scoreTexts[i].transform.parent.gameObject.SetActive(false);
        }
    }
}
