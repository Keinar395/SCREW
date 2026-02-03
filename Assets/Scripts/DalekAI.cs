using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Bu script ses kaynağına ihtiyaç duyar
public class DalekAI : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform player;
    public float moveSpeed = 2.5f;
    public float detectionRange = 20f;
    public float stoppingDistance = 5f;
    public float turnSpeed = 5f;

    [Header("Süzülme Efekti")]
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 1f;

    [Header("Ses Efektleri")]
    public AudioClip detectionSound; // "EXTERMINATE!" sesi buraya
    private AudioSource audioSource;
    private bool hasSpotted = false; // Sesin bir kere çalması için kontrol

    private Vector3 startPos;

    void Start()
    {
        // Mevcut AudioSource'u al (Enemy scripti için eklediğin ile aynı olabilir)
        audioSource = GetComponent<AudioSource>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // --- MANTIK DEĞİŞİKLİĞİ ---
        
        if (distanceToPlayer < detectionRange)
        {
            // Eğer daha önce tespit etmediysek (İlk görüş anı)
            if (!hasSpotted)
            {
                PlayDetectionSound();
                hasSpotted = true; // Artık gördük, tekrar çalma
            }

            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // Menzilden çıkınca durumu sıfırla
            // Böylece oyuncu uzaklaşıp tekrar gelirse tekrar ses çalar
            hasSpotted = false; 
            
            HoverEffect();
        }
    }

    void PlayDetectionSound()
    {
        if (audioSource != null && detectionSound != null)
        {
            // Sesleri üst üste bindirmez, diğer ses varsa kesip bunu çalar
            // İstersen PlayOneShot da kullanabilirsin
            audioSource.PlayOneShot(detectionSound);
        }
    }

    void ChasePlayer(float distance)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        if (distance > stoppingDistance)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    void HoverEffect()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}