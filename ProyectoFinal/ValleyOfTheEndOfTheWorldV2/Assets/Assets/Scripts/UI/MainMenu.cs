using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // =====================================
    // JUGAR
    // =====================================

    public void PlayGame()
    {
        SceneManager.LoadScene(
            "GameScene");
    }

    // =====================================
    // TUTORIAL
    // =====================================

    public void OpenTutorial()
    {
        SceneManager.LoadScene(
            "TutorialScene");
    }

    // =====================================
    // SALIR
    // =====================================

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");

        Application.Quit();
    }
}