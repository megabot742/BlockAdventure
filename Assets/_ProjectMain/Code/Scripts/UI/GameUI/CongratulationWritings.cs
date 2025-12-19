using System.Collections.Generic;
using UnityEngine;

public class CongratulationWritings : MonoBehaviour
{
    public List<GameObject> writings;
    void Start()
    {
        GameEvents.ShowCongralationWriting += ShowCongratulationWriting;
    }
    void OnDisable()
    {
        GameEvents.ShowCongralationWriting -= ShowCongratulationWriting;
    }
    void ShowCongratulationWriting()
    {
        int index = Random.Range(0, writings.Count);
        writings[index].SetActive(true);
    }

}
