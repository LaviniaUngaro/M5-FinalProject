using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onGameOver;
    [SerializeField] private UnityEvent _onWin;

    public void GameOver()
    {
        Invoke(nameof(DelayGameOver), 1);
    }

    public void DelayGameOver()
    {
        _onGameOver?.Invoke();
        SoundManager.Instance.OnGameOver();
        SoundManager.Instance.StopBackgroundMusic();
    }

    public void Win()
    {
        Invoke(nameof(DelayWin), 1);
    }

    public void DelayWin()
    {
        _onWin?.Invoke();
        SoundManager.Instance.OnWin();
        SoundManager.Instance.StopBackgroundMusic();
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
