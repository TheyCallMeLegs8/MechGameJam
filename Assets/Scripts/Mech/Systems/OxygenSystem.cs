using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OxygenSystem : MonoBehaviour
{
    private bool _isPoisoned = false;
    private bool _canBePoisoned = true;
    [SerializeField] private float _poisonSpeed = 5.0f;
    [SerializeField] private float _cleanAirSpeed = 5.0f;
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private GameObject _isPoisonedText;
    [SerializeField] private GameObject _isCleaningText;
    private Coroutine _poisonCoroutine;
    private Coroutine _purifyCoroutine;

    public void StartPoison()
    {
        _isPoisonedText.SetActive(true);
        _isCleaningText.SetActive(false);
        Debug.Log("Started Poison");
        _poisonCoroutine = StartCoroutine(PoisonRoutine());
    }

    private IEnumerator PoisonRoutine()
    {
        while (_isPoisoned && _canBePoisoned)
        {
            _slider.value -= _poisonSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void EndPoison()
    {
        _isPoisonedText.SetActive(false);
        _isCleaningText.SetActive(true);
        Debug.Log("Ended Poison");
        _purifyCoroutine = StartCoroutine(CleanAirRoutine());
    }

    private IEnumerator CleanAirRoutine()
    {
        while (!_isPoisoned || !_canBePoisoned)
        {
            _slider.value += _cleanAirSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void TurnPurifierOn()
    {
        _canBePoisoned = false;
        if (_isPoisoned)
        {
            EndPoison();
        }
    }

    public void TurnPurifierOff()
    {
        _canBePoisoned = true;
        if (_isPoisoned)
        {
            StartPoison();
        }
    }

    public void SetIsPoisoned(bool isPoisoned)
    {
        _isPoisoned = isPoisoned;
        if (_isPoisoned == true)
        {
            StartPoison();
        }
        else
        {
            EndPoison();
        }
    }

    public void ResetPoisonAmount()
    {
        _slider.value = 1;
    }
}
