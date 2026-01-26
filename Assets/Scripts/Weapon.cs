using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject bulletPrefab;     // Mermi Prefab'ı
    public Transform bulletSpawn;       // Merminin çıkacağı namlu ucu
    public float bulletVelocity = 30f;  // Mermi hızı
    public float bulletPrefabLifeTime = 3f; // Merminin yok olma süresi

    // InputManager'dan çağıracağımız ateş etme fonksiyonu
    public void Fire()
    {
        // 1. Mermiyi namlu ucunda (bulletSpawn) oluştur
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // 2. Mermiye hız ver (Rigidbody bileşeni olduğunu varsayıyoruz)
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Namlunun baktığı yöne (forward) doğru fırlat
            rb.linearVelocity = bulletSpawn.forward * bulletVelocity; 
            // NOT: Unity 6 öncesi bir sürüm kullanıyorsan 'rb.linearVelocity' yerine 'rb.velocity' yazmalısın.
        }

        // 3. Belirli bir süre sonra mermiyi yok et (Performans için)
        Destroy(bullet, bulletPrefabLifeTime);
    }
}