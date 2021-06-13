using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject resetPopup;
    public UIGrid grid;

    public GameObject configPopup;
    public InputField noOfCellsIF;
    public InputField aoiIF;
   
    void Start()
    {
        configPopup.SetActive(true);
        resetPopup.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            if (configPopup.activeSelf)
                configPopup.SetActive(false);
            resetPopup.SetActive(true);
        }
    }

    public void OnClickShow()
    {
        int aOI = 0;
        int.TryParse(aoiIF.text, out aOI);

        int noOfCell = 0;
        int.TryParse(noOfCellsIF.text, out noOfCell);

        if(aOI < 1 || noOfCell < 1)
        {
            Debug.LogError($"Invalid input type: AOE :{aOI}, and NoOfCell :{noOfCell}");
            return;
        }
        Debug.Log($"AOE :{aOI}, and NoOfCell :{noOfCell}");
        configPopup.SetActive(false);
        grid.PopulateGrid(noOfCell, aOI);
    }

    public void OnClickYes()
    {
        resetPopup.SetActive(false);
        Reload();
    }

    public void OnClickNo()
    {
        resetPopup.SetActive(false);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
