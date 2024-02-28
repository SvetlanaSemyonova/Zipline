using System.Collections.Generic;
using UnityEngine;
using WrappingRopeLibrary.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    public static gamemanager Instance;

    [HideInInspector] public List<Vector3> wapoints = new List<Vector3>();
    [HideInInspector] public GameObject[] peoples = new GameObject[10];
    [HideInInspector] public List<Transform> peopleparent = new List<Transform>();
    [HideInInspector] public List<Transform> reachedpeople = new List<Transform>();
    [HideInInspector] public Transform people;
    [HideInInspector] public int spawnedno, peoplereached;
    [HideInInspector] public float posX, posY;
    [HideInInspector] public Transform lastpos;

    public int LevelNumber;
    public int recived;
    public int childs, spawnpeople;
    public int peopletofinish;
    public int playerssent = 0;
    public Transform ropeparent;
    public Transform startpos;
    public Text reachedtext;
    public Text levelshowtext;
    public bool locked;
    public bool laser;
    public GameObject blood;
    public GameObject exlpode_effect;
    public GameObject endlevel, completelevel, effect;

    private float time;
    private bool level;
    private bool levelpass;
    private bool limitreached, limitreached1;
    private int levelshow, retryno;
    private GameObject retrybuttonprefab;

    private void Awake()
    {
        LevelNumber = PlayerPrefs.GetInt("LevelNumber", 1);
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

        CreatePeople();
        Check();
        lastpos = GameObject.Find("lastpos").GetComponent<Transform>();
        completelevel.SetActive(false);
        endlevel.SetActive(false);
        effect.SetActive(false);
        levelshowtext.text = "L e v e l  " + levelshow;
    }

    private void Update()
    {
        reachedtext.text = reachedpeople.Count + "/" + peopletofinish;

        if (reachedpeople.Count == peopletofinish || reachedpeople.Count >= peopletofinish)
        {
            retrybuttonprefab.SetActive(false);
            PassLevel();
            completelevel.SetActive(true);
            effect.SetActive(true);

            if (levelpass == true)
            {
                LoadLevel();
                levelpass = false;
            }
        }
        else if (recived <= 0 && reachedpeople.Count < peopletofinish && playerssent == spawnpeople)
        {
            Retry();

            endlevel.SetActive(true);
        }
    }

    public void ChangeLastPosition(int i, Transform pos)
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

                    reachedpeople[i].transform.position = new Vector3(posX, posY, 0.8f);
                }
                else if (limitreached1)
                {
                    reachedpeople[i].transform.position = new Vector3(posX + .1f, posY, 0.8f);
                }
            }
        }
    }

    void CreatePeople()
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

            if (LevelNumber < 1)
            {
            }

            startpos.position = new Vector3(startpos.position.x + .1f, startpos.position.y, startpos.position.z);

            GameObject go;
            var select = peoples[Random.Range(0, 10)];
            if (i == 0)
            {
                go = Instantiate(select, startpos.position, startpos.rotation);
            }
            else
            {
                go = Instantiate(select, startpos.position, startpos.rotation);
            }

            go.transform.SetParent(people);
        }
    }

    public void HandleTimer()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (spawnedno < peopleparent.Count)
            {
                ActivatePeople(spawnedno);
                time = .1f;
            }
        }
    }


    void ActivatePeople(int i)
    {
        peopleparent[i].gameObject.GetComponent<Follow>().enabled = true;
        spawnedno++;
    }


    void Check()
    {
        foreach (Transform addpeople in people)
        {
            peopleparent.Add(addpeople);
            for (var i = 0; i < peopleparent.Count; i++)
            {
                peopleparent[i].gameObject.GetComponent<Follow>().enabled = false;
            }
        }
    }

    public void Calculate()
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

    void PassLevel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            levelpass = true;
        }
    }

    public void Retry()
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

    public void LoadLevel()
    {
        LevelNumber++;
        SceneManager.LoadScene(LevelNumber);
        PlayerPrefs.SetInt("LevelNumber", LevelNumber);
        FindObjectOfType<AdManager>().ShowAdmobInterstitial();
    }
}