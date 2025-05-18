using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Revolver : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform firePoint; // �Ѿ��� ������ ��ġ
    public float fireRate = 0.2f; // ���� �ӵ�
    private float nextFireTime = 0f;

    [Header("Ammo Settings")]
    public int magazineSize = 6; // �� źâ�� �Ѿ� ����
    public int currentAmmo;       // ���� �����ִ� �Ѿ� ��
    public float reloadTime = 1.5f; // ������ �ð�
    public bool isReloading = false;
    private Coroutine reloadCoroutine;
    public Text ammoText;



    [Header("Card Effect")]
    public CardEffect activeCardEffect; // ���� Ȱ��ȭ�� ī�� ȿ��

    private void Start()
    {
        currentAmmo = magazineSize;
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (!isReloading)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }

        if (currentAmmo <= 0 && !isReloading)
        {
            reloadCoroutine = StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading && currentAmmo < magazineSize)
            {
                reloadCoroutine = StartCoroutine(Reload());
            }
        }
    }

    private void Fire()
    {
        if (currentAmmo > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector2 direction = (mousePosition - firePoint.position).normalized;
            float minDistance = 0.3f;

            if (Vector2.Distance(mousePosition, firePoint.position) < minDistance)
            {
                direction = transform.right;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            bulletScript.SetDirection(direction, bulletScript.damageModel);

            // ī�� ȿ�� ����
            if (activeCardEffect != null)
            {
                bulletScript.cardEffect = activeCardEffect;
                activeCardEffect.OnFire(bulletScript);
            }

            currentAmmo--;
            UpdateUI();
        }
        else
        {
            Debug.Log("źâ�� ������ϴ�! �������ϼ���!");
        }
    }

    private IEnumerator Reload()
    {
        if (currentAmmo == magazineSize) yield break;

        isReloading = true;
        Debug.Log("������ ��...");

        if (currentAmmo == 0)
        {
            yield return new WaitForSeconds(0.4f);
        }

        float interval = reloadTime / magazineSize;
        for (int i = currentAmmo; i < magazineSize; i++)
        {
            yield return new WaitForSeconds(interval);
            currentAmmo++;
            UpdateUI();
        }

        isReloading = false;
        Debug.Log("������ �Ϸ�!");
    }

    private void UpdateUI()
    {
        if (ammoText != null)
        {
            ammoText.text = isReloading ? "������ ��..." : $"źâ: {currentAmmo} / {magazineSize}";
        }
    }
}
