using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] private Canvas _interactCanvas;

    void Start()
    {
        _interactCanvas.gameObject.SetActive(false);
    }

    public void InteractButtonAppears()
    {
        _interactCanvas.gameObject.SetActive(true);
    }

    public void InteractButtonDisappears()
    {
        _interactCanvas.gameObject.SetActive(false);
    }
}
