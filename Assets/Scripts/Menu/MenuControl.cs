using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
