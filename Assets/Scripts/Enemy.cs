using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Düşman Ayarları")]
    public float health = 100f;
    public float blinkDuration = 0.1f;
    
    [Header("Blink (Parlama) Ayarı")]
    public Material flashMaterial; // OLUŞTURDUĞUN KIRMIZI MATERYALİ BURAYA SÜRÜKLE

    [Header("Efektler")]
    public GameObject explosionParticle; 
    public float explosionForce = 500f; 
    public float explosionRadius = 5f;

    // Rendererları ve ORİJİNAL MATERYALLERİ saklayacağız
    private Renderer[] allRenderers;
    private Material[] originalMaterials;

    void Start()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
        
        // Her renderer'ın orijinal materyalini kaydetmemiz lazım
        originalMaterials = new Material[allRenderers.Length];

        for (int i = 0; i < allRenderers.Length; i++)
        {
            originalMaterials[i] = allRenderers[i].material;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health > 0)
        {
            // Coroutine çakışmasını önle
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
        if (explosionParticle != null)
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }

        // RunningEnemyAI veya DalekAI hangisi varsa kapat
        if (GetComponent<DalekAI>() != null) GetComponent<DalekAI>().enabled = false;
        if (GetComponent<RunningEnemyAI>() != null) GetComponent<RunningEnemyAI>().enabled = false;

        GetComponent<Collider>().enabled = false;

        Transform[] allParts = GetComponentsInChildren<Transform>();

        foreach (Transform part in allParts)
        {
            if (part == transform) continue; 

            Rigidbody rb = part.gameObject.GetComponent<Rigidbody>();
            if (rb == null) rb = part.gameObject.AddComponent<Rigidbody>();

            part.SetParent(null); 
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            Destroy(part.gameObject, 5f);
        }

        Destroy(gameObject);
    }

    IEnumerator BlinkEffect()
    {
        // 1. Eğer flash materyali atadıysan hepsini o materyalle değiştir
        if (flashMaterial != null)
        {
            for (int i = 0; i < allRenderers.Length; i++)
            {
                allRenderers[i].material = flashMaterial;
            }
        }
        else 
        {
            // Materyal atamayı unuttuysan eski usul kızart (Yedek plan)
            foreach (Renderer r in allRenderers) r.material.color = Color.red;
        }

        yield return new WaitForSeconds(blinkDuration);

        // 2. Hepsine kendi orijinal kıyafetini (Texture'lı materyalini) geri giydir
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].material = originalMaterials[i];
        }
    }
}