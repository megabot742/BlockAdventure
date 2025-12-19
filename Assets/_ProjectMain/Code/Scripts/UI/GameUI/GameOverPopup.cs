using TMPro;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{

    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject loosePopup;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] TMP_Text scoreText;
    void Start()
    {
        gameOverMenu.SetActive(false);
    }
    void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
        GameEvents.UpdateGameOverScore += UpdateGameOverScore;
    }
    void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
        GameEvents.UpdateGameOverScore -= UpdateGameOverScore;
    }
    private void OnGameOver(bool newBestScore)
    {
        gameOverMenu.SetActive(true);
        loosePopup.SetActive(true);
    }
    private void UpdateGameOverScore(int currentScore, int bestScore)
    {

        scoreText.text = currentScore.ToString();
        bestScoreText.text = bestScore.ToString();
    }
}
