using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Bu scripti ekleyince otomatik Rigidbody de ekler
public class RunningEnemyAI : MonoBehaviour
{
    [Header("Hedef ve Hareket")]
    public Transform player;
    public float moveSpeed = 4.0f;
    public float detectionRange = 15f; 
    public float stoppingDistance = 1.5f; 
    public float turnSpeed = 5f;

    [Header("Animasyon")]
    public Animator enemyAnimator; 

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody bileşenini alıyoruz
        rb = GetComponent<Rigidbody>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
        if (enemyAnimator == null)
            enemyAnimator = GetComponent<Animator>();
    }

    // Fizik işlemleri her zaman FixedUpdate içinde yapılmalıdır!
    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        
        // Menzildeyse ve çok yakında değilse
        if (distance < detectionRange && distance > stoppingDistance)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }
    }

    void ChasePlayer()
    {
        // 1. YÜZÜNÜ DÖNME (Sadece Y ekseninde - Sağa sola dönmesi için)
        // Oyuncunun yerini al ama yüksekliğini (Y) yok say, kendi yüksekliğimizi koruyalım.
        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 direction = (lookPos - transform.position).normalized;
        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Rotasyonu Rigidbody üzerinden yumuşakça yapıyoruz
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime));
        }

        // 2. FİZİKSEL HAREKET (Yokuş Çıkma & İnme)
        // Mevcut dikey hızımızı (Gravity) koruyarak, sadece yatayda hız veriyoruz.
        Vector3 targetVelocity = transform.forward * moveSpeed;
        
        // Zombinin düşme/zıplama hızını (rb.linearVelocity.y) koru, yoksa havada asılı kalır.
        // NOT: Unity 6 öncesi sürüm kullanıyorsan 'rb.linearVelocity' yerine 'rb.velocity' yazmalısın.
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        // 3. ANİMASYON
        if (enemyAnimator != null)
        {
            enemyAnimator.SetFloat("Speed", 1f); 
        }
    }

    void StopChasing()
    {
        // Durduğumuzda kaymayı önlemek için X ve Z hızını sıfırla ama Y (Gravity) kalsın.
        // NOT: Unity 6 öncesi sürüm kullanıyorsan 'rb.linearVelocity' yerine 'rb.velocity' yazmalısın.
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

        if (enemyAnimator != null)
        {
            enemyAnimator.SetFloat("Speed", 0f);
        }
    }
}