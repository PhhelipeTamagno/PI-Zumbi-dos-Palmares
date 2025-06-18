using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Luz2D : MonoBehaviour
{
    private Light2D fireLight;

    [Header("Intensity Flicker")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;

    [Header("Radius Flicker")]
    public float minRadius = 3.5f;
    public float maxRadius = 4.5f;

    [Header("Flicker Speed")]
    public float flickerSpeed = 0.1f;

    private void Awake()
    {
        fireLight = GetComponent<Light2D>();
    }

    private void Start()
    {
        InvokeRepeating("Flicker", 0f, flickerSpeed);
    }

    void Flicker()
    {
        fireLight.intensity = Random.Range(minIntensity, maxIntensity);
        fireLight.pointLightOuterRadius = Random.Range(minRadius, maxRadius);
    }
}
