using UnityEngine;

public class movecross : MonoBehaviour
{
    public float speed;
    public bool movingright;
    public Transform groundetection;
   public bool up = true ;

    private void FixedUpdate()
    {

        if (up)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);

        }else if(up==false)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);

        }

       
        var groundinfo = Physics2D.Raycast(groundetection.position, Vector2.up, .01f);
      
       
            if(groundinfo.collider.CompareTag("front"))
            {
            up = false;

        }


        if (groundinfo.collider.CompareTag("back"))
        {
                up = true;
           
                

            }

        

    }
}
