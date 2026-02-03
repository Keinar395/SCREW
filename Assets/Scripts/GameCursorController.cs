using UnityEngine;

public class GameCursorController : MonoBehaviour
{
    void Start()
    {
        // Oyun başlar başlamaz mouse'u kilitle ve gizle
        LockCursor();
    }

    void Update()
    {
        // Eğer oyuncu yanlışlıkla ESC'ye basıp mouse'u açarsa,
        // oyuna tekrar tıkladığında (Sol Tık) geri kilitlensin.
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Ortaya kilitle
        Cursor.visible = false; // Görünmez yap
    }
}