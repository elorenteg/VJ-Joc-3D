using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    public Texture eyesNormalTex;
    public Texture haloCenterNormalTex;
    public Texture haloEndNormalTex;

    public Texture eyesDeadTex;
    public Texture haloCenterDeadTex;
    public Texture haloEndDeadTex;

    // Use this for initialization
    void Start ()
    {
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 15;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        UpdateTextures();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animation>().Play("Move", PlayMode.StopAll);

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
        skinnedMeshRenderer.materials[1].mainTexture = eyesNormalTex;
        skinnedMeshRenderer.materials[3].mainTexture = haloCenterNormalTex;
        skinnedMeshRenderer.materials[2].mainTexture = haloEndNormalTex;

        UpdateCoordinates();
    }

    void UpdateCoordinates()
    {
        // skinnedMeshRenderer.materials: vector de los materiales del modelo
        // 0: cuerpo
        // 1: ojos
        // 2: halo central
        // 3: halo end

        if (state == 0)
        {
            Vector2 offset = new Vector2(0.0f, 0.0f);
            skinnedMeshRenderer.materials[1].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[3].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[2].mainTextureOffset = offset;
        }
        else
        {
            Vector2 offset = new Vector2(0.5f, 0.0f);
            skinnedMeshRenderer.materials[1].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[3].mainTextureOffset = offset;
            skinnedMeshRenderer.materials[2].mainTextureOffset = offset;
        }
    }
}
