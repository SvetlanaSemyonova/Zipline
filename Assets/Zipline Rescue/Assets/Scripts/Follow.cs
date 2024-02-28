using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
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
    bool add = false;
    Transform lastpos;
    int i;

    private void Start()
    {
        var go = Instantiate(AudioManager.instance.empty, transform.position, Quaternion.identity);
        Destroy(go, 2f);
        box = gameObject.AddComponent<BoxCollider2D>();
        i = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        waypoints = GameManager.Instance.wapoints;
        end = GameObject.Find("end");

        anim = GetComponent<Animator>();
        box.enabled = false;

        animations = true;
    }

    private void Update()
    {
        Check();
        lastpos = GameManager.Instance.LastPosition;
        var dis = Vector3.Distance(lastpos.position, transform.position);
        if (dis < .01f)
        {
            Follow script;
            script = GetComponent<Follow>();
            script.enabled = false;
        }

        if (GameManager.Instance.IsLaserActive)
        {
            GameManager.Instance.IsLaserActive = false;
        }
    }


    private void Check()
    {
        var distance = Vector3.Distance(transform.position, end.transform.position);
        if (distance > .01f && move)
        {
            SpawnPeople();
            box.enabled = true;
            box.isTrigger = true;
            if (animations)
            {
                GameManager.Instance.playerssent++;
                anim.SetBool("climb", true);
                animations = false;
            }
        }

        if (distance < .01f)
        {
            box.enabled = false;
            if (i == 0)
            {
                GameManager.Instance.reachedpeople.Add(transform);
                i = 1;
            }

            GameManager.Instance.PeopleRecieved--;

            anim.SetBool("climb", false);
            GameManager.Instance.ChangeLastPosition(GameManager.Instance.Children, GameManager.Instance.LastPosition);
            move = false;
            GameManager.Instance.Children++;
            GameManager.Instance.posX = transform.position.x;
            GameManager.Instance.posY = transform.position.y;
        }
    }


    private void SpawnPeople()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, waypoints[waypointindex], movespeed * Time.deltaTime);

        var distance = Vector3.Distance(transform.position, waypoints[waypointindex]);
        if (distance < .01f && waypointindex < waypoints.Count - 1)
        {
            waypointindex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(AudioManager.instance.coin, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            if (add == false)
            {
                GameManager.Instance.PeopleRecieved--;
                add = true;
            }

            box.enabled = false;
            var blood = Instantiate(GameManager.Instance.GetBloodPrefab(), transform.position, Quaternion.identity);
            blood.transform.SetParent(transform);
            anim.SetBool("climb", false);
            movespeed = 0;
            if (transform.GetComponent<Rigidbody2D>() == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                var dir = (transform.position - collision.transform.position).normalized;
                rb.AddForce(dir * thrust, ForceMode2D.Impulse);
            }


            box.enabled = false;
            Destroy(gameObject, 3f);
        }

        if (collision.gameObject.CompareTag("bomb"))
        {
            if (add == false)
            {
                GameManager.Instance.PeopleRecieved--;
                add = true;
            }

            Instantiate(GameManager.Instance.GetExplosionPrefab(), collision.transform.position,
                Quaternion.identity);
            var blood = Instantiate(GameManager.Instance.GetBloodPrefab(), transform.position, Quaternion.identity);
            blood.transform.SetParent(transform);
            anim.SetBool("climb", false);
            movespeed = 0;
            if (transform.GetComponent<Rigidbody2D>() == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
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