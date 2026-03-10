using UnityEngine;

[RequireComponent(typeof(Movement))]

public class MechControls : MonoBehaviour
{
    [SerializeField] private MechTerminal _terminal;
    [SerializeField] private Movement _movement;

    [SerializeField] private Transform _torso;
    [SerializeField] private Transform _legs;
    [SerializeField] private float _matchTorsoAndLegsSpeed = 40.0f;

    private void OnValidate()
    {
        if(_movement == null) _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _movement.SetMoveInput(_legs.transform.forward);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            _movement.SetMoveInput(-_legs.transform.forward);
        }
        if (Input.GetKey(KeyCode.J))
        {
            _movement.RotateXPosAtSpeed(_torso, _movement.TurnSpeed);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            _movement.EndRotation();
        }
        if (Input.GetKey(KeyCode.L))
        {
            _movement.RotateXPosAtSpeed(_torso, - _movement.TurnSpeed);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            _movement.EndRotation();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _movement.SetMoveInput(new Vector3(0, 0, 0));
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            OrientLegsToTorso();
        }
    }

    private void OrientLegsToTorso()
    {
        _movement.MatchTransformRotations(_torso, _legs, _matchTorsoAndLegsSpeed);
    }
}
