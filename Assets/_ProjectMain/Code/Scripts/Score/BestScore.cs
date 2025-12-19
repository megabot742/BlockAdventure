using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text bestScoreText;

    void OnEnable()
    {
        GameEvents.UpdateBestScoreBar += UpdateBestScoreBar;
    }
    void OnDisable()
    {
        GameEvents.UpdateBestScoreBar -= UpdateBestScoreBar;
    }
    void UpdateBestScoreBar(int currentScore, int bestScore)
    {
        float currentPrecentage = (float)currentScore / (float)bestScore;
        fillImage.fillAmount = currentPrecentage;
        bestScoreText.text = bestScore.ToString();
    }

}
