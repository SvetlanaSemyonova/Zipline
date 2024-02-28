using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {
    List<Vector3> waypoints;
    List<Transform> peopleparent;
    public float movespeed = 6;
    int waypointindex = 0;
    public GameObject end;
    Rigidbody2D rb;
    BoxCollider2D box;
    float thrust = 3f;
    public Animator anim;
    int peoplereached;
    bool animations;
    bool move = true;
    bool add=false;
    Transform lastpos;
    public camerashake Camerashake;
    int i;
   
    private void Start()
    {

         var go = Instantiate(AudioManager.instance.empty, transform.position, Quaternion.identity);
        Destroy(go, 2f);
        box = this.gameObject.AddComponent<BoxCollider2D>();
        i = 0;
       
        rb = gameObject.GetComponent<Rigidbody2D>();
        waypoints = gamemanager.Instance.wapoints;
        end = GameObject.Find("end");
       
        anim = GetComponent<Animator>();
        box.enabled = false;
   
      
        animations = true;
      
       
      
    }

    private void Update()
    {

        check();
        lastpos = gamemanager.Instance.lastpos;
        var dis = Vector3.Distance(lastpos.position , transform.position);
        if (dis < .01f)
        {
            follow script;
            script = GetComponent<follow>();
            script.enabled = false;

        }
        if(gamemanager.Instance.laser)
        {
            gamemanager.Instance.laser = false;
          


        }
    }

   
    void check()
    {
       
        var distance = Vector3.Distance(transform.position, end.transform.position);
       if(distance>.01f &&move)
        {
           
            spawnpeople();
            box.enabled = true;
            box.isTrigger = true;
            if (animations)
            {
                gamemanager.Instance.playerssent++;
                anim.SetBool("climb", true);
                animations = false;
              
            }
          

        }
       if(distance<.01f)
        {
            box.enabled = false;
            if (i==0)
            {
                gamemanager.Instance.reachedpeople.Add(transform);
                i = 1;
            }
         
            gamemanager.Instance.recived--;
            
            anim.SetBool("climb", false);
            gamemanager.Instance.lastposchange(gamemanager.Instance.childs,gamemanager.Instance.lastpos);
            move = false;
            gamemanager.Instance.childs++;
            Debug.Log("running..");
           

            gamemanager.Instance.posX = transform.position.x;
            gamemanager.Instance.posY = transform.position.y;


          
          
        }

    }

   
    void spawnpeople()
    {
           transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointindex], movespeed * Time.deltaTime);

            var distance = Vector3.Distance(transform.position, waypoints[waypointindex]);
            if (distance < .01f && waypointindex < waypoints.Count - 1)
            {
                waypointindex++;

            }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if(collision.gameObject.CompareTag("Player"))
        {
            var go = Instantiate(AudioManager.instance.coin, transform.position, Quaternion.identity);
            Debug.Log("wroked...!!!");
            gamemanager.Instance.peoplereached++;
          
        }
        if(collision.gameObject.CompareTag("enemy"))
        {
          
            if(add==false)
            {
                gamemanager.Instance.recived--;
                add = true;

            }
           
            box.enabled = false;
            var blood = Instantiate(gamemanager.Instance.blood, transform.position, Quaternion.identity);
            blood.transform.SetParent(this.transform);
            anim.SetBool("climb", false);
            Debug.Log("killed...");
            movespeed = 0;
            if(transform.GetComponent<Rigidbody2D>()==null)
            {
                rb = this.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                var dir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(dir * thrust, ForceMode2D.Impulse);
            }
         
          
         
            box.enabled = false;
            Destroy(gameObject, 3f);
          
          
          

        }
        if (collision.gameObject.CompareTag("bomb"))
        {
            if(add==false)
            {
                gamemanager.Instance.recived--;
                add = true;
            }
           
            var exlpode = Instantiate(gamemanager.Instance.exlpode_effect, collision.transform.position, Quaternion.identity);
            var blood = Instantiate(gamemanager.Instance.blood, transform.position, Quaternion.identity);
            blood.transform.SetParent(this.transform);
            anim.SetBool("climb", false);
            Debug.Log("killed...");
            movespeed = 0;
            if(transform.GetComponent<Rigidbody2D>()==null)
            {
                rb = this.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                var dir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(transform.right * 7f, ForceMode2D.Impulse);

            }
           
            box.enabled = false;
            Destroy(gameObject, 3f);
            Destroy(collision.gameObject);




        }


    }
}
