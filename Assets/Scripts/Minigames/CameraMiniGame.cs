using System.Collections.Generic;
using UnityEngine;

public class CameraMiniGame : MiniGameBase
{
    [SerializeField] private List<PuzzleLight> _lights = new List<PuzzleLight>();
    [SerializeField] private int _startOnLights = 3;

    private void OnEnable()
    {
        foreach (PuzzleLight lights in _lights)
        {
            lights.OnTurnLightOn.AddListener(CheckLights);
        }
    }

    private void OnDisable()
    {
        foreach (PuzzleLight lights in _lights)
        {
            lights.OnTurnLightOn.RemoveListener(CheckLights);
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
        int onLights = 0;
        for(int i = 0; i < _lights.Count; i++)
        {
            if(_lights[i].IsOn) onLights++;
        }

        if(onLights >= _lights.Count)
        {
            OnGameComplete.Invoke();
        }
    }
}
