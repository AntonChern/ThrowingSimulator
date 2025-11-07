using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerManager : MonoBehaviour, IPlayMode
{
    private void Start()
    {
        GenerateCrates();
        GeneratePlayer();
    }

    private void GenerateCrates()
    {
        int cratesNum = (int)Random.Range(5, 8.5f);
        for (int i = 0; i < cratesNum; i++)
        {
            GameObject crate = Instantiate(
                GameManager.Instance.cratePrefab,
                new Vector3(Random.Range(-7f, 7f), Random.Range(1f, 3f), Random.Range(-6, 1)),
                Quaternion.Euler(new Vector3(0f, 0f, 0f))
            );
            var synchronizer = crate.GetComponent<CrateSynchronizationHandler>();
            synchronizer.Index = i;
            synchronizer.IsAuthor = true;
            crate.transform.localScale = Vector3.one * Random.Range(0.6f, 1f);
        }
    }

    private void GeneratePlayer()
    {
        GameObject newPlayer = Instantiate(
            GameManager.Instance.playerPrefab,
            new Vector3(0f, 1f, 0f),
            Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        newPlayer.AddComponent<PlayerMovement>();
        newPlayer.AddComponent<PlayerInteraction>();
        var synchronizer = newPlayer.GetComponent<PlayerSynchronizationHandler>();
        synchronizer.Id = "Name";
        synchronizer.IsAuthor = true;
    }

    public void SendCrateInteracting(string id, int index, bool isTaken)
    {
        Debug.Log($"No implementation");
    }

    public void SendCrateMoving(int index)
    {
        Debug.Log($"No implementation");
    }

    public void SendPlayerMoving(string id)
    {
        Debug.Log($"No implementation");
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
