using UnityEngine;

public class movefront : MonoBehaviour
{
    public float speed;
    public bool movingright;
    public Transform groundetection;

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        var groundinfo = Physics2D.Raycast(groundetection.position, Vector2.down, .01f);
        if (groundinfo.collider == true && !groundinfo.collider.CompareTag("people"))
        {
            if (movingright == true)
            {
                transform.eulerAngles = new Vector3(-180, 0, 0);
                movingright = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingright = true;
            }
        }
    }
}