using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    //public UIController uiController;
    public Button btn;
    public int pageNumber;
    void Start()
    {
        btn = this.transform.GetComponent<Button>();
        if(btn != null)
        {
            //btn.onClick.AddListener(UIController.Instance.BtnClick);
            btn.onClick.AddListener(NextPage);
        }
    }
    
    public void NextPage()
    {
        int a = pageNumber;
        Destroy(transform.parent.gameObject);
        UIController.Instance.currentPage = Instantiate(UIController.Instance.pagePrefab[pageNumber].gameObject, UIController.Instance.canvas.transform);
    }
}
