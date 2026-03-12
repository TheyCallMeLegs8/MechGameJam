using System.Collections;
using UnityEngine;

public class FreqMiniGame : MiniGameBase
{
    [field: SerializeField] public LineRenderer LineRenderer { get; private set; }
    [field: SerializeField] public int Points { get; private set; }
    [field: SerializeField] public float Amplitude { get; private set; } = 1.0f;
    [field: SerializeField] public float Frequency { get; private set; } = 1.0f;
    [field: SerializeField] public Vector2 XLimits { get; private set; } = new Vector2(0, 1);
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1.0f;
    [SerializeField] protected FreqMiniGame _miniGameToMatch;

    [Header("Modifiable")]
    [SerializeField] private bool _canBeModified = false;
    [SerializeField] private float _modifyAmplitudeSpeed = 2.0f;
    [SerializeField] private float _modifyFrequencySpeed = 2.0f;
    [SerializeField] protected float _maxAmplitude = 0.5f;
    [SerializeField] protected float _maxFrequency = 3.0f;
    [SerializeField] protected float _minAmplitude = 0.5f;
    [SerializeField] protected float _minFrequency = 3.0f;
    private bool _isModifyingAmplitude = false;
    private bool _isModifyingFrequency = false;
    [SerializeField] protected float _matchRange = 0.4f;

    [Header("Randomize")]
    [SerializeField] private bool _randomizeOnStart = true;
    [SerializeField] private float _ampRandomRange = 0.2f;
    [SerializeField] private float _freqRandomRange = 3.0f;

    public override void StartMinigame()
    {
        base.StartMinigame();

        if(!_randomizeOnStart) return;
        Amplitude = Random.Range(_minAmplitude, _ampRandomRange);
        Frequency = Random.Range(_minFrequency, _freqRandomRange);
    }

    void Draw()
    {
        float xStart = XLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = XLimits.y;

        LineRenderer.positionCount = Points;
        for (int currentPoint = 0; currentPoint < Points; currentPoint++)
        {
            float progress = (float)currentPoint / (Points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = Amplitude * Mathf.Sin((Tau * Frequency * x) + (Time.timeSinceLevelLoad * MoveSpeed));
            LineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    void Update()
    {
        Draw();
    }

    public void ModifyAmplitudeUp()
    {
        if (!_canBeModified) return;
        _isModifyingAmplitude = true;
        StartCoroutine(ModifyAmplitudeRoutine(_modifyAmplitudeSpeed));
    }

    public void ModifyAmplitudeDown()
    {
        if (!_canBeModified) return;
        _isModifyingAmplitude = true;
        StartCoroutine(ModifyAmplitudeRoutine(-_modifyAmplitudeSpeed));
    }

    public void ModifyFrequencyLeft()
    {
        if (!_canBeModified) return;
        _isModifyingFrequency = true;
        StartCoroutine(ModifyFrequencyRoutine(-_modifyFrequencySpeed));
    }

    public void ModifyFrequencyRight()
    {
        if (!_canBeModified) return;
        _isModifyingFrequency = true;
        StartCoroutine(ModifyFrequencyRoutine(_modifyFrequencySpeed));
    }

    public void StopModifyAmplitude()
    {
        _isModifyingAmplitude = false;
    }

    public void StopModifyFrequency()
    {
        _isModifyingFrequency = false;
    }

    private IEnumerator ModifyAmplitudeRoutine(float speed)
    {
        while (_isModifyingAmplitude)
        {
            Amplitude = Mathf.Clamp(Amplitude + speed * Time.deltaTime, _minAmplitude, _maxAmplitude);
            if(Mathf.Abs(_miniGameToMatch.Amplitude - Amplitude) < _matchRange && Mathf.Abs(_miniGameToMatch.Frequency - Frequency) < _matchRange)
            {
                Amplitude = _miniGameToMatch.Amplitude;
                Frequency = _miniGameToMatch.Frequency;
            }
            yield return null;
        }
    }

    private IEnumerator ModifyFrequencyRoutine(float speed)
    {
        while (_isModifyingFrequency)
        {
            Frequency = Mathf.Clamp(Frequency + speed * Time.deltaTime, _minFrequency, _maxFrequency);
            if (Mathf.Abs(_miniGameToMatch.Amplitude - Amplitude) < _matchRange && Mathf.Abs(_miniGameToMatch.Frequency - Frequency) < _matchRange)
            {
                Amplitude = _miniGameToMatch.Amplitude;
                Frequency = _miniGameToMatch.Frequency;
            }
            yield return null;
        }
    }
}
