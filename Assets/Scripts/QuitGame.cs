using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private bool _setCursorVisible = true;

    private void Start()
    {
        if(!_setCursorVisible) return;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
