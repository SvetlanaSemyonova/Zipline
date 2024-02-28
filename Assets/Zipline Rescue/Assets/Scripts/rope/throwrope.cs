using UnityEngine;

public class throwrope : MonoBehaviour
{
    public GameObject hook;
    GameObject currenthook;

    public GameObject cursor;

    void Update()
    {
        Vector2 destiny = cursor.transform.position;
        currenthook = cursor;
        currenthook.GetComponent<Rope>().destiny = destiny;
    }
}