using UnityEngine;

public class DalekAI : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform player; // Karakterini buraya sürükle
    public float moveSpeed = 2.5f;
    public float detectionRange = 20f; // Seni ne kadar uzaktan görecek?
    public float stoppingDistance = 5f; // İçine girmesin diye durma mesafesi
    public float turnSpeed = 5f;

    [Header("Süzülme Efekti")]
    public float hoverAmplitude = 0.5f; // Ne kadar yukarı aşağı oynasın?
    public float hoverFrequency = 1f; // Ne kadar hızlı oynasın?

    private Vector3 startPos;

    void Start()
    {
        // Eğer player atanmadıysa otomatik bul (Tag'inin "Player" olduğundan emin ol)
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        // Mesafeyi ölç
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Eğer menzildeyse KOVALA
        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // Menzilde değilse yerinde süzül (Idle)
            HoverEffect();
        }
    }

    void ChasePlayer(float distance)
    {
        // 1. Oyuncuya Dön (Yüzünü dönmesi için)
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // 2. Oyuncuya Doğru Uç (Eğer dibinde değilse)
        if (distance > stoppingDistance)
        {
            // 3D uzayda direkt üstüne gider (Uçma efekti)
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    void HoverEffect()
    {
        // Beklerken olduğu yerde hafifçe aşağı yukarı süzülsün (Ürkütücü durur)
        float newY = startPos.y + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}