using UnityEngine;

public class bullet : MonoBehaviour
{
    public gun gunscript;
    int dir;
    private void Start()
    {
        gunscript = transform.GetComponentInParent<gun>();
        dir =gunscript.direction_selector;
      
    }

    // Update is called once per frame
    void Update()
    {
        if(dir==0)
        {
            transform.position += Vector3.up * 3f * Time.deltaTime;
        }
        if (dir == 1)
        {
            transform.position += Vector3.down * 3f * Time.deltaTime;
        }
        if (dir == 2)
        {
            transform.position += Vector3.right * 3f * Time.deltaTime;
        }
        if (dir == 3)
        {
            transform.position += Vector3.left * 3f * Time.deltaTime;
        }
    }
}
