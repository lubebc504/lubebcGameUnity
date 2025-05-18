using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RevolverUI : MonoBehaviour
{
    public Text ammoText;  // UI 텍스트
    public Revolver gun;        // Gun 스크립트 참조

    private void Update()
    {
        // 탄창 상태 업데이트
        if (gun.isReloading)
        {
            ammoText.text = $"탄창: {gun.currentAmmo} / {gun.magazineSize}";
        }
        else
        {
            ammoText.text = $"탄창: {gun.currentAmmo} / {gun.magazineSize}";
        }
    }
}