using UnityEngine;
using UnityEngine.UI;

public class PauseController : BaseInGameMenuController
{
    [Header("Pause Buttons")]
    [SerializeField]
    private Button _pauseButton;

    [Space]
    [Header("Other")]
    [SerializeField]
    private GameObject _pauseIcon;
    [SerializeField]
    private GameObject _cancelIcon;

    private bool _isPause;

    protected override void Start()
    {
        base.Start();

        _isPause = false;
        _pauseIcon.SetActive(!_isPause);
        _cancelIcon.SetActive(_isPause);

        _pauseButton.onClick.AddListener(() => PauseUnpause());
    }

    private void PauseUnpause()
    {
        _isPause = !_isPause;

        _pauseIcon.SetActive(!_isPause);
        _cancelIcon.SetActive(_isPause);

        SetMenuActive(_isPause);
    }
}