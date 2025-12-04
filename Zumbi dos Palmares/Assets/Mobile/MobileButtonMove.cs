using UnityEngine;

public class MobileButtonMove : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    bool up, down, left, right;
    bool run = false; // << NOVO

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (up) move += Vector2.up;
        if (down) move += Vector2.down;
        if (left) move += Vector2.left;
        if (right) move += Vector2.right;

        // Escolhe velocidade
        float currentSpeed = run ? runSpeed : walkSpeed;

        transform.Translate(move.normalized * currentSpeed * Time.deltaTime);
    }

    public void UpPressed() { up = true; }
    public void UpReleased() { up = false; }

    public void DownPressed() { down = true; }
    public void DownReleased() { down = false; }

    public void LeftPressed() { left = true; }
    public void LeftReleased() { left = false; }

    public void RightPressed() { right = true; }
    public void RightReleased() { right = false; }

    // --------- CORRER (NOVO) ---------
    public void RunPressed()
    {
        run = true;
    }

    public void RunReleased()
    {
        run = false;
    }
}
