using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class DataManager : MonoBehaviour
{
    private static string savePath = ".\\Assets\\Save\\user_data.txt";

    private static int LINES_USER_DATA = 2;
    private static int LINE_HIGH_SCORE = 0;

    void Start()
    {
        prepareUserData();
    }

    void Update()
    {

    }

    private void prepareUserData()
    {
        if (!File.Exists(savePath))
        {
            StreamWriter myFile = new StreamWriter(savePath);

            try
            {
                myFile.WriteLine("high_score:0");
                myFile.WriteLine("player:0");
            }
            finally
            {
                myFile.Close();
            }
        }
    }

    public int readMaxScore()
    {
        int maxScore = -1;

        StreamReader myReadFile = new StreamReader(savePath);
        int count = 0;

        while (count < LINES_USER_DATA)
        {
            string line = myReadFile.ReadLine();

            if (count == LINE_HIGH_SCORE)
            {
                int separatorPosition = line.IndexOf(":");

                int.TryParse(line.Substring(separatorPosition + 1), out maxScore);
            }
            ++count;
        }
        myReadFile.Close();

        return maxScore;
    }

    public void saveMaxScore(int score)
    {
        string[] lines = new string[LINES_USER_DATA];
        int count = 0;

        StreamReader myReadFile = new StreamReader(savePath);

        while (count < LINES_USER_DATA)
        {
            string line = myReadFile.ReadLine();
            lines[count] = line;

            if (count == LINE_HIGH_SCORE)
            {
                int separatorPosition = lines[count].IndexOf(":");
                string key = lines[count].Substring(0, separatorPosition);
                int value = score;
                lines[count] = key + ":" + value;
            }
            ++count;
        }
        myReadFile.Close();

        StreamWriter myWriteFile = new StreamWriter(savePath);

        try
        {
            for (int i = 0; i < LINES_USER_DATA; ++i)
            {
                myWriteFile.WriteLine(lines[i]);
            }
        }
        finally
        {
            myWriteFile.Close();
        }
    }
}
