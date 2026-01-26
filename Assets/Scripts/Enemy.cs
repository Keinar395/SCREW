using UnityEngine;
using System.Collections; // Coroutine kullanmak için gerekli

public class Enemy : MonoBehaviour
{
    [Header("Düşman Ayarları")]
    public float health = 100f;
    public float blinkDuration = 0.1f; // Ne kadar süre kırmızı kalsın?

    private Renderer enemyRenderer;
    private Color originalColor;

    void Start()
    {
        // Düşmanın rengini değiştirebilmek için Renderer bileşenini alıyoruz
        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    // Bu fonksiyonu mermi çağıracak
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        // Vurulma efektini başlat
        StartCoroutine(BlinkEffect());

        // Can 0'a düştü mü kontrol et
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Şimdilik sadece objeyi yok ediyoruz. İleride patlama efekti vs. ekleyebilirsin.
        Destroy(gameObject);
    }

    // Yanıp sönme efekti (Coroutine)
    IEnumerator BlinkEffect()
    {
        enemyRenderer.material.color = Color.red; // Rengi Kırmızı yap
        yield return new WaitForSeconds(blinkDuration); // Biraz bekle
        enemyRenderer.material.color = originalColor; // Eski rengine dön
    }
}