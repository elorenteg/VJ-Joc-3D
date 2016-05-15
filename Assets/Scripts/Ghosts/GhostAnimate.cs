using UnityEngine;
using System.Collections;

public class GhostAnimate : MonoBehaviour
{
    private const int MOVE = 0;
    private const int KILLEABLE = 1;
    private const int DIE = 2;

    private AudioSource audioSource;
    public AudioClip moveSound;

    private Animation ghostAnimation;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private const int BODY = 0;
    private const int EYES = 1;
    private const int HALO_CEN = 3;
    private const int HALO_END = 2;

    private const int TRANSPARENT = 0;
    private const int CUTOUT = 1;

    public Texture bodyNormalTex;
    public Texture eyesNormalTex;
    public Texture haloCenterNormalTex;
    public Texture haloEndNormalTex;

    public Texture bodyKilleableTex;
    public Texture eyesKilleableTex;
    public Texture haloCenterKilleableTex;
    public Texture haloEndKilleableTex;

    public Texture bodyDeadTex;
    public Texture eyesDeadTex;
    public Texture haloCenterDeadTex;
    public Texture haloEndDeadTex;

    private const int LEFT = 0;
    private const int RIGHT = 1;

    // Use this for initialization
    public void Start()
    {
        ghostAnimation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        SetTextures(MOVE, LEFT);
    }

    public int stateMove()
    {
        return MOVE;
    }

    public int stateKilleable()
    {
        return KILLEABLE;
    }

    public int stateDead()
    {
        return DIE;
    }

    public void SetBodyTexture(Texture tex)
    {
        bodyNormalTex = tex;
    }

    public void SetTextures(int alive, int state)
    {
        switch (alive)
        {
            case MOVE:
                SetupMaterialWithBlendMode(skinnedMeshRenderer.materials[BODY], CUTOUT);
                skinnedMeshRenderer.materials[BODY].mainTexture = bodyNormalTex;
                skinnedMeshRenderer.materials[EYES].mainTexture = eyesNormalTex;
                skinnedMeshRenderer.materials[HALO_CEN].mainTexture = haloCenterNormalTex;
                skinnedMeshRenderer.materials[HALO_END].mainTexture = haloEndNormalTex;

                skinnedMeshRenderer.materials[BODY].mainTextureScale = new Vector2(1.0f, 1.0f);
                skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.0f, 0.0f);
                break;
            case KILLEABLE:
                SetupMaterialWithBlendMode(skinnedMeshRenderer.materials[BODY], CUTOUT);
                skinnedMeshRenderer.materials[BODY].mainTexture = bodyKilleableTex;
                skinnedMeshRenderer.materials[EYES].mainTexture = eyesKilleableTex;
                skinnedMeshRenderer.materials[HALO_CEN].mainTexture = haloCenterKilleableTex;
                skinnedMeshRenderer.materials[HALO_END].mainTexture = haloEndKilleableTex;

                skinnedMeshRenderer.materials[BODY].mainTextureScale = new Vector2(0.5f, 1.0f);
                if (state == 0)
                    skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.0f, 0.0f);
                else
                    skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.5f, 0.0f);
                break;
            case DIE:
                SetupMaterialWithBlendMode(skinnedMeshRenderer.materials[BODY], TRANSPARENT);
                skinnedMeshRenderer.materials[BODY].mainTexture = bodyDeadTex;
                skinnedMeshRenderer.materials[EYES].mainTexture = eyesDeadTex;
                skinnedMeshRenderer.materials[HALO_CEN].mainTexture = haloCenterDeadTex;
                skinnedMeshRenderer.materials[HALO_END].mainTexture = haloEndDeadTex;

                skinnedMeshRenderer.materials[BODY].mainTextureScale = new Vector2(0.5f, 1.0f);
                if (state == 0)
                    skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.0f, 0.0f);
                else
                    skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.5f, 0.0f);
                break;
        }

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
        skinnedMeshRenderer.materials[EYES].mainTextureOffset = offset;
        skinnedMeshRenderer.materials[HALO_CEN].mainTextureOffset = offset;
        skinnedMeshRenderer.materials[HALO_END].mainTextureOffset = offset;
    }

    public void Animate(int anim)
    {
        switch (anim)
        {
            case MOVE:
                ghostAnimation.Play("Move", PlayMode.StopAll);
                break;
            case KILLEABLE:
                break;
            case DIE:
                ghostAnimation.Play("Die", PlayMode.StopAll);
                break;
        }
    }

    public void PlaySound(int sound)
    {
        switch (sound)
        {
            case MOVE:
                break;
            case KILLEABLE:
                break;
            case DIE:
                break;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.volume = 0.5f;
            audioSource.clip = moveSound;
            audioSource.Play();
        }
    }

    public static void SetupMaterialWithBlendMode(Material material, int blendMode)
    {
        switch (blendMode)
        {
            case CUTOUT:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case TRANSPARENT:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
