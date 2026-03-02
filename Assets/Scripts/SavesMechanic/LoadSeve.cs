using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSeve : MonoBehaviour
{
    private const string folderPath = "WildTrunipStudio/Seves/Seve1";
    public static PlayerInformations loadedPlayer;

    public static void LoadGame(string sceneName)
    {
        var dir = Path.Combine(Application.persistentDataPath, folderPath);
        var path = Path.Combine(dir, "player.json");
        var json = File.ReadAllText(path);
        loadedPlayer = JsonUtility.FromJson<PlayerInformations>(json);
        SceneManager.LoadScene(sceneName);
    }
}