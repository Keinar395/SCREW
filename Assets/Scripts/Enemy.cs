using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Düşman Ayarları")]
    public float health = 100f;
    public float blinkDuration = 0.1f;

    [Header("Efektler")]
    public GameObject explosionParticle; // Patlama Prefab'ı (Asset Store'dan bulabilirsin)
    public float explosionForce = 500f;  // Parçaların fırlama gücü
    public float explosionRadius = 5f;

    // Tek bir renderer yerine, bir renderer DİZİSİ (Listesi) tutuyoruz
    private Renderer[] allRenderers;
    // Her parçanın orijinal rengini saklamak için de bir renk dizisi tutuyoruz
    private Color[] originalColors;

    void Start()
    {
        // 1. Bu objenin altındaki TÜM (kendisi + çocukları) Renderer bileşenlerini bul
        allRenderers = GetComponentsInChildren<Renderer>();

        // 2. Orijinal renkleri saklayacak diziyi oluştur (Renderer sayısı kadar yer aç)
        originalColors = new Color[allRenderers.Length];

        // 3. Döngü ile her parçanın kendi rengini hafızaya al
        for (int i = 0; i < allRenderers.Length; i++)
        {
            // Eğer parçanın bir materyali varsa rengini kaydet
            if (allRenderers[i].material.HasProperty("_Color") || allRenderers[i].material.HasProperty("_BaseColor"))
            {
                originalColors[i] = allRenderers[i].material.color;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        // Eğer canı varsa yanıp sön, yoksa öl
        if (health > 0)
        {
            // Zaten yanıp sönüyorsa durdurup baştan başlat (Overlap olmasın)
            StopCoroutine(nameof(BlinkEffect)); 
            StartCoroutine(nameof(BlinkEffect));
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        // 1. Particle Efektini Yarat (Varsa)
        if (explosionParticle != null)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }

        // 2. Yapay Zekayı Kapat (Artık kovalayamaz)
        if (GetComponent<DalekAI>() != null) 
            GetComponent<DalekAI>().enabled = false;

        // 3. Collider'ı kapat (Mermiler artık içinden geçsin, cesede takılmasın)
        GetComponent<Collider>().enabled = false;

        // 4. PARÇALANMA MANTIĞI
        // Bu objenin altındaki tüm parçaları bul
        Transform[] allParts = GetComponentsInChildren<Transform>();

        foreach (Transform part in allParts)
        {
            if (part == transform) continue; // Ana objeyi atla

            // Her parçaya Rigidbody ekle (Fizik kazandır)
            Rigidbody rb = part.gameObject.GetComponent<Rigidbody>();
            if (rb == null) rb = part.gameObject.AddComponent<Rigidbody>();

            // Parçayı ana objeden ayır (Bağımsız olsunlar)
            part.SetParent(null); 

            // PATLAMA KUVVETİ UYGULA
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            
            // 5 saniye sonra parçaları yok et (Sahne çöplüğe dönmesin)
            Destroy(part.gameObject, 5f);
        }

        // Ana boş objeyi yok et
        Destroy(gameObject);
    }

    IEnumerator BlinkEffect()
    {
        // 1. Tüm parçaları KIRMIZI yap
        foreach (Renderer r in allRenderers)
        {
            r.material.color = Color.red;
        }

        yield return new WaitForSeconds(blinkDuration);

        // 2. Tüm parçaları kendi ESKİ renklerine döndür
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].material.color = originalColors[i];
        }
    }
}