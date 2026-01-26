using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f; // Merminin vereceği hasar

    // Mermi bir şeye çarptığında (Trigger açık olduğu için OnTriggerEnter kullanıyoruz)
    private void OnTriggerEnter(Collider other)
    {
        // Çarptığımız objede "Enemy" scripti var mı diye kontrol et
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            // Eğer çarptığımız şey düşmansa hasar ver
            enemy.TakeDamage(damage);
        }

        // Düşmana da çarpsa, duvara da çarpsa mermi yok olmalı
        Destroy(gameObject);
    }
}