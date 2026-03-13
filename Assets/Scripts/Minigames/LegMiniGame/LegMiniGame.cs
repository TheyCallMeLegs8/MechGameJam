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
    private Slider _correctRepairSlider;
    private Slider _brokenSlider;

    private Transform _brokenSliderTransform;
    private Transform _intialRepaitSliderTransform;

    private Vector3 _originalRepairPos;
    private int _repairSlidersInFixSlot = 0;

    public override void StartMinigame()
    {
        base.StartMinigame();

        _currentRepairSliders.Clear();
        _repairSlidersInFixSlot = 0;

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

        _brokenSliderTransform = _brokenSlider.GetComponentInParent<Transform>();
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

        bool hasBrokenOneSpawned = false;
        for (int i = 0; i < _repairSliderPoints.Count; i++)
        {
            // make sure correct one matches the broken one
            // this should be PERFECT
            if (i == indexToMatch && hasBrokenOneSpawned == false)
            {
                repairSliders.Remove(goalSlider);
                goalSlider = Instantiate(goalSlider, _repairSliderPoints[i]);
                goalSlider.SetIsRepairSlider(true);

                goalSlider.OnClickRepair.AddListener(MoveRepairSlider);
                
                _correctRepairSlider = goalSlider;
                _currentRepairSliders.Add(goalSlider);
                continue;
            }

            // spawn random one from remaining repair sliders
            int indexToSpawn = Random.Range(0, repairSliders.Count);

            Slider spawnedRepairSlider = Instantiate(repairSliders[indexToSpawn], _repairSliderPoints[i]);
            spawnedRepairSlider.SetIsRepairSlider(true);

            spawnedRepairSlider.OnClickRepair.AddListener(MoveRepairSlider);

            // makes sure if the broken one hasn't spawned yet we don't spawn it again
            if (repairSliders[indexToSpawn].Id == goalSlider.Id)
            {
                hasBrokenOneSpawned = true;
                _correctRepairSlider = repairSliders[indexToSpawn];
            }

            repairSliders.Remove(repairSliders[indexToSpawn]);
            _currentRepairSliders.Add(spawnedRepairSlider);
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
            repairSliders.OnClickRepair.RemoveListener(MoveRepairSlider);
            Destroy(repairSliders.gameObject);
        }
        _currentRepairSliders.Clear();
    }

    private void MoveRepairSlider(GameObject repairSlider)
    {
        if(!_brokenSlider.IsDisposed) return;
        if (_repairSlidersInFixSlot == 0)
        {
            _originalRepairPos = repairSlider.transform.position;
            repairSlider.transform.position = _brokenSliderTransform.position;
            repairSlider.GetComponent<Slider>().SetIsRepairing(true);
            _repairSlidersInFixSlot++;

            if (repairSlider.GetComponent<Slider>().Id == _brokenSlider.Id)
            {
                _correctRepairSlider.OnRepairNewSlider.AddListener(EndGame);
                _brokenSlider.SetNewSliderForClamps(repairSlider.GetComponent<Slider>(), true);
            }
        }
        else if (_repairSlidersInFixSlot > 0 && repairSlider.GetComponent<Slider>().IsRepairing)
        {
            repairSlider.transform.position = _originalRepairPos;
            repairSlider.GetComponent<Slider>().SetIsRepairing(false);
            _repairSlidersInFixSlot--;

            if (repairSlider.GetComponent<Slider>().Id == _brokenSlider.Id)
            {
                _correctRepairSlider.OnRepairNewSlider.RemoveListener(EndGame);
                _brokenSlider.SetNewSliderForClamps(repairSlider.GetComponent<Slider>(), false);
            }
        }
    }

    public void DisposeSlider()
    {
        _brokenSlider._sliderMesh.transform.position = _teleportPoint.position;
        _brokenSlider.GetComponent<BoxCollider>().enabled = false;
    }

    private void EndGame()
    {
        // DO nend game stuff
        Debug.Log("Done");
    }

    private void OnDisable()
    {
        _correctRepairSlider.OnRepairNewSlider.RemoveListener(EndGame);
        _brokenSlider.OnDispose.RemoveListener(DisposeSlider);
    }
}
