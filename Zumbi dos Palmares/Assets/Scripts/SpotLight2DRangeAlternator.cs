using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpotLight2DRangeAlternator : MonoBehaviour
{
    public Light2D spotLight2D;
    public float valorA = 1.87f;
    public float valorB = 1.80f;
    public float intervaloTroca = 0.1f; // tempo em segundos entre as trocas

    private float cronometro = 0f;
    private bool usandoValorA = true;

    void Start()
    {
        if (spotLight2D == null)
            spotLight2D = GetComponent<Light2D>();
    }

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= intervaloTroca)
        {
            spotLight2D.pointLightOuterRadius = usandoValorA ? valorA : valorB;
            usandoValorA = !usandoValorA;
            cronometro = 0f;
        }
    }
}
