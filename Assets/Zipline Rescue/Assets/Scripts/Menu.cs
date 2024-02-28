using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject LevelSelect, LevelSelectPanel;
    
    private int levelToLoad;
    private bool panelShow = false;

    private void Start()
    {
        var ii = 0;

        foreach (Transform buttons in LevelSelect.transform)
        {
            ii++;
            buttons.name = (ii).ToString();
            buttons.GetChild(0).GetComponent<TextMeshProUGUI>().text = buttons.name;
            var levelButtons = buttons.GetComponent<Button>();
            levelButtons.interactable = false;
        }

        for (var i = 0; i < gamemanager.Instance.LevelNumber; i++)
        {
            LevelSelect.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            var levelButtons = LevelSelect.transform.GetChild(i).GetComponent<Button>();
            levelButtons.interactable = true;
        }

        LevelSelectPanel.SetActive(false);
    }

    private void Update()
    {
        if (!panelShow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                panelShow = true;
                LevelSelectPanel.SetActive(true);
            }
        }
    }

    public void Play()
    {
        levelToLoad = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        SceneManager.LoadScene(levelToLoad + 1);
    }
}