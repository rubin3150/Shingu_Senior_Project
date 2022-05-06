using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button btn;

    public int pageNumber;
    public int maxPageNumber;
    
    void Start()
    {
        btn = this.transform.GetComponent<Button>();
        
        if(btn != null)
        {
            if(pageNumber < maxPageNumber) {
                btn.onClick.AddListener(NextPage);
            }
            else {
                return;
            }
        }
    }
    
    public void NextPage()
    {
        Destroy(transform.parent.gameObject);
        UIController.Instance.currentPage = Instantiate(UIController.Instance.pagePrefab[pageNumber].gameObject, UIController.Instance.canvas.transform);
    }
}