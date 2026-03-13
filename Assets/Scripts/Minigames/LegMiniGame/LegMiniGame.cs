using System.Collections.Generic;
using UnityEngine;

public class LegMiniGame : MiniGameBase
{
    [SerializeField] private Slider[] _sliderPrefabs;
    [SerializeField] private Slider[] _repairSliderPrefabs;
    private List<Slider> _sliders = new List<Slider>();
    private List<Slider> _currentRepairSliders = new List<Slider>();
    [SerializeField] private List<Transform> _repairSliderPoints = new List<Transform>();
    [SerializeField] private Transform[] _sliderSpawnPoints;
    [SerializeField] private Transform _teleportPoint;
    private Slider _brokenSlider;

    public override void StartMinigame()
    {
        base.StartMinigame();

        PickRandomSliders();
        FixAllSliders();
        BreakRandomSlider();
        SpawnRepairSliders();
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
        _brokenSlider = _sliders[indexToBreak];
        _brokenSlider.OnDispose.AddListener(DisposeSlider);
    }

    private void SpawnRepairSliders()
    {
        List<Slider> repairSliders = new List<Slider>();
        repairSliders.Clear();

        Slider goalSlider = null;

        // fills repairSliders List & sets goal slider
        for (int i = 0; i < _repairSliderPrefabs.Length; i++)
        {
            repairSliders.Add(_repairSliderPrefabs[i]);

            // doesn't set goal slider if not match ID
            if (_repairSliderPrefabs[i].Id != _brokenSlider.Id) continue;
            goalSlider = _repairSliderPrefabs[i];
        }

        // selects which of 3 points will have the correct one
        int indexToMatch = Random.Range(0, _repairSliderPoints.Count);
        Debug.Log(indexToMatch);
        
        for(int i = 0; i < _repairSliderPoints.Count; i++)
        {
            // make sure correct one matches the broken one
            if(i  == indexToMatch)
            {
                repairSliders.Remove(goalSlider);
                goalSlider = Instantiate(goalSlider, _repairSliderPoints[i]);
                _currentRepairSliders.Add(goalSlider);
                continue;
            }

            // spawn random one from prefabs
            int indexToSpawn = Random.Range(0, _repairSliderPrefabs.Length);

            for(int j = 0; j < _repairSliderPrefabs.Length; j++)
            {
                // || repairSliders[indexToSpawn].Id == _currentRepairSliders[i].Id
                if (_repairSliderPrefabs[indexToSpawn].Id == _brokenSlider.Id) continue;

                Slider spawnedRepairSlider = Instantiate(_repairSliderPrefabs[j], _repairSliderPoints[i]);
                _currentRepairSliders.Add(spawnedRepairSlider);
            }

            Debug.Log(i);
        }
    }

    public void DestroyAllSliders()
    {
        foreach (Slider sliders in _sliders)
        {
            Destroy(sliders.gameObject);
        }
        foreach (Slider repairSliders in _currentRepairSliders)
        {
            Destroy(repairSliders.gameObject);
        }
        _currentRepairSliders.Clear();
    }

    public void DisposeSlider()
    {
        _brokenSlider._sliderMesh.transform.position = _teleportPoint.position;
    }

    private void OnDisable()
    {
        _brokenSlider.OnDispose.RemoveListener(DisposeSlider);
    }
}
