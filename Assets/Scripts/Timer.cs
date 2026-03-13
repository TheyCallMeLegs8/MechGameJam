using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _timerText;
    [SerializeField] private float _startTime = 5.0f;
    [SerializeField] public UnityEvent OnCountdownComplete = new UnityEvent();
    private bool _canTimerStart = true;
    private Coroutine _timerCoroutine;

    public void StartCountdown()
    {
        _canTimerStart = true;
        _timerCoroutine = StartCoroutine(CountdownRoutine());
    }

    public void ResetCountdown()
    {
        StopCoroutine(_timerCoroutine);
        _timerCoroutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        float time = _startTime;
        while(time > 0)
        {
            if(!_canTimerStart) break;

            time -= Time.deltaTime;
            _timerText.text = time.ToString("0.00");
            yield return null;
        }
        if (_canTimerStart)
        {
            OnCountdownComplete.Invoke();
        }
    }

    public void StopTimer()
    {
        _canTimerStart = false;
        StopCoroutine(_timerCoroutine);
    }
}
