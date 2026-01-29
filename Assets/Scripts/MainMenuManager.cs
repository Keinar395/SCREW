using UnityEngine;
using UnityEngine.SceneManagement; // Sahne değişimi için şart

public class MainMenuManager : MonoBehaviour
{
    // OYUNA BAŞLA
    public void PlayGame()
    {
        // "1" numaralı sahneyi yükle (Birazdan Build Settings'ten ayarlayacağız)
        // Veya sahne adını tırnak içinde yazabilirsin: SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene(1); 
    }

    // OYUNDAN ÇIK
    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkıldı!"); // Editörde çıkış çalışmaz, konsola yazar.
        Application.Quit();
    }
}