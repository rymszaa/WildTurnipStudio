using UnityEngine;

public class PlayerSetLoadetData : MonoBehaviour
{
    private void Awake()
    {
        var data = LoadSeve.loadedPlayer;
        if (data == null) return;

        transform.position = data.PlayerPosytion;
        transform.rotation = data.PlayerRotation;
    }
}
