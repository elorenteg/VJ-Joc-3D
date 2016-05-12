using UnityEngine;

public class CameraFog : MonoBehaviour
{
    public bool Enabled;
    public float StartDistance;
    public float EndDistance;
    public FogMode Mode;
    public float Density;
    public Color Color;

    private float _startDistance;
    private float _endDistance;
    private FogMode _mode;
    private float _density;
    private Color _color;
    private bool _enabled;

    private void OnPreRender()
    {
        this._startDistance = RenderSettings.fogStartDistance;
        this._endDistance = RenderSettings.fogEndDistance;
        this._mode = RenderSettings.fogMode;
        this._density = RenderSettings.fogDensity;
        this._color = RenderSettings.fogColor;
        this._enabled = RenderSettings.fog;

        RenderSettings.fog = this.Enabled;
        RenderSettings.fogStartDistance = this.StartDistance;
        RenderSettings.fogEndDistance = this.EndDistance;
        RenderSettings.fogMode = this.Mode;
        RenderSettings.fogDensity = this.Density;
        RenderSettings.fogColor = this.Color;
    }

    private void OnPostRender()
    {
        RenderSettings.fog = this._enabled;
        RenderSettings.fogStartDistance = this._startDistance;
        RenderSettings.fogEndDistance = this._endDistance;
        RenderSettings.fogMode = this._mode;
        RenderSettings.fogDensity = this._density;
        RenderSettings.fogColor = this._color;
    }
}

