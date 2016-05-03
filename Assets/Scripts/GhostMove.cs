using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    private float GHOST_Y_POS = 18.5f;

    private bool isDead;

    private int state;
    private int timeState;
    private int MAX_TIME_STATE;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private GhostAnimate animationScript;

    // Use this for initialization
    public void Start ()
    {
        isDead = false;
        state = 0;
        timeState = 0;
        MAX_TIME_STATE = 15;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animationScript = GetComponent<GhostAnimate>();
        //animationScript.SetTextures(animationScript.stateMove(), state);
    }

    // Update is called once per frame
    public void Update()
    {
        if (timeState == MAX_TIME_STATE)
        {
            timeState = 0;
            state = state + 1;
            if (state == 2) state = 0;

            if (!isDead) animationScript.SetTextures(animationScript.stateMove(), state);
            else animationScript.SetTextures(animationScript.stateDead(), state);
        }

        ++timeState;

        Vector3 pos = transform.position;
        pos.y = GHOST_Y_POS;
        transform.position = pos;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX
                                      | RigidbodyConstraints.FreezeRotationY
                                      | RigidbodyConstraints.FreezeRotationZ;
    }

    public void SetDead()
    {
        isDead = true;
        animationScript.SetTextures(animationScript.stateDead(), state);

        // Mover a base
        // isDead = false;
    }
}
