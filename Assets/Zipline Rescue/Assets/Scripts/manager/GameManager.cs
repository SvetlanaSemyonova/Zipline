using System.Collections.Generic;
using UnityEngine;
using WrappingRopeLibrary.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int ChildrenNumber, spawnpeople;
    [SerializeField] private int peopletofinish;
    [SerializeField] private Transform RopeParent;
    [SerializeField] private Transform StartPosition;
    [SerializeField] private Text ReachedText;
    [SerializeField] private Text CurrentLevelText;
    [SerializeField] private bool Locked;
    [SerializeField] private bool laser;
    [SerializeField] private GameObject BloodPrefab;
    [SerializeField] private GameObject ExplosionEffect;
    [SerializeField] private GameObject EndLevelPopup, WinLevelPopup, effect;
    
    [HideInInspector] public List<Vector3> wapoints = new();
    [HideInInspector] public GameObject[] peoples = new GameObject[10];
    [HideInInspector] public List<Transform> peopleparent = new List<Transform>();
    [HideInInspector] public List<Transform> reachedpeople = new List<Transform>();
    [HideInInspector] public Transform People;
    [HideInInspector] public int SpawnedNumber;
    [HideInInspector] public float posX, posY;
    [HideInInspector] public Transform LastPosition;

    public int Children
    {
        get => ChildrenNumber;
        set => ChildrenNumber = value;
    }

    public bool IsLocked
    {
        get => Locked;
        set => Locked = value;
    }

    public bool IsLaserActive
    {
        get => laser;
        set => laser = value;
    }

    public int PeopleRecieved
    {
        get => recived;
        set => recived = value;
    }

    public int LevelNumber => currentLevel;
    public int playerssent = 0;

    private int currentLevel;
    private int recived;
    private float time;
    private bool level;
    private bool levelpass;
    private bool limitreached, limitreached1;
    private int levelshow, retryno;
    private GameObject retrybuttonprefab;

    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("LevelNumber", 1);
        levelshow = SceneManager.GetActiveScene().buildIndex;
        retryno = SceneManager.GetActiveScene().buildIndex;
        Instance = this;
    }

    private void Start()
    {
        CurrentLevelText = GameObject.Find("levelshow").GetComponent<Text>();
        effect = GameObject.Find("exlposioneffect");
        WinLevelPopup = GameObject.Find("gamewinpanel");
        EndLevelPopup = GameObject.Find("gameoverpanel");
        recived = spawnpeople;
        posX = -2.5f;
        posY = -3.2f;

        retrybuttonprefab = GameObject.Find("retrybutton");

        CreatePeople();
        Check();
        LastPosition = GameObject.Find("lastpos").GetComponent<Transform>();
        WinLevelPopup.SetActive(false);
        EndLevelPopup.SetActive(false);
        effect.SetActive(false);
        CurrentLevelText.text = "L e v e l  " + levelshow;
    }

    public GameObject GetExplosionPrefab()
    {
        return ExplosionEffect;
    } 
    
    public GameObject GetBloodPrefab()
    {
        return BloodPrefab;
    }
    
    private void Update()
    {
        ReachedText.text = reachedpeople.Count + "/" + peopletofinish;

        if (reachedpeople.Count == peopletofinish || reachedpeople.Count >= peopletofinish)
        {
            retrybuttonprefab.SetActive(false);
            PassLevel();
            WinLevelPopup.SetActive(true);
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

            EndLevelPopup.SetActive(true);
        }
    }

    public void ChangeLastPosition(int i, Transform pos)
    {
        if (i == 0)
        {
            reachedpeople[i].transform.position = LastPosition.position;
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
            else if (limitreached && ChildrenNumber < 20)
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
                StartPosition.position =
                    new Vector3(StartPosition.position.x - .1f * 10, StartPosition.position.y, StartPosition.position.z);
            }

            if (i == 20)
            {
                StartPosition.position =
                    new Vector3(StartPosition.position.x - .1f * 10, StartPosition.position.y, StartPosition.position.z);
            }

            if (LevelNumber < 1)
            {
            }

            StartPosition.position = new Vector3(StartPosition.position.x + .1f, StartPosition.position.y, StartPosition.position.z);

            GameObject go;
            var select = peoples[Random.Range(0, 10)];
            if (i == 0)
            {
                go = Instantiate(select, StartPosition.position, StartPosition.rotation);
            }
            else
            {
                go = Instantiate(select, StartPosition.position, StartPosition.rotation);
            }

            go.transform.SetParent(People);
        }
    }

    public void HandleTimer()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (SpawnedNumber < peopleparent.Count)
            {
                ActivatePeople(SpawnedNumber);
                time = .1f;
            }
        }
    }


    void ActivatePeople(int i)
    {
        peopleparent[i].gameObject.GetComponent<Follow>().enabled = true;
        SpawnedNumber++;
    }


    void Check()
    {
        foreach (Transform addpeople in People)
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
        foreach (Transform trans in RopeParent)
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

    private void LoadLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
        PlayerPrefs.SetInt("LevelNumber", currentLevel);
        FindObjectOfType<AdManager>().ShowAdmobInterstitial();
    }
}