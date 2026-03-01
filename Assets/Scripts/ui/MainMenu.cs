using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void Continue()
    {
        LoadSeve.LoadGame("SampleScene");
    }

    public void NewGame()
    {
        CreateNewGame.DeleteSeve();
        CreateNewGame.CreateSeve();
        SceneManager.LoadScene("SampleScene");
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
