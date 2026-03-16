using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    [Tooltip("Default is 1.0"), SerializeField] private float _clipVolume = 1.0f;
    [Tooltip("Default is 1.0"), SerializeField] private float _pitch = 1.0f;
    [SerializeField] private bool _loop = false;
    [SerializeField] private bool _playOnStart = false;

    private void Start()
    {
        if (_playOnStart) PlayAudio();
    }

    public void PlayAudio()
    {
        _source.pitch = _pitch;
        if (_loop)
        {
            _source.loop = true;
            _source.clip = _clip;
            _source.volume = _clipVolume;
            _source.Play();
        }
        else
        {
            _source.PlayOneShot(_clip, _clipVolume);
        }
    }

    public void StopAudio()
    {
        _source.Stop();
    }
}
