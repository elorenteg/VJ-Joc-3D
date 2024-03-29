﻿using UnityEngine;
using System.Collections;

public class ObjectAnimate : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clipsource;
    public float audioVolume;

    private bool stateAttract = false;
    private Vector3 postPos;

    private float startTime, duration;
    private int frame;
    public float moveSpeed = 5;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetStateAttraction(Vector3 fiPos, float y)
    {
        stateAttract = true;
        postPos = fiPos;
        postPos.y = y;

        duration = (postPos - transform.position).magnitude / moveSpeed;
        startTime = Time.time;
        frame = 0;
    }

    void Update()
    {
        if (stateAttract)
        {
            float time = Time.time - startTime; // time since start
            transform.position = Vector3.Lerp(transform.position, postPos, time / duration);

            frame += 1;

            //Debug.Log(frame);
            if (frame == 4) Destroy(this.gameObject);
        }
    }

    public void PlaySound()
    {
        audioSource.volume = audioVolume;
        audioSource.clip = clipsource;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
