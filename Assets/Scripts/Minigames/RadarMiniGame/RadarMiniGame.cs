using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RadarMiniGame : MiniGameBase
{
    [SerializeField] private List<RadarButtonGame> _radarButtons = new List<RadarButtonGame>();
    private List<RadarButtonGame> _currentRadarButtons = new List<RadarButtonGame>();
    private RadarButtonGame _currentReadyButton;

    [SerializeField] public UnityEvent OnGameStart = new UnityEvent();
    [SerializeField] public UnityEvent OnTurnAllOff = new UnityEvent();

    public override void StartMinigame()
    {
        base.StartMinigame();

        OnGameStart.Invoke();
        Reset();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _radarButtons.Count; i++)
        {
            _radarButtons[i].OnClick.AddListener(ClickButton);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _radarButtons.Count; i++)
        {
            _radarButtons[i].OnClick.RemoveListener(ClickButton);
        }
    }

    public void Reset()
    {
        TurnAllOff();
        FillButtonList();
        PickRandomFromList();
    }

    private void FillButtonList()
    {
        _currentRadarButtons.Clear();

        for(int i = 0;  i < _radarButtons.Count; i++)
        {
            _currentRadarButtons.Add(_radarButtons[i]);
        }
    }

    private void PickRandomFromList()
    {
        if (_currentRadarButtons.Count == 0)
        {
            EndMinigame();
            return;
        }

        int randomIndex = Random.Range(0, _currentRadarButtons.Count);
        _currentRadarButtons[randomIndex].SetReady();
        _currentReadyButton = _currentRadarButtons[randomIndex];
    }

    private void ClickButton(RadarButtonGame button)
    {
        if (button.IsOff) Reset();
        if (button.IsOn) return;
        if (button.IsReady)
        {
            button.TurnOn();
            _currentRadarButtons.Remove(button);
            PickRandomFromList();
        }
    }

    public void TurnAllOff()
    {
        foreach(RadarButtonGame button in _radarButtons)
        {
            button.TurnOff();
        }
        OnTurnAllOff.Invoke();
    }

    public override void EndMinigame()
    {
        base.EndMinigame();

        OnGameComplete.Invoke();
    }
}
