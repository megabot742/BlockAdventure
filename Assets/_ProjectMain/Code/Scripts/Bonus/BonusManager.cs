using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] List<GameObject> bonusList;

    void Start()
    {
        GameEvents.ShowBonusScreen += ShowBonusScreen;
    }
    void OnDisable()
    {
        GameEvents.ShowBonusScreen -= ShowBonusScreen;
    }
    private void ShowBonusScreen(List<Config.SquareColor> colors)
    {
        //Debug.Log("ShowBonusScreen called with colors: " + string.Join(", ", colors));
        StartCoroutine(ShowBonusesSequentially(colors));
    }

    IEnumerator ShowBonusesSequentially(List<Config.SquareColor> colors)
    {
        List<GameObject> activeBonuses = new List<GameObject>();
        
        // Duyệt qua từng màu để kích hoạt bonus tương ứng
        foreach (var color in colors)
        {
            bool bonusFound = false;
            foreach (var bonus in bonusList)
            {
                Bonus bonusComp = bonus.GetComponent<Bonus>();
                if (bonusComp != null && bonusComp.color == color)
                {
                    //Debug.Log($"Activating bonus for color: {color} (GameObject: {bonus.name})");
                    bonus.SetActive(true);
                    activeBonuses.Add(bonus);
                    bonusFound = true;
                    // Đợi 2 giây để hiển thị bonus này trước khi kích hoạt bonus tiếp theo
                    yield return new WaitForSeconds(2f);
                    // Tắt bonus ngay sau khi hiển thị xong
                    if (bonus != null)
                    {
                        //Debug.Log($"Deactivating bonus: {bonus.name}");
                        bonus.SetActive(false);
                    }
                }
            }
            if (!bonusFound)
            {
                //Debug.LogWarning($"No bonus found for color: {color}. Check bonusList or Bonus components.");
            }
            // Delay nhẹ giữa các bonus để tránh cảm giác giật
            yield return new WaitForSeconds(0.2f);
        }

        if (activeBonuses.Count == 0)
        {
            //Debug.LogWarning("No bonuses activated. Check bonusList or Bonus components.");
        }
    }
    
}
