using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] SquareTextureData squareTextureData;
    void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        squareTextureData.SetStartColor();
    }
}
