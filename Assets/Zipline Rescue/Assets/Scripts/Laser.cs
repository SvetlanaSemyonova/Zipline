using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform laserpos;
    public GameObject spark;
    private LineRenderer line;
    private bool add = true;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = true;
        line.useWorldSpace = true;
    }

    void Update()
    {
        var hit = Physics2D.Raycast(transform.position, transform.right);

        Debug.DrawLine(transform.position, hit.point);
        laserpos.position = hit.point;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, laserpos.position);
        spark.transform.position = hit.point;


        if (hit.collider.CompareTag("people"))
        {
            BoxCollider2D box;
            Follow followscript;
            if (hit.transform.GetComponent<Follow>() != null)
            {
                box = hit.collider.GetComponent<BoxCollider2D>();
                box.enabled = false;
                followscript = hit.collider.GetComponent<Follow>();
                followscript.movespeed = 0;
            }

            Rigidbody2D rb;

            var blood = Instantiate(gamemanager.Instance.blood, hit.collider.gameObject.transform.position,
                Quaternion.identity);
            blood.transform.SetParent(hit.collider.transform);
            if (hit.collider.GetComponent<Rigidbody2D>() == null)
            {
                rb = hit.collider.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                var dir = (transform.position - hit.collider.transform.position).normalized;
                rb.AddForce(dir * 3f, ForceMode2D.Impulse);
            }
        }
    }
}