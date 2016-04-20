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
    private int BODY = 0;
    private int EYES = 1;
    private int HALO_CEN = 3;
    private int HALO_END = 2;

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
        skinnedMeshRenderer.materials[EYES].mainTexture = eyesNormalTex;
        skinnedMeshRenderer.materials[HALO_CEN].mainTexture = haloCenterNormalTex;
        skinnedMeshRenderer.materials[HALO_END].mainTexture = haloEndNormalTex;

        UpdateCoordinates();
    }

    void UpdateCoordinates()
    {
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
}
