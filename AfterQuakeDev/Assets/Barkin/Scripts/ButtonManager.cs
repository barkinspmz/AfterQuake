using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void ClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
