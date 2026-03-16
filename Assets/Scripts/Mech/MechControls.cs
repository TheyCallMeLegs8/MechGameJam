using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement))]

public class MechControls : MonoBehaviour
{
    [SerializeField] private MechTerminal _terminal;
    [SerializeField] private Movement _movement;
    [SerializeField] private float _overClockLegsSpeed;

    [SerializeField] private Transform _torso;
    [SerializeField] private Transform _legs;
    [SerializeField] private float _matchTorsoAndLegsSpeed = 40.0f;
    private bool _isRotating = false;

    public UnityEvent OnStartMove = new UnityEvent();
    public UnityEvent OnEndMove = new UnityEvent();

    private void OnValidate()
    {
        if(_movement == null) _movement = GetComponent<Movement>();
    }

    public void MoveForward()
    {
        OnStartMove.Invoke();
        _movement.SetMoveInput(_legs.transform.forward);
    }

    public void MoveBackward()
    {
        OnStartMove.Invoke();
        _movement.SetMoveInput(-_legs.transform.forward);
    }

    public void RotateLeft()
    {
        _isRotating = true;
        StartCoroutine(RotationRoutine(_torso, -_movement.TurnSpeed));
    }

    public void RotateRight()
    {
        _isRotating = true;
        StartCoroutine(RotationRoutine(_torso, _movement.TurnSpeed));
    }

    public void EndRotation(int punchDir)
    {
        _isRotating = false;
        _movement.EndRotation(punchDir);
    }

    public void StopMovement()
    {
        OnEndMove.Invoke();
        _movement.SetMoveInput(new Vector3(0, 0, 0));
    }

    public void OrientLegsToTorso()
    {
        _movement.MatchTransformRotations(_torso, _legs, _matchTorsoAndLegsSpeed);
    }

    private IEnumerator RotationRoutine(Transform targetTransform, float speed)
    {
        while (_isRotating)
        {
            _movement.RotateXPosAtSpeed(targetTransform, speed);
            yield return null;
        }
    }

    public void OverClockLegs()
    {
        _movement.SetSpeed(_overClockLegsSpeed);
    }

    public void StopOverclockLegs()
    {
        _movement.SetSpeed(_movement.BaseSpeed);
    }
}
