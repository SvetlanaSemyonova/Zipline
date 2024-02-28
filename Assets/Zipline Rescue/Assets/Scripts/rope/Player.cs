using UnityEngine;

public class Player : MonoBehaviour
{
    BoxCollider2D box;
    public GameObject peopleprefab;
    public Transform poeple, hook;
    public Animator circle;
    public GameObject rope;

    private int count = 0;
    private int touch = 0;
    private bool reached, go;
    
    private void Start()
    {
        box = GetComponent<BoxCollider2D>();

        rope = GameObject.Find("Rope");
    }

    private void Update()
    {
        if (reached)
        {
            if (Input.GetMouseButton(0))
            {
                var flag = true;
                if (flag)
                {
                    GameManager.Instance.HandleTimer();
                    GameManager.Instance.IsLocked = true;
                    flag = false;
                }
            }
        }

        if (GameManager.Instance.IsLocked)
        {
            GameManager.Instance.Calculate();
        }

        HandleMouseControl();
    }


    private void HandleMouseControl()
    {
        if (touch < 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touch++;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            touch++;
            if (go)
            {
                transform.position = hook.position;
                var mat = rope.GetComponent<MeshRenderer>().material;
                mat.color = Color.green;
                reached = true;
                box.enabled = false;
            }

            touch = 0;
        }
    }

    private void OnMouseDrag()
    {
        var mousepos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        var objectpos = Camera.main.ScreenToWorldPoint(mousepos);
        transform.position = objectpos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            go = true;
            circle.SetBool("touching", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            circle.SetBool("touching", false);
            go = false;
        }
    }
}