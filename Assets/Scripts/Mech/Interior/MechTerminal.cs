using System.Collections.Generic;
using UnityEngine;

public class MechTerminal : MonoBehaviour
{
    [SerializeField] private List<Panel> _panels = new List<Panel>();
    [SerializeField] private List<BoardStart> _boards = new List<BoardStart>();

    // Serialized only for debugging
    [SerializeField] private List<Panel> _currentPanels = new List<Panel>();
    [SerializeField] private List<BoardStart> _currentBoards = new List<BoardStart>();

    private void OnEnable()
    {
        // fill current systems
        for (int i = 0; i < _panels.Count; i++)
        {
            _currentPanels.Add(_panels[i]);
            _panels[i].OnFix.AddListener(FixPanel);
        }

        for (int i = 0; i < _boards.Count; i++)
        {
            _currentBoards.Add(_boards[i]);
        }
    }

    private void OnDisable()
    {
        // empty current systems
        for (int i = 0; i < _panels.Count; i++)
        {
            _currentPanels.Remove(_panels[i]);
            _panels[i].OnFix.RemoveListener(FixPanel);
        }

        for (int i = 0; i < _boards.Count; i++)
        {
            _currentBoards.Remove(_boards[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _currentPanels[2].SetBroken(true);
            _currentBoards[2].SetBroken(true);

            _currentPanels.Remove(_currentPanels[2]);
            _currentBoards.Remove(_currentBoards[2]);
        }
    }

    // currently just make it brake a random major system
    public void BreakRandomSystem()
    {
        int indexToBreak = Random.Range(0, _currentPanels.Count);

        _currentPanels[indexToBreak].SetBroken(true);
        _currentBoards[indexToBreak].SetBroken(true);

        _currentPanels.Remove(_currentPanels[indexToBreak]);
        _currentBoards.Remove(_currentBoards[indexToBreak]);
    }

    private void FixPanel(Panel panel)
    {
        _currentPanels.Add(panel);

        for(int i = 0; i < _panels.Count; i++)
        {
            if (_panels[i] == panel)
            {
                _currentBoards.Add(_boards[i]);
                _currentBoards[i].SetBroken(false);
            }
        }
    }
}
