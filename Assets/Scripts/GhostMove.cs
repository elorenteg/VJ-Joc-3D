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
    public Texture eyeState1Tex;
    public Texture eyeState2Tex;
    public Texture haloCenterState1Tex;
    public Texture haloCenterState2Tex;
    public Texture haloEndState1Tex;
    public Texture haloEndState2Tex;

    // Use this for initialization
    void Start ()
    {
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 10;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        UpdateTextures();
    }

    // Update is called once per frame
    void Update() {
        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = state + 1;
            if (state == 2) state = 0;
        }

        ++timeState;

        UpdateTextures();
    }

    void UpdateTextures()
    {
        // skinnedMeshRenderer.materials: vector de los materiales del modelo
        // 0: cuerpo
        // 1: ojos
        // 2: halo central
        // 3: halo end
        if (state == 0)
        {
            skinnedMeshRenderer.materials[1].mainTexture = eyeState1Tex;
            skinnedMeshRenderer.materials[3].mainTexture = haloCenterState1Tex;
            skinnedMeshRenderer.materials[2].mainTexture = haloEndState1Tex;
        }
        else
        {
            skinnedMeshRenderer.materials[1].mainTexture = eyeState2Tex;
            skinnedMeshRenderer.materials[3].mainTexture = haloCenterState2Tex;
            skinnedMeshRenderer.materials[2].mainTexture = haloEndState2Tex;
        }
    }
}
