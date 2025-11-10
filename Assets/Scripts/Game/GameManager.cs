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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        string playerMode = PlayerPrefs.GetString("Mode");
        if (playerMode == "Multiplayer")
        {
            playMode = gameObject.AddComponent<MultiplayerManager>();
        }
        else
        {
            playMode = gameObject.AddComponent<SingleplayerManager>();
        }
    }

    public void SendCrateInteracting(string id, int index, bool isTaken)
    {
        playMode?.SendCrateInteracting(id, index, isTaken);
    }

    public void SendCrateMoving(int index)
    {
        playMode?.SendCrateMoving(index);
    }
    
    public void SendCrateVelocity(int index)
    {
        playMode?.SendCrateVelocity(index);
    }

    public void SendPlayerLocation(string id)
    {
        playMode?.SendPlayerLocation(id);
    }

    public void SendPlayerMovement(string id, Vector3 movement)
    {
        playMode?.SendPlayerMovement(id, movement);
    }

    public void SendCrateAuthority(string id, int index)
    {
        playMode?.SendCrateAuthority(id, index);
    }

    public void Exit()
    {
        playMode?.Exit();
    }
}
