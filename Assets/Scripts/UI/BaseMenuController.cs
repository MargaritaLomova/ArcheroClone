using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseMenuController : MonoBehaviour
{
    [Header("Components")]
    [Header("Buttons")]
    [SerializeField]
    protected Button _restartButton;
    [SerializeField]
    protected Button _exitButton;

    protected virtual void Start()
    {
        _restartButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single));
        _exitButton.onClick.AddListener(() => Application.Quit());
    }
}