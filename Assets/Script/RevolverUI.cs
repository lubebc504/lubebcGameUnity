using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RevolverUI : MonoBehaviour
{
    public Text ammoText;  // UI �ؽ�Ʈ
    public Revolver gun;        // Gun ��ũ��Ʈ ����

    private void Update()
    {
        // źâ ���� ������Ʈ
        if (gun.isReloading)
        {
            ammoText.text = $"źâ: {gun.currentAmmo} / {gun.magazineSize}";
        }
        else
        {
            ammoText.text = $"źâ: {gun.currentAmmo} / {gun.magazineSize}";
        }
    }
}