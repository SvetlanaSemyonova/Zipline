using System.Collections.Generic;
using UnityEngine;
using WrappingRopeLibrary.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    public int levelno;
    public Transform ropeparent;
    [HideInInspector] public List<Vector3> wapoints = new List<Vector3>();
    [HideInInspector] public GameObject[] peoples = new GameObject[10];
    public bool locked = false;
    public static gamemanager Instance;
    [HideInInspector] public List<Transform> peopleparent = new List<Transform>();
    [HideInInspector] public List<Transform> reachedpeople = new List<Transform>();
    [HideInInspector] public Transform people;
    float time;
    public int childs, spawnpeople;
    [HideInInspector] public int spawnedno, peoplereached;
    [HideInInspector] public float posX, posY;
    bool limitreached, limitreached1;

    [HideInInspector] public Transform lastpos;

    public Transform startpos;
    public GameObject blood;
    public int recived;
    public Text reachedtext;
    public int peopletofinish;
    bool level;
    bool levelpass;
    public GameObject endlevel, completelevel, effect;
    public Text levelshowtext;
    int levelshow, retryno;
    public bool laser = false;
    public GameObject exlpode_effect;
    private GameObject retrybuttonprefab;
    public int playerssent = 0;

    private void Awake()
    {
        levelno = PlayerPrefs.GetInt("levelno", 1);
        levelshow = SceneManager.GetActiveScene().buildIndex;
        retryno = SceneManager.GetActiveScene().buildIndex;
        Instance = this;
    }

    private void Start()
    {
        levelshowtext = GameObject.Find("levelshow").GetComponent<Text>();
        effect = GameObject.Find("exlposioneffect");
        completelevel = GameObject.Find("gamewinpanel");
        endlevel = GameObject.Find("gameoverpanel");
        recived = spawnpeople;
        posX = -2.5f;
        posY = -3.2f;

        retrybuttonprefab = GameObject.Find("retrybutton");

        createpeople();
        checkpeople();
        lastpos = GameObject.Find("lastpos").GetComponent<Transform>();
        completelevel.SetActive(false);
        endlevel.SetActive(false);
        effect.SetActive(false);
    }

    private void Update()
    {
        levelshowtext.text = "L e v e l  " + levelshow;

        reachedtext.text = reachedpeople.Count + "/" + peopletofinish;


        if (reachedpeople.Count == peopletofinish || reachedpeople.Count >= peopletofinish)
        {
            retrybuttonprefab.SetActive(false);
            levelpassmethod();
            completelevel.SetActive(true);
            effect.SetActive(true);

            if (levelpass == true)
            {
                loadlevel();
                levelpass = false;
            }
        }
        else if (recived <= 0 && reachedpeople.Count < peopletofinish && playerssent == spawnpeople)
        {
            retrylevel();

            endlevel.SetActive(true);
        }
    }

    public void lastposchange(int i, Transform pos)
    {
        if (i == 0)
        {
            reachedpeople[i].transform.position = lastpos.position;
        }
        else if (i < 10)
        {
            pos.position = new Vector3(posX + .1f, posY, .08f);
            reachedpeople[i].transform.position = pos.position;
        }

        else if (i >= 10)
        {
            if (limitreached == false)
            {
                posX = reachedpeople[0].position.x;
                posY = pos.position.y + .1f;
                pos.position = new Vector3(posX + .1f, posY, .08f);
                limitreached = true;
                reachedpeople[i].transform.position = new Vector3(posX, posY, 0.8f);
            }
            else if (limitreached && childs < 20)
            {
                reachedpeople[i].transform.position = new Vector3(posX + .1f, posY, 0.8f);
            }

            if (i >= 20)
            {
                if (limitreached1 == false)
                {
                    posX = reachedpeople[0].position.x;
                    posY = pos.position.y + .1f;
                    pos.position = new Vector3(posX + .1f, posY, .08f);
                    limitreached1 = true;
                    Debug.Log("next x");

                    reachedpeople[i].transform.position = new Vector3(posX, posY, 0.8f);
                }
                else if (limitreached1)
                {
                    Debug.Log("next x_continous");
                    reachedpeople[i].transform.position = new Vector3(posX + .1f, posY, 0.8f);
                }
            }
        }


        Debug.Log(pos.position);
    }

    void createpeople()
    {
        for (var i = 0; i < spawnpeople; i++)
        {
            if (i == 10)
            {
                startpos.position =
                    new Vector3(startpos.position.x - .1f * 10, startpos.position.y, startpos.position.z);
            }

            if (i == 20)
            {
                startpos.position =
                    new Vector3(startpos.position.x - .1f * 10, startpos.position.y, startpos.position.z);
            }

            if (levelno < 1)
            {
            }

            startpos.position = new Vector3(startpos.position.x + .1f, startpos.position.y, startpos.position.z);

            GameObject go;
            var select = peoples[Random.Range(0, 10)];
            if (i == 0)
            {
                go = Instantiate(select, startpos.position, startpos.rotation);
                Debug.Log("one");
            }
            else
            {
                go = Instantiate(select, startpos.position, startpos.rotation);
                Debug.Log("greater than one..");
            }

            go.transform.SetParent(people);
        }
    }

    public void timer()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (spawnedno < peopleparent.Count)
            {
                activepeople(spawnedno);
                time = .1f;

                Debug.Log("spawn people");
            }
        }
    }


    void activepeople(int i)
    {
        peopleparent[i].gameObject.GetComponent<follow>().enabled = true;

        Debug.Log("Come On...!!");
        spawnedno++;
    }


    void checkpeople()
    {
        foreach (Transform addpeople in people)
        {
            peopleparent.Add(addpeople);
            for (var i = 0; i < peopleparent.Count; i++)
            {
                peopleparent[i].gameObject.GetComponent<follow>().enabled = false;
            }
        }
    }

    public void calculate()
    {
        var temp = new List<Transform>();
        foreach (Transform trans in ropeparent)
        {
            temp.Add(trans);
        }

        for (var i = 0; i < temp.Count; i++)
        {
            wapoints.Add(temp[i].GetComponent<Piece>().FrontBandPoint.PositionInWorldSpace);

            if (i == temp.Count - 1)
            {
                wapoints.Add(temp[i].GetComponent<Piece>().BackBandPoint.PositionInWorldSpace);
            }
        }
    }

    void levelpassmethod()
    {
        if (Input.GetMouseButtonDown(0))
        {
            levelpass = true;
        }
    }

    public void retrylevel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(retryno);
        }
    }

    public void retrybutton()
    {
        FindObjectOfType<AdManager>().ShowAdmobInterstitial();
        SceneManager.LoadScene(retryno);
    }

    public void loadlevel()
    {
        levelno++;
        SceneManager.LoadScene(levelno);
        PlayerPrefs.SetInt("levelno", levelno);
        FindObjectOfType<AdManager>().ShowAdmobInterstitial();
        Debug.Log("adshow");
    }
}