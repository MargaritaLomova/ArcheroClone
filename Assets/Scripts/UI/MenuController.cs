using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Button _startGameButton;
    [SerializeField]
    private Button _exitButton;

    private void Start()
    {
        _startGameButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single));
        _exitButton.onClick.AddListener(() => Application.Quit());
    }
}