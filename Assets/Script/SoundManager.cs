using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum EBgm
    {
        BGM_GAME,
    }

    public enum ESfx
    {
        SFX_BUTTON,
        SFX_HIT,
        SFX_LEVELUP,
        SFX_DEAD,
        SFX_SHOOT,
        SFX_RELOAD,
        SFX_COIN,
        SFX_CHEST,
    }

    //audio clip 담을 수 있는 배열
    [SerializeField] private AudioClip[] bgms;

    [SerializeField] private AudioClip[] sfxs;

    //플레이하는 AudioSource
    [SerializeField] private AudioSource audioBgm;

    [SerializeField] private AudioSource audioSfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(EBgm bgmIdx)
    {
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }
}