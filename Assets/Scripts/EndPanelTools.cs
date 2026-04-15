using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelTools : MonoBehaviour
{
    public void PlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
