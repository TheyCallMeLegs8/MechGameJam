using System.Collections.Generic;
using UnityEngine;

public class LegMiniGame : MiniGameBase
{
    [SerializeField] private List<Slider> _sliders = new List<Slider>();
    [SerializeField] private List<RepairSliders> _repairSliders = new List<RepairSliders>();
    [SerializeField] private List<Transform> _repairSliderPoints = new List<Transform>();
    [SerializeField] private int _brokenLights = 1;

    public override void StartMinigame()
    {
        base.StartMinigame();


    }

    private void PickRandomSlider()
    {
        // get sliders from list
        List<Slider> sliders = new List<Slider>();
        for (int i = 0; i < _sliders.Count; i++)
        {
            sliders.Add(_sliders[i]);
        }

        for(int i = 0;  i < _brokenLights; i++)
        {
            int indexToBreak = Random.Range(0, _sliders.Count);
            sliders.Remove(sliders[indexToBreak]);


            //lights[indexToTurnOn].Switch();
            ////savedLights.Add(lights[indexToTurnOn]);
            //lights.Remove(lights[indexToTurnOn]);
        }
    }

    private void SpawnRepairSlider()
    {

    }
}
