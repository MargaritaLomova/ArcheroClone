using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseInGameMenuController : BaseMenuController
{
    [Header("In Game Buttons")]
    [SerializeField]
    protected Button _menuButton;

    [Header("In Game Other Components")]
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [Space]
    [Header("In Game Variables")]
    [SerializeField]
    private float _animationSpeed = 0.5f;

    protected override void Start()
    {
        base.Start();

        SetMenuActive(false, true);

        _menuButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single));
    }

    protected void SetMenuActive(bool value, bool isFast = false)
    {
        if (!isFast)
        {
            if (value)
            {
                Time.timeScale = 0;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
                _canvasGroup.DOFade(1f, _animationSpeed).SetUpdate(true);
            }
            else
                _canvasGroup.DOFade(0f, _animationSpeed).SetUpdate(true).OnComplete(() =>
                {
                    Time.timeScale = 1;
                    _canvasGroup.blocksRaycasts = false;
                    _canvasGroup.interactable = false;
                });
        }
        else
        {
            if (value)
            {
                Time.timeScale = 0;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
                _canvasGroup.DOFade(1f, 0f).SetUpdate(true);
            }
            else
                _canvasGroup.DOFade(0f, 0f).SetUpdate(true).OnComplete(() =>
                {
                    Time.timeScale = 1;
                    _canvasGroup.blocksRaycasts = false;
                    _canvasGroup.interactable = false;
                });
        }
    }
}