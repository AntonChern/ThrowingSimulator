using Colyseus;
using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private ColyseusClient client;
    private ColyseusRoom<ThrowingSimulatorState> room;
    public string localSessionId;

    public GameObject playerPrefab;
    public GameObject cratePrefab;
    private Dictionary<string, GameObject> spawnedPlayers = new Dictionary<string, GameObject>();
    private List<GameObject> spawnedCrates = new List<GameObject>();

    private async void Awake()
    {
        Instance = this;

        client = new ColyseusClient("ws://localhost:2567"); // Replace with your server address
        try
        {
            room = await client.JoinOrCreate<ThrowingSimulatorState>("my_room");
            Debug.Log("Joined room: " + room.RoomId);
            localSessionId = room.SessionId;

            var callbacks = Callbacks.Get(room);
            callbacks.OnAdd(state => state.players, (key, player) =>
            {
                OnPlayerAdd(key, player);
            });
            callbacks.OnRemove(state => state.players, (key, player) =>
            {
                OnPlayerRemove(key, player);
            });
            room.OnStateChange += OnStateChangeHandler;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to join room: " + e.Message);
        }
    }

    private void GenerateCrates(ThrowingSimulatorState state)
    {
        for (int i = 0; i < state.crates.Count; i++)
        {
            GameObject crate = Instantiate(
                cratePrefab,
                new Vector3(state.crates[i].position.x, state.crates[i].position.y, state.crates[i].position.z),
                Quaternion.Euler(state.crates[i].rotation.x, state.crates[i].rotation.y, state.crates[i].rotation.z)
            );
            crate.GetComponent<CrateSynchronizationHandler>().Index = i;
            crate.transform.localScale = Vector3.one * state.crates[i].scale;
            spawnedCrates.Add(crate);
        }
    }

    private void HandlePlayers(ThrowingSimulatorState state)
    {
        foreach (string key in state.players.Keys)
        {
            var player = state.players[key];
            if (player.crateIndex + 1 > Mathf.Epsilon) // player.crateIndex != -1
            {
                spawnedPlayers[key].GetComponent<Grabber>().Take(spawnedCrates[(int)player.crateIndex], false);
            }
            else
            {
                spawnedPlayers[key].GetComponent<Grabber>().Throw(false);
            }
            var synchronizer = spawnedPlayers[key].GetComponent<PlayerSynchronizationHandler>();
            synchronizer.UpdateActualState(
                new Vector3(player.position.x, player.position.y, player.position.z),
                new Quaternion(player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w)
            );
        }
    }

    private void HandleCrates(ThrowingSimulatorState state)
    {
        for (int i = 0; i < state.crates.Count; i++)
        {
            var synchronizer = spawnedCrates[i].GetComponent<CrateSynchronizationHandler>();
            synchronizer.IsAuthor = localSessionId == state.crates[i].author;
            synchronizer.UpdateActualState(
                new Vector3(state.crates[i].position.x, state.crates[i].position.y, state.crates[i].position.z),
                new Quaternion(state.crates[i].rotation.x, state.crates[i].rotation.y, state.crates[i].rotation.z, state.crates[i].rotation.w)
            );
        }
    }

    private void OnStateChangeHandler(ThrowingSimulatorState state, bool isFirstState)
    {
        Debug.Log("State changed!");
        if (isFirstState)
        {
            GenerateCrates(state);
            return;
        }
        HandlePlayers(state);
        HandleCrates(state);
    }

    void OnPlayerAdd(string key, Player player)
    {
        Debug.Log("Player added: " + key);
        GameObject newPlayer = Instantiate(
            playerPrefab,
            new Vector3(player.position.x, player.position.y, player.position.z),
            Quaternion.Euler(new Vector3(player.rotation.x, player.rotation.y, player.rotation.z)));
        if (key == localSessionId)
        {
            newPlayer.AddComponent<PlayerMovement>();
            newPlayer.AddComponent<PlayerInteraction>();
        }
        spawnedPlayers.Add(key, newPlayer);
        newPlayer.GetComponent<PlayerSynchronizationHandler>().Id = key;
        newPlayer.GetComponent<PlayerSynchronizationHandler>().IsAuthor = key == localSessionId;
    }

    void OnPlayerRemove(string key, Player player)
    {
        Debug.Log("Player removed: " + key);
        if (spawnedPlayers.ContainsKey(key))
        {
            Destroy(spawnedPlayers[key]);
            spawnedPlayers.Remove(key);
        }
    }

    public void SendPlayerMoving(string id)
    {
        if (room != null)
        {
            Transform transform = spawnedPlayers[id].transform;
            room.Send("move_player", new {
                id = id,
                posX = transform.position.x,
                posY = transform.position.y,
                posZ = transform.position.z,
                rotX = transform.rotation.x,
                rotY = transform.rotation.y,
                rotZ = transform.rotation.z,
                rotW = transform.rotation.w
            });
        }
    }

    public void SendCrateMoving(int index)
    {
        if (room != null)
        {
            Transform transform = spawnedCrates[index].transform;
            room.Send("move_crate", new
            {
                index = index,
                posX = transform.position.x,
                posY = transform.position.y,
                posZ = transform.position.z,
                rotX = transform.rotation.x,
                rotY = transform.rotation.y,
                rotZ = transform.rotation.z,
                rotW = transform.rotation.w,
            });
        }
    }

    public void SendCrateInteracting(string id, int index, bool isTaken)
    {
        if (room != null)
        {
            room.Send("interact_crate", new
            {
                owner = id,
                index = index,
                isTaken = isTaken
            });
        }
    }

    void OnApplicationQuit()
    {
        room?.Leave();
    }
}