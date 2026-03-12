using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
