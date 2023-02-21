using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public AudioClip heartBeatSound;
    AudioSource audioSource;

    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (GameManager.Instance.isEnemyNear)
        {

            audioSource.clip = heartBeatSound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
