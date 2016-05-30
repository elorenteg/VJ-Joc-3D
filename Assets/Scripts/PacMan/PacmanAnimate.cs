using UnityEngine;
using System.Collections;

public class PacmanAnimate : MonoBehaviour
{
    public const int MOVE = 0;
    public const int DIE = 1;

    private AudioSource audioSource;
    public AudioClip moveSound;

    private Animation pacmanAnimation;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private const int BODY = 1;
    private const int EYES = 0;

    public Texture bodyNormalTex;
    public Texture eyesNormalTex;

    private const int LEFT = 0;
    private const int RIGHT = 1;

    // Use this for initialization
    public void Start()
    {
        pacmanAnimation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public int stateMove()
    {
        return MOVE;
    }

    public int stateDead()
    {
        return DIE;
    }

    public void SetTextures(int state)
    {
        Vector2 offset = new Vector2(0.0f, 0.0f);
        switch (state)
        {
            case LEFT:
                //offset = new Vector2(0.0f, 0.0f);
                break;
            case RIGHT:
                offset = new Vector2(0.5f, 0.0f);
                break;
        }

        skinnedMeshRenderer.materials[BODY].mainTexture = bodyNormalTex;
        skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.0f, 0.0f);

        skinnedMeshRenderer.materials[EYES].mainTexture = eyesNormalTex;
        skinnedMeshRenderer.materials[EYES].mainTextureOffset = offset;
    }

    public void Animate(int anim) {
        switch (anim)
        {
            case MOVE:
                pacmanAnimation.Play("Move", PlayMode.StopAll);
                break;
            case DIE:
                pacmanAnimation.Play("Die", PlayMode.StopAll);
                break;
        }
    }

    public void PlaySound(int sound) {
        switch(sound)
        {
            case MOVE:
                break;
            case DIE:
                break;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.volume = 0.1f;
            audioSource.clip = moveSound;
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
