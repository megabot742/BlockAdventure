using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestNewShape : MonoBehaviour
{
    [SerializeField] int numberOfRequests = 3;//default 3
    [SerializeField] TMP_Text numberText;
    [SerializeField] Button requestNewShapeButton;

    private int currentNumberOfRequests;
    private bool isLocked;
    void Start()
    {
        currentNumberOfRequests = numberOfRequests;
        numberText.text = currentNumberOfRequests.ToString();
        UnClock();
    }

    public void RequestNewShapeButton()
    {
        if (isLocked == false)
        {
            currentNumberOfRequests--;
            GameEvents.RequestNewShape();
            GameEvents.CheckIfPlayerLost();

            if (currentNumberOfRequests <= 0)
            {
                Lock();
            }
            numberText.text = currentNumberOfRequests.ToString();
        }
    }
    void Lock()
    {
        isLocked = true;
        requestNewShapeButton.interactable = false;
        numberText.text = currentNumberOfRequests.ToString();
    }
    void UnClock()
    {
        isLocked = false;
        requestNewShapeButton.interactable = true;
    }
}
