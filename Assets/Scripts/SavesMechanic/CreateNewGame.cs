using System.IO;
using UnityEngine;

public class CreateNewGame : MonoBehaviour
{
    private const string folderPath = "WildTrunipStudio/Seves/Seve1";

    public static void DeleteSeve()
    {
        string dir = Path.Combine(Application.persistentDataPath, folderPath);
 

        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
    }

    public static void CreateSeve()
    {

        string dir = Path.Combine(Application.persistentDataPath, folderPath);
        Directory.CreateDirectory(dir);

        string path = Path.Combine(dir, "player.json");

        PlayerInformations playerInforations = new PlayerInformations
        {
            PlayerPosytion = new Vector3(0, 0.07f, 0),
            PlayerRotation = Quaternion.identity
        };

        string json = JsonUtility.ToJson(playerInforations,true);

        File.WriteAllText(path, json);
    }
}