using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [SerializeField] private GameObject _atwood;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _atwood.SetActive(true);
        }
    }
}
