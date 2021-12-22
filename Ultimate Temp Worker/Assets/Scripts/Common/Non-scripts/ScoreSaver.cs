using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class ScoreSaver
{
    static public void Save(Score score)
    {
        Scores savedScores = LoadSavedScores();
        if (savedScores == null)
        {
            savedScores = new Scores();
            savedScores.scores = new List<Score>();
        }

        savedScores.scores.Add(score);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/HighScores.dat");

        bf.Serialize(file, savedScores);
        file.Close();

        Debug.Log("Scores saved to: " + Application.persistentDataPath + "/HighScores.dat");
    }

    public static Scores LoadSavedScores()
    {
        if (File.Exists(Application.persistentDataPath + "/HighScores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/HighScores.dat", FileMode.Open);
            Scores sharedData = bf.Deserialize(fs) as Scores;
            fs.Close();

            return sharedData;
        }

        return null;
    }

    public static bool IsFirstTimePlaying()
    {
        if (File.Exists(Application.persistentDataPath + "/FirstTime.dat"))
        {
            return true;
        }
        return false;
    }

    public static void SaveIsFirstTimePlaying()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/FirstTime.dat");

        bf.Serialize(file, "no");
        file.Close();
    }
}
