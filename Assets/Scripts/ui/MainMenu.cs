using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Continue()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void NewGame()
    {
        Debug.Log("add save system 2");
    }

    public void LoadGame()
    {
        Debug.Log("add save system 3");
    }

    public void OpenSettings()
    {
        Debug.Log("add settings");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
