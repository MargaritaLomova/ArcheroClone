using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Components")]
    [Header("Buttons")]
    [SerializeField]
    private Button _pauseButton;
    [SerializeField]
    private Button _restartButton;
    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private Button _exitButton;

    [Space]
    [Header("Other")]
    [SerializeField]
    private CanvasGroup _pauseMenu;
    [SerializeField]
    private GameObject _pauseIcon;
    [SerializeField]
    private GameObject _cancelIcon;

    [Space]
    [Header("Variables")]
    [SerializeField]
    private float _animationSpeed = 0.5f;

    private bool _isPause;

    private void Start()
    {
        _isPause = false;
        _pauseIcon.SetActive(!_isPause);
        _cancelIcon.SetActive(_isPause);
        SetPauseMenuActive(_isPause, true);

        _pauseButton.onClick.AddListener(() => PauseUnpause());
        _restartButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single));
        _menuButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single));
        _exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void PauseUnpause()
    {
        _isPause = !_isPause;

        _pauseIcon.SetActive(!_isPause);
        _cancelIcon.SetActive(_isPause);

        SetPauseMenuActive(_isPause);
    }

    private void SetPauseMenuActive(bool value, bool isFast = false)
    {
        if (!isFast)
        {
            if (value)
            {
                Time.timeScale = 0;
                _pauseMenu.blocksRaycasts = true;
                _pauseMenu.interactable = true;
                _pauseMenu.DOFade(1f, _animationSpeed).SetUpdate(true);
            }
            else
                _pauseMenu.DOFade(0f, _animationSpeed).SetUpdate(true).OnComplete(() =>
                {
                    Time.timeScale = 1;
                    _pauseMenu.blocksRaycasts = false;
                    _pauseMenu.interactable = false;
                });
        }
        else
        {
            if (value)
            {
                Time.timeScale = 0;
                _pauseMenu.blocksRaycasts = true;
                _pauseMenu.interactable = true;
                _pauseMenu.DOFade(1f, 0f).SetUpdate(true);
            }
            else
                _pauseMenu.DOFade(0f, 0f).SetUpdate(true).OnComplete(() =>
                {
                    Time.timeScale = 1;
                    _pauseMenu.blocksRaycasts = false;
                    _pauseMenu.interactable = false;
                });
        }
    }
}