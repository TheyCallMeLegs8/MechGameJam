using System.Collections.Generic;
using UnityEngine;

public class CameraMiniGame : MiniGameBase
{
    [SerializeField] private List<PuzzleLight> _lights = new List<PuzzleLight>();
    [SerializeField] private int _startOnLights = 3;
    private bool _canEndMinigame = true;

    private int _onLights = 0;

    private void OnEnable()
    {
        foreach (PuzzleLight lights in _lights)
        {
            lights.OnTurnLightOn.AddListener(CheckOffLight);
            lights.OnTurnLightOff.AddListener(CheckOffLight);
        }
        _canEndMinigame = true;
    }

    private void OnDisable()
    {
        foreach (PuzzleLight lights in _lights)
        {
            lights.OnTurnLightOn.RemoveListener(CheckOffLight);
            lights.OnTurnLightOff.RemoveListener(CheckOffLight);
        }
        _canEndMinigame = false;
    }

    private void Update()
    {
        // fixes glitch where you can complete it without evervy light being on
        if(_canEndMinigame && _onLights == _lights.Count)
        {
            OnGameComplete.Invoke();
            _canEndMinigame = false;
        }
    }

    public override void StartMinigame()
    {
        base.StartMinigame();
        foreach (PuzzleLight light in _lights)
        {
            light.StartOff();
        }

        //List<PuzzleLight> savedLights = new List<PuzzleLight>();
        List<PuzzleLight> lights = new List<PuzzleLight>();
        for (int i = 0; i < _lights.Count; i++)
        {
            lights.Add(_lights[i]);
        }


        for (int i = 0; i < _startOnLights; i++)
        {
            // gets random lights from light list and turns them on
            int indexToTurnOn = Random.Range(0, lights.Count);
            lights[indexToTurnOn].Switch();
            //savedLights.Add(lights[indexToTurnOn]);
            lights.Remove(lights[indexToTurnOn]);
        }
    }

    private void CheckLights()
    {
        _onLights = 0;
        for(int i = 0; i < _lights.Count; i++)
        {
            if(!_lights[i].IsOff) _onLights++;
        }

        CheckOffLight();
    }

    private void CheckOffLight()
    {
        _onLights = 0;
        for (int i = 0; i < _lights.Count; i++)
        {
            if (_lights[i].IsOn)
            {
                _onLights++;
                continue;
            }
            else
            {
                _onLights--;
                return;
            }
        }
    }

    private void LightOff()
    {
        _onLights--;

        CheckOffLight();
    }
}
