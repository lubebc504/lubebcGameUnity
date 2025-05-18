using UnityEngine;

public class GunAim : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer playerSprite; // 캐릭터 스프라이트 렌더러

    public float rotationSpeed = 15f; // 회전 속도 조절

    private void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // 총구 방향 계산
        Vector2 direction = (mousePosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 부드러운 회전 적용
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Mathf.Clamp(Time.deltaTime * rotationSpeed, 0f, 1f));
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        bool isFlipped = mousePosition.x < player.transform.position.x;
        playerSprite.flipX = isFlipped;

        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(currentScale.x, Mathf.Abs(currentScale.y) * (isFlipped ? -1 : 1), currentScale.z);
    }
}