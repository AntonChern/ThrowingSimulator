using Colyseus;
using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour, IPlayMode
{
    private ColyseusClient client;
    private ColyseusRoom<ThrowingSimulatorState> room;
    public string localSessionId;

    private Dictionary<string, GameObject> spawnedPlayers = new Dictionary<string, GameObject>();
    private List<GameObject> spawnedCrates = new List<GameObject>();

    private async void Awake()
    {
        client = new ColyseusClient("ws://45.12.72.33:2567");
        //client = new ColyseusClient("ws://localhost:2567"); // Replace with your server address
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

    public void Exit()
    {
        room?.Leave();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        if (room == null) return;
        room.OnStateChange -= OnStateChangeHandler;
    }

    private void GenerateCrates(ThrowingSimulatorState state)
    {
        for (int i = 0; i < state.crates.Count; i++)
        {
            GameObject crate = Instantiate(
                GameManager.Instance.cratePrefab,
                new Vector3(state.crates[i].position.x, state.crates[i].position.y, state.crates[i].position.z),
                new Quaternion(state.crates[i].rotation.x, state.crates[i].rotation.y, state.crates[i].rotation.z, state.crates[i].rotation.w)
            );
            var synchronizer = crate.GetComponent<CrateSynchronizationHandler>();
            synchronizer.Index = i;
            synchronizer.IsAuthor = localSessionId == state.crates[i].author;
            crate.transform.localScale = Vector3.one * state.crates[i].scale;
            spawnedCrates.Add(crate);
        }
    }

    private void HandlePlayers()
    {
        var state = room.State;
        foreach (string key in state.players.Keys)
        {
            if (localSessionId == key) continue;
            var player = state.players[key];
            if (player.crateIndex + 1 > Mathf.Epsilon) // player.crateIndex != -1
            {
                var grabber = spawnedPlayers[key].GetComponent<Grabber>();
                if (!grabber.TakenObject)
                {
                    grabber.Take(spawnedCrates[(int)player.crateIndex], false);
                }
            }
            else
            {
                var grabber = spawnedPlayers[key].GetComponent<Grabber>();
                if (grabber.TakenObject)
                {
                    grabber.Throw(false);
                }
            }
            var synchronizer = spawnedPlayers[key].GetComponent<PlayerSynchronizationHandler>();
            synchronizer.UpdateActualState(
                new Vector3(player.position.x, player.position.y, player.position.z),
                new Quaternion(player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w)
            );
        }
    }

    private void HandleCrates()
    {
        var state = room.State;
        for (int i = 0; i < spawnedCrates.Count; i++)
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
        Debug.Log($"State changed by {state.lastChangedBy}!");
        if (isFirstState)
        {
            GenerateCrates(state);
            return;
        }
        if (localSessionId == state.lastChangedBy)
            return;
        HandlePlayers();
        HandleCrates();
    }

    void OnPlayerAdd(string key, Player player)
    {
        GameObject newPlayer = Instantiate(
            GameManager.Instance.playerPrefab,
            new Vector3(player.position.x, player.position.y, player.position.z),
            new Quaternion(player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w));
        if (key == localSessionId)
        {
            newPlayer.AddComponent<PlayerMovement>();
            newPlayer.AddComponent<PlayerInteraction>();
        }
        spawnedPlayers.Add(key, newPlayer);
        var synchronizer = newPlayer.GetComponent<PlayerSynchronizationHandler>();
        synchronizer.Id = key;
        synchronizer.IsAuthor = key == localSessionId;
    }

    void OnPlayerRemove(string key, Player player)
    {
        Debug.Log("Player removed: " + key);
        if (spawnedPlayers.ContainsKey(key))
        {
            spawnedPlayers[key].GetComponent<Grabber>().Throw(false);
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

    public void SendCrateAuthority(string id, int index)
    {
        if (room != null)
        {
            room.Send("change_authority", new
            {
                index = index,
                author = id
            });
        }
    }

    void OnApplicationQuit()
    {
        room?.Leave();
    }
}