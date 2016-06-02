using UnityEngine;
using System.Collections;

public class WallAnimate : MonoBehaviour
{

    private bool animTop;
    private bool animFront;
    private bool animBack;
    private bool animLeftRight;

    private int state;

    // Use this for initialization
    void Start()
    {

    }

    public void SetTextures(Texture top, Texture front, Texture back, Texture leftRight)
    {
        transform.FindChild("Top").gameObject.GetComponent<Renderer>().material.mainTexture = top;
        transform.FindChild("Front").gameObject.GetComponent<Renderer>().material.mainTexture = front;
        transform.FindChild("Back").gameObject.GetComponent<Renderer>().material.mainTexture = back;
        transform.FindChild("Left").gameObject.GetComponent<Renderer>().material.mainTexture = leftRight;
        transform.FindChild("Right").gameObject.GetComponent<Renderer>().material.mainTexture = leftRight;
    }

    public void SetAnimatedWalls(bool top, bool front, bool back)
    {
        state = 0;

        animTop = top;
        animFront = front;
        animBack = back;

        if (animTop) transform.FindChild("Top").gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.5f, 1.0f);
        if (animFront) transform.FindChild("Front").gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.5f, 1.0f);
        if (animBack) transform.FindChild("Back").gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.5f, 1.0f);
    }

    public void AnimateTexture()
    {
        float xOffset = 0.0f;
        if (state == 1) xOffset = 0.5f;
        Vector2 offset = new Vector2(xOffset, 1.0f);

        if (animTop)
        {
            GameObject face = transform.FindChild("Top").gameObject;
            face.GetComponent<Renderer>().material.mainTextureOffset = offset;
        }

        if (animFront)
        {
            GameObject face = transform.FindChild("Front").gameObject;
            face.GetComponent<Renderer>().material.mainTextureOffset = offset;
        }

        if (animBack)
        {
            GameObject face = transform.FindChild("Back").gameObject;
            face.GetComponent<Renderer>().material.mainTextureOffset = offset;
        }

        state++;
        if (state == 2) state = 0;
    }
}
