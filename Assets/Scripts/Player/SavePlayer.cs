using System.IO;
using UnityEngine;

public class SavePlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    private const string folderPath = "WildTrunipStudio/Seves/Seve1";
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SavePoint")) return;
        Save();
    }
    private void Save()
    {
        PlayerInformations playerInforations = new PlayerInformations
        {
            PlayerPosytion=player.position,
            PlayerRotation=player.rotation,
        };
        string dir = Path.Combine(Application.persistentDataPath, folderPath);
        string json = JsonUtility.ToJson(playerInforations, true);
        string path = Path.Combine(dir, "player.json");

        File.WriteAllText(path, json);
    }
}
