using System.Collections.Generic;
using UnityEngine;

public class ropescript : MonoBehaviour
{
    public Vector2 destiny, currentpos;
    public float speed = 1;
    public float distance = 2f;
    public GameObject nodeprefab;
    public GameObject player;
    public GameObject lastnod;
    public LineRenderer lr;
    public GameObject nodespos;
    int vertexcount = 2;
    public List<GameObject> nodes = new List<GameObject>();
    bool done = false;


    void Start()
    {
        nodespos = GameObject.FindGameObjectWithTag("parent");
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        lastnod = transform.gameObject;
        nodes.Add(transform.gameObject);
    }

    void Update()
    {
        currentpos = transform.position;


        if ((Vector2)transform.position == destiny)
        {
            lastnod.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            if (Vector2.Distance(player.transform.position, lastnod.transform.position) > distance)
            {
                createnod();
                done = true;
            }
        }

        renderline();
    }

    void renderline()
    {
        lr.SetVertexCount(vertexcount);
        int i;
        for (i = 0; i < nodes.Count; i++)
        {
            lr.SetPosition(i, nodes[i].transform.position);
        }

        lr.SetPosition(i, player.transform.position);
    }

    void createnod()
    {
        Vector2 position2create = player.transform.position - lastnod.transform.position;
        position2create.Normalize();
        position2create *= distance;
        position2create += (Vector2)lastnod.transform.position;
        var nodesprefabs = (GameObject)Instantiate(nodeprefab, position2create, Quaternion.identity);
        nodesprefabs.transform.SetParent(nodespos.transform);


        lastnod.GetComponent<HingeJoint2D>().connectedBody = nodesprefabs.GetComponent<Rigidbody2D>();
        lastnod = nodesprefabs;
        nodes.Add(lastnod);
        vertexcount++;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("node"))
        {
            lastnod = transform.gameObject;
        }
    }
}