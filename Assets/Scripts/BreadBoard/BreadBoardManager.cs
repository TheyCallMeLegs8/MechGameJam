using System.Collections.Generic;
using UnityEngine;

public class BreadBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject _pointHolder;
    [SerializeField] private GameObject _onLight;
    private GameObject _light;

    private List<BoardConnector> _boardConnectors = new List<BoardConnector>();
    private List<BoardStart> _boardStartPoints = new List<BoardStart>();

    private BoardStart _currentStarter;
    private BoardConnector _currentConnector;

    private void OnEnable()
    {
        foreach (BoardStart starter in _pointHolder.GetComponentsInChildren<BoardStart>())
        {
            starter.OnClick.AddListener(OnClickStarter);
            _boardStartPoints.Add(starter);
        }
        foreach(BoardConnector connector in _pointHolder.GetComponentsInChildren<BoardConnector>())
        {
            connector.OnClick.AddListener(OnClickConnector);
            _boardConnectors.Add(connector);
        }
    }

    private void OnDisable()
    {
        foreach (BoardStart starter in _pointHolder.GetComponentsInChildren<BoardStart>())
        {
            starter.OnClick.RemoveListener(OnClickStarter);
            _boardStartPoints.Remove(starter);
        }
        foreach (BoardConnector connector in _pointHolder.GetComponentsInChildren<BoardConnector>())
        {
            connector.OnClick.RemoveListener(OnClickConnector);
            _boardConnectors.Remove(connector);
        }
    }

    private void OnClickStarter(BoardStart starter)
    {
        if(_currentStarter != null && _currentStarter.CurrentConnecters.Count <= 0) _currentStarter.DestroyLight();

        if (starter.CurrentConnecters.Count > 0)
        {
            for (int i = 0; i < starter.CurrentConnecters.Count; i++)
            {
                starter.CurrentConnecters[i].DestroyLight();
            }

            starter.ClearConnectors();
        }

        starter.DestroyLight();

        starter.SpawnLight();
        _currentStarter = starter;
        _currentConnector = starter.GetComponent<BoardConnector>();

    }

    private void OnClickConnector(BoardConnector connector)
    {
        if (connector == _currentConnector)
        {
            connector.DestroyLight();
            return;
        }

        if(connector.IsActive) return;

        if (connector.TryConnect(_currentConnector))
        {
            connector.SpawnLight();
            _currentConnector = connector;
            _currentStarter.AddConnectorToSequence(_currentConnector);
        }
        else
        {
            return;
        }
    }

    private void RemovePoint()
    {

    }

    private void RemoveStarterConnectors(BoardConnector connector)
    {
        _currentStarter.RemoveConnectorFromSequence(connector);
    }
}
