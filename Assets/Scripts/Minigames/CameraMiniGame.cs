using System.Collections.Generic;
using UnityEngine;

public class CameraMiniGame : MiniGameBase
{
    [SerializeField] private List<PuzzleLight> _lights = new List<PuzzleLight>();
    [SerializeField] private int _startOnLights = 3;

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
}
