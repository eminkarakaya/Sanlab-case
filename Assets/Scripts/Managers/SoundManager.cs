using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip success,fail,finish;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void SuccessSoundEffect()
    {
        audioSource.PlayOneShot(success);
    }
    public void FailSoundEffect()
    {
        audioSource.PlayOneShot(fail);
    }
    public void FinishSoundEffect()
    {
        audioSource.PlayOneShot(finish);
    }
}
