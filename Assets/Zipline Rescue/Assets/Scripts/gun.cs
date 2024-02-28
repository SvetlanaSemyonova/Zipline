using UnityEngine;

public class gun : MonoBehaviour
{

    public GameObject bullet;
    public Transform spawnpoint;
    float timer = 3f;
    public int direction_selector;
    private void Start()
    {
      
     

    }

  
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer<=0)
        {
            var go = Instantiate(bullet, spawnpoint.position, spawnpoint.rotation);
            go.transform.SetParent(transform);
            timer = 3f;


        }
    }
}
