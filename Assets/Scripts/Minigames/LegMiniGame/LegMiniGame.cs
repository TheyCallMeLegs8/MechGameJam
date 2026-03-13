using System.Collections.Generic;
using UnityEngine;

public class LegMiniGame : MiniGameBase
{
    [SerializeField] private Slider[] _sliderPrefabs;
    [SerializeField] private Slider[] _repairSliderPrefabs;
    private List<Slider> _sliders = new List<Slider>();
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
        Slider goalSlider = null;
        for(int i = 0; i < _repairSliderPrefabs.Length; i++)
        {
            if (_repairSliderPrefabs[i].Id != _brokenSlider.Id) continue;
            goalSlider = _repairSliderPrefabs[i];
        }

        int indexToMatch = Random.Range(0, _repairSliderPoints.Count);

        for(int i = 0; i < _repairSliderPoints.Count; i++)
        {
            // make sure correct one matches the broken one
            if(i  == indexToMatch)
            {
                Instantiate(goalSlider, _repairSliderPoints[i]);
                continue;
            }

            // spawn random one from prefabs
            int indexToSpawn = Random.Range(0, _repairSliderPoints.Count);

            // if it is a duplicate of the other repair one then skip
            if (_repairSliderPrefabs[indexToSpawn].Id == _brokenSlider.Id) continue;

            // spawn slider
            Slider spawnedRepairSlider = Instantiate(_repairSliderPrefabs[indexToSpawn], _repairSliderPoints[i]);
        }
    }

    public void DestroyAllSliders()
    {
        foreach (Slider sliders in _sliders)
        {
            Destroy(sliders.gameObject);
        }
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
