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
            _panels[1].SetBroken(true);
            _boards[1].SetBroken(true);

            _currentPanels.Remove(_panels[1]);
            _currentBoards.Remove(_boards[1]);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _panels[2].SetBroken(true);
            _boards[2].SetBroken(true);

            _currentPanels.Remove(_panels[2]);
            _currentBoards.Remove(_boards[2]);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            _panels[3].SetBroken(true);
            _boards[3].SetBroken(true);

            _currentPanels.Remove(_panels[3]);
            _currentBoards.Remove(_boards[3]);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            _panels[0].SetBroken(true);
            _boards[0].SetBroken(true);

            _currentPanels.Remove(_panels[0]);
            _currentBoards.Remove(_boards[0]);
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
                _boards[i].SetBroken(false);
            }
        }
    }
}
