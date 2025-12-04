using UnityEngine;

public class MobileButtonMove : MonoBehaviour
{
    public float speed = 5f;

    bool up, down, left, right;

    void Update()
    {
        Vector2 move = Vector2.zero;

        if (up) move += Vector2.up;
        if (down) move += Vector2.down;
        if (left) move += Vector2.left;
        if (right) move += Vector2.right;

        transform.Translate(move.normalized * speed * Time.deltaTime);
    }

    public void UpPressed() { up = true; }
    public void UpReleased() { up = false; }

    public void DownPressed() { down = true; }
    public void DownReleased() { down = false; }

    public void LeftPressed() { left = true; }
    public void LeftReleased() { left = false; }

    public void RightPressed() { right = true; }
    public void RightReleased() { right = false; }
}
