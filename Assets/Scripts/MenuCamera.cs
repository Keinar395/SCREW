using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform targetPoint; // Kameranın varacağı nokta (CameraTarget)
    public float speed = 2.0f;    // Yaklaşma hızı

    void Update()
    {
        // Kameranın pozisyonunu, hedefe doğru adım adım değiştir
        // Vector3.MoveTowards(Şu anki Konum, Hedef Konum, Hız * Zaman)
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // İstersen kamera hedefe varınca hafif titreme yapabilirsin ama şimdilik sadece dursun.
    }
}