using UnityEngine;

public class patrol : MonoBehaviour {

    public float speed;
    public bool movingright;
    public Transform groundetection;

    private void Update()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        var groundinfo = Physics2D.Raycast(groundetection.position, Vector2.down,.2f);
        if(groundinfo.collider==false)
        {
            if(movingright==true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
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
