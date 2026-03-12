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
        _sliders.Clear();

        for (int i = 0;  i < _sliderSpawnPoints.Length; i++)
        {
            // selects random prefab
            int indexToSpawn = Random.Range(0, _sliderPrefabs.Length);
            Slider spawnedSlider = Instantiate(_sliderPrefabs[indexToSpawn], _sliderSpawnPoints[i]);
            _sliders.Add(spawnedSlider);
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
        int indexToBreak = Random.Range(0, 2);
        _sliders[indexToBreak].SetBroken();
    }

    public void DestroyAllSliders()
    {
        foreach (Slider sliders in _sliders)
        {
            Destroy(sliders.gameObject);
        }
    }

    private void SpawnRepairSlider()
    {

    }
}
