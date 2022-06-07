using UnityEngine;
using UnityEngine.UI;

public class AddListener_Test : MonoBehaviour
{
    public Button btn1, btn2, btn3;

    void Start() {
        btn1.onClick.AddListener(btn1print);
        btn2.onClick.AddListener(delegate { btn2print("Hello"); });
        btn3.onClick.AddListener(() => btn3print("goodbye"));
    }

    void btn1print()
    {
        Debug.Log("Test");
    }    
    void btn2print(string message)
    {
        Debug.Log(message);
    }    
    void btn3print(string message)
    {
        Debug.Log(message);
    }    
}