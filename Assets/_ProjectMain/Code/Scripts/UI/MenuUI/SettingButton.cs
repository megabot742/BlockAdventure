using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField] Button openSettingBtn;
    [SerializeField] Button closeSettingBtn;
    [SerializeField] Animator settingAnimator;

    int openParam = Animator.StringToHash("OpenSettings");
    int closeParam = Animator.StringToHash("CloseSettings");

    public void SettingsOpened()
    {
        openSettingBtn.gameObject.SetActive(false);
        closeSettingBtn.gameObject.SetActive(true);
        closeSettingBtn.interactable = true;
        settingAnimator.SetTrigger(openParam);
    }
    public void SettingsClose()
    {
        openSettingBtn.gameObject.SetActive(true);
        openSettingBtn.interactable = true;
        closeSettingBtn.gameObject.SetActive(false);
        settingAnimator.SetTrigger(closeParam);
    }
}
