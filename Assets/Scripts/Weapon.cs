using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject bulletPrefab;     // Mermi Prefab'ı
    public Transform bulletSpawn;       // Merminin çıkacağı namlu ucu
    public float bulletVelocity = 30f;  // Mermi hızı
    public float bulletPrefabLifeTime = 1f; // Merminin yok olma süresi

    [Header("Animasyon")]
    public Animator gunAnimator; 

    [Header("Geri Tepme")]
    public Recoil recoilScript; 

    [Header("Efektler")]
    // Burayı değiştirdim: Namlu ucu ateşi (Muzzle Flash) ile Patlama efekti karışmasın.
    public GameObject muzzleFlash; // Buraya namlu ateşi efektini sürükle (Legacy Particle'dan küçük bir efekt seç)

    public void Fire()
    {
        // 1. NAMLU EFEKTİNİ OLUŞTUR (DÜZELTİLEN KISIM)
        if (muzzleFlash != null)
        {
            // bulletSpawn yerine bulletSpawn.position yazdık (Konum)
            // Quaternion.identity yerine bulletSpawn.rotation yazdık (Yön)
            GameObject flash = Instantiate(muzzleFlash, bulletSpawn.position, bulletSpawn.rotation);
            
            // Efekti namlunun içine çocuk (child) yapalım ki silah geri teperken efekt havada asılı kalmasın, silahla gitsin.
            flash.transform.parent = bulletSpawn; 
            
            // Efekt sonsuza kadar sahnede kalmasın, 0.5 saniye sonra yok et
            Destroy(flash, 0.5f); 
        }

        // 2. Mermiyi oluştur
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // 3. Animasyonu Tetikle
        gunAnimator.SetTrigger("Shoot");

        // 4. Geri Tepme
        if (recoilScript != null)
        {
            recoilScript.RecoilFire();
        }

        // 5. Mermiye hız ver
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bulletSpawn.forward * bulletVelocity; 
        }

        // 6. Mermiyi yok et
        Destroy(bullet, bulletPrefabLifeTime);
    }
}