using System;
using System.Collections;
using TMPro;
using UnityEngine;
[System.Serializable]
public class BestScoreData
{
    public int score = 0;
}
public class Score : MonoBehaviour
{
    [SerializeField] SquareTextureData squareTextureData;
    [SerializeField] TMP_Text scoreText;
    private BestScoreData bestScore = new BestScoreData();
    private const string bestScoreKey = "bsdat"; //bsdat = best score data
    //C:\Users\megab\AppData\LocalLow\DefaultCompany\BlockAdventure\saves\bsdat.dat
    private bool newBestScore;
    private int currentScore;
    void Awake()
    {
        if (BinaryDataSteam.Exist(bestScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }
    IEnumerator ReadDataFile()
    {
        bestScore = BinaryDataSteam.Read<BestScoreData>(bestScoreKey);
        yield return new WaitForEndOfFrame(); 
        GameEvents.UpdateBestScoreBar(currentScore, bestScore.score); //display best score when start game first time
        Debug.Log("Read best score: " + bestScore.score);
    }
    void Start()
    {
        newBestScore = false;
        squareTextureData.SetStartColor();
        DisplayScore();
    }
    void OnEnable()
    {
        GameEvents.AddScores += AddScores;
        GameEvents.GameOver += SaveBestScore;
    }
    void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
        GameEvents.GameOver -= SaveBestScore;
    }
    private void SaveBestScore(bool newBestScoreValue)
    {
        BinaryDataSteam.Save(bestScore, bestScoreKey);
        GameEvents.UpdateGameOverScore?.Invoke(currentScore, bestScore.score); //CheckNull
    }
    private void AddScores(int score)
    {
        currentScore += score;
        if (currentScore > bestScore.score)
        {
            newBestScore = true;
            bestScore.score = currentScore;
            SaveBestScore(newBestScore); // true
        }
        UpdateSquareColor();
        GameEvents.UpdateBestScoreBar(currentScore, bestScore.score);
        DisplayScore();
    }
    private void UpdateSquareColor()
    {
        if (GameEvents.UpdateSquareColor != null && currentScore >= squareTextureData.tresholdVal)
        {
            squareTextureData.UpdateColors(currentScore);
            GameEvents.UpdateSquareColor(squareTextureData.currentColor);
        }
    }
    private void DisplayScore()
    {
        scoreText.text = currentScore.ToString();
    }
}
