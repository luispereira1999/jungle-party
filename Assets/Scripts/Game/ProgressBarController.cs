using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] ProgressBarCircle _ProgressBar;

    void Start()
    {
        _ProgressBar.BarValue = 0;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.G))
        {
            _ProgressBar.BarValue += 1;
        }

        if (Input.GetKey(KeyCode.F))
        {
            _ProgressBar.BarValue -= 1;
        }
    }
}