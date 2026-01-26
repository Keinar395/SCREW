using UnityEngine;

public class Recoil : MonoBehaviour
{
    // Silahın başlangıç konumlarını saklayacağımız değişkenler
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 currentRotation;
    private Vector3 targetRotation;
    
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    [Header("Geri Tepme Ayarları")]
    public float recoilX = -10f; 
    public float recoilY = 2f;  
    public float kickBackZ = 0.2f;

    [Header("Hız Ayarları")]
    public float snappiness = 10f; 
    public float returnSpeed = 5f; 

    void Start()
    {
        // OYUN BAŞLADIĞINDA SİLAH NEREDEYSE ORAYI KAYDET
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Geri tepme hesaplamaları (Yine 0'a dönmeye çalışacaklar)
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.fixedDeltaTime);

        // KRİTİK DÜZELTME: Hesaplanan tepkiyi, ORİJİNAL konumun üzerine ekle
        transform.localPosition = originalPosition + currentPosition;
        transform.localRotation = originalRotation * Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        // Tepkiyi tetikle
        targetPosition -= new Vector3(0, 0, kickBackZ); 
        targetRotation += new Vector3(0, Random.Range(-recoilY, recoilY), recoilX);
    }
}