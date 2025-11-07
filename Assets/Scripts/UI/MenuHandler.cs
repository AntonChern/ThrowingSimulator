using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private Button singleplayerButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        singleplayerButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("Mode", "Singleplayer");
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
        multiplayerButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("Mode", "Multiplayer");
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
