using UnityEngine;

public class PeopleJointer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private  BoxCollider2D boxCollider;
    
    private bool follow;
    private bool add = true;
    private Transform followTransform;

    private void Start()
    {
        follow = false;
    }

    private void Update()
    {
        if (follow)
        {
            if (followTransform == null)
            {
                return;
            }

            if (followTransform.position != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, followTransform.position, 5f * Time.deltaTime);
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
                GameManager.Instance.PeopleRecieved++;
            }

            Destroy(rigidbody);
            followTransform = collision.transform;
            follow = true;

            animator.SetBool("climb", true);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            follow = false;
            GameManager.Instance.reachedpeople.Add(transform);
            GameManager.Instance.ChangeLastPosition(GameManager.Instance.Children, GameManager.Instance.LastPosition);
            GameManager.Instance.Children++;
            animator.SetBool("climb", false);
            GameManager.Instance.posX = transform.position.x;
            GameManager.Instance.posY = transform.position.y;
            GameManager.Instance.PeopleRecieved--;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            GameManager.Instance.PeopleRecieved--;
            var blood = Instantiate(GameManager.Instance.GetBloodPrefab(), transform.position, Quaternion.identity);
            blood.transform.SetParent(transform);
            animator.SetBool("climb", false);

            rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 1f;
            var dir = (transform.position - collision.transform.position).normalized;
            rigidbody.AddForce(dir * 3f, ForceMode2D.Impulse);
            boxCollider.enabled = false;
            Destroy(gameObject, 3f);
        }
    }
}