using UnityEngine;

public class joinpeople : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;
    BoxCollider2D box;
    bool follow;
    Transform followpos;
    bool add = true;

    private void Start()
    {
        follow = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (follow)
        {
            if (followpos == null)
            {
                return;
            }

            if (followpos.position != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, followpos.position, 5f * Time.deltaTime);
                transform.gameObject.tag = "people";
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("people"))
        {
            if (add)
            {
                add = false;
                gamemanager.Instance.recived++;
            }

            Destroy(rb);
            followpos = collision.transform;
            follow = true;

            anim.SetBool("climb", true);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            follow = false;
            gamemanager.Instance.reachedpeople.Add(transform);
            gamemanager.Instance.peoplereached++;
            gamemanager.Instance.ChangeLastPosition(gamemanager.Instance.childs, gamemanager.Instance.lastpos);
            gamemanager.Instance.childs++;
            anim.SetBool("climb", false);
            gamemanager.Instance.posX = transform.position.x;
            gamemanager.Instance.posY = transform.position.y;
            gamemanager.Instance.recived--;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            gamemanager.Instance.recived--;
            var blood = Instantiate(gamemanager.Instance.blood, transform.position, Quaternion.identity);
            blood.transform.SetParent(this.transform);
            anim.SetBool("climb", false);

            rb = this.gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            var dir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(dir * 3f, ForceMode2D.Impulse);
            box.enabled = false;
            Destroy(gameObject, 3f);
        }
    }
}