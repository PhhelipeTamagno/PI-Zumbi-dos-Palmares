using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animator animator;

    public void PlayJogo()
    {
        animator.SetTrigger("Jogo");
    }

    public void PlaySettings()
    {
        animator.SetTrigger("Settings");
    }
}