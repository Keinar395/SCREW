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
        // Şu anki sahne numarasını al (Örn: 2)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Bir sonraki sahne numarasını hesapla (Örn: 3)
        int nextSceneIndex = currentSceneIndex + 1;

        // Kontrol Et: Listede böyle bir sahne var mı?
        // SceneManager.sceneCountInBuildSettings = Toplam sahne sayısı
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Varsa sıradaki level'i yükle
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Yoksa (yani oyun bittiyse), Ana Menüye (Index 0) dön
            Debug.Log("Oyun Bitti! Ana Menüye dönülüyor...");
            SceneManager.LoadScene(0);

            // --- KRİTİK KISIM: İMLECİ SERBEST BIRAK ---
            // Bunu yapmazsan menüde mouse hareket etmez!
            Cursor.lockState = CursorLockMode.None; // Kilidi aç
            Cursor.visible = true; // Görünür yap
        }
    }
}