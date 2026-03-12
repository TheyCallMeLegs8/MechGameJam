using System.Collections.Generic;
using UnityEngine;

public class LegMiniGame : MiniGameBase
{
    [SerializeField] private Slider[] _sliderPrefabs;
    private List<Slider> _sliders = new List<Slider>();
    [SerializeField] private List<RepairSliders> _repairSliders = new List<RepairSliders>();
    [SerializeField] private List<Transform> _repairSliderPoints = new List<Transform>();
    [SerializeField] private Transform[] _sliderSpawnPoints;

    public override void StartMinigame()
    {
        base.StartMinigame();

        PickRandomSliders();
        FixAllSliders();
        BreakRandomSlider();
    }

    private void PickRandomSliders()
    {
        for(int i = 0;  i < _sliderSpawnPoints.Length; i++)
        {
            // selects random prefab
            int indexToSpawn = Random.Range(0, _sliderPrefabs.Length);
            Instantiate(_sliderPrefabs[indexToSpawn], _sliderSpawnPoints[i]);
            _sliders.Add(_sliderPrefabs[indexToSpawn]);
        }
    }

    private void FixAllSliders()
    {
        foreach (Slider sliders in _sliders)
        {
            sliders.SetFixed();
        }
    }

    private void BreakRandomSlider()
    {        
        int indexToBreak = Random.Range(0, 1);
        Debug.Log(indexToBreak);
        Debug.Log(_sliders.Count);
        _sliders[indexToBreak].SetBroken();
    }

    private void SpawnRepairSlider()
    {

    }
}
