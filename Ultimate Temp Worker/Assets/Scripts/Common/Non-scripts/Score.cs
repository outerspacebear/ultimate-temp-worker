using System;
using System.Collections.Generic;

[System.Serializable]
public class Score
{
    public string name;
    public float score;
}

[System.Serializable]
public class Scores
{
    public List<Score> scores;
}
