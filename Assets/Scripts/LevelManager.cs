using UnityEngine;
using UnityEngine.SceneManagement; // Sahne değişimi için
using TMPro; // TextMeshPro için

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // Diğer scriptlerden kolayca ulaşmak için

    [Header("UI Ayarları")]
    public TextMeshProUGUI enemyCountText; // Canvas'taki Text nesnesi
    public string prefixText = "KALAN DÜŞMAN: "; // Yazının başı

    private int totalEnemies;

    void Awake()
    {
        // Singleton yapısı (Her yerden LevelManager.Instance diye ulaşabilmek için)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // 1. Sahnedeki "Enemy" tag'ine sahip tüm objeleri bul ve say
        // DİKKAT: Düşmanların Tag'inin "Enemy" olduğundan emin ol!
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;

        UpdateUI();
    }

    public void EnemyDied()
    {
        totalEnemies--;

        // Sayı eksiye düşmesin (Garanti olsun)
        if (totalEnemies < 0) totalEnemies = 0;

        UpdateUI();

        // Düşman kalmadıysa sonraki seviyeye geç
        if (totalEnemies <= 0)
        {
            LoadNextLevel();
        }
    }

    void UpdateUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = prefixText + totalEnemies.ToString();
        }
    }

    void LoadNextLevel()
    {
        Debug.Log("Tüm düşmanlar öldü! Sonraki seviye yükleniyor...");
        
        // Build Settings'deki sıraya göre bir sonraki sahneyi açar
        // Örn: Level 1 (Index 1) -> Level 2 (Index 2)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}