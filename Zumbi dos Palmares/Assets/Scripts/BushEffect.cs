using UnityEngine;

public class BushEffect : MonoBehaviour
{
    [Header("Som")]
    public AudioClip enterBushSound; // som ao entrar na moita
    public float volume = 1f;

    [Header("Efeito Visual")]
    public GameObject bushEffect; // referencia para o objeto de animação DENTRO da moita

    private bool playerInside = false;

    private void Start()
    {
        if (bushEffect != null)
            bushEffect.SetActive(false); // garante que começa desativado
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerInside)
        {
            playerInside = true;

            // --- TOCAR SOM ---
            if (enterBushSound != null)
                AudioSource.PlayClipAtPoint(enterBushSound, transform.position, volume);

            // --- ATIVAR ANIMAÇÃO ---
            if (bushEffect != null)
            {
                bushEffect.SetActive(true);

                float duration = GetEffectDuration(bushEffect);
                if (duration > 0)
                    StartCoroutine(DisableAfterTime(duration));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    private float GetEffectDuration(GameObject effect)
    {
        Animator anim = effect.GetComponent<Animator>();
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
                return clips[0].length;
        }
        return 0.5f;
    }

    private System.Collections.IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (bushEffect != null)
            bushEffect.SetActive(false);
    }
}
