using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    public int varDead;

    private bool isDead;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

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

    public Texture bodyDeadTex;
    public Texture eyesDeadTex;
    public Texture haloCenterDeadTex;
    public Texture haloEndDeadTex;

    // Use this for initialization
    void Start ()
    {
        isDead = varDead == 0;
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 15;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        UpdateTextures();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animation>().Play("Die", PlayMode.StopAll);

        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = state + 1;
            if (state == 2) state = 0;
        }

        ++timeState;

        UpdateCoordinates();
    }

    void UpdateTextures()
    {
        if (!isDead)
        {
            SetupMaterialWithBlendMode(skinnedMeshRenderer.materials[BODY], CUTOUT);
            skinnedMeshRenderer.materials[BODY].mainTexture = bodyNormalTex;
            skinnedMeshRenderer.materials[EYES].mainTexture = eyesNormalTex;
            skinnedMeshRenderer.materials[HALO_CEN].mainTexture = haloCenterNormalTex;
            skinnedMeshRenderer.materials[HALO_END].mainTexture = haloEndNormalTex;
        }
        else
        {
            SetupMaterialWithBlendMode(skinnedMeshRenderer.materials[BODY], TRANSPARENT);
            skinnedMeshRenderer.materials[BODY].mainTexture = bodyDeadTex;
            skinnedMeshRenderer.materials[EYES].mainTexture = eyesDeadTex;
            skinnedMeshRenderer.materials[HALO_CEN].mainTexture = haloCenterDeadTex;
            skinnedMeshRenderer.materials[HALO_END].mainTexture = haloEndDeadTex;
        }

        UpdateCoordinates();
    }

    void UpdateCoordinates()
    {
        if (!isDead)
        {
            skinnedMeshRenderer.materials[BODY].mainTextureScale = new Vector2(1.0f, 1.0f);
            skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.0f, 0.0f);
        }
        else
        {
            skinnedMeshRenderer.materials[BODY].mainTextureScale = new Vector2(0.5f, 1.0f);
            if (state == 0)
                skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.0f, 0.0f);
            else
                skinnedMeshRenderer.materials[BODY].mainTextureOffset = new Vector2(0.5f, 0.0f);
        }

        if (state == 0)
        {
            Vector2 offset = new Vector2(0.0f, 0.0f);
            skinnedMeshRenderer.materials[EYES].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[HALO_CEN].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[HALO_END].mainTextureOffset = offset;
        }
        else
        {
            Vector2 offset = new Vector2(0.5f, 0.0f);
            skinnedMeshRenderer.materials[EYES].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[HALO_CEN].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[HALO_END].mainTextureOffset = offset;
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
