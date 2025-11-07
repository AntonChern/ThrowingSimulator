using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private IPlayMode playMode;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject cratePrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string playerMode = PlayerPrefs.GetString("Mode");
        //if (playerMode == "Multiplayer")
        //{
            playMode = gameObject.AddComponent<MultiplayerManager>();
        //}
        //else
        //{
        //    playMode = gameObject.AddComponent<SingleplayerManager>();
        //}
    }

    public void SendCrateInteracting(string id, int index, bool isTaken)
    {
        playMode.SendCrateInteracting(id, index, isTaken);
    }

    public void SendCrateMoving(int index)
    {
        playMode.SendCrateMoving(index);
    }

    public void SendPlayerMoving(string id)
    {
        playMode.SendPlayerMoving(id);
    }
}
