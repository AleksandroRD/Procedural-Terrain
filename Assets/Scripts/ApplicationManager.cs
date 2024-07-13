using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField] private bool _limitFPS = true;
    [SerializeField] private int maxFPS = 60;

    void Start()
    {
        if (!_limitFPS) { return; }
        Application.targetFrameRate = maxFPS;
    }
}
