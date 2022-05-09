using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button btn;

    void Start()
    {
        btn.onClick.AddListener(UIController.Instance.OnClickBtn);
    }
}