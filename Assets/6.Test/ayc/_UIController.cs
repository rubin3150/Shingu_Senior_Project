using UnityEngine;

public class _UIController : MonoBehaviour
{
    public GameObject popUP;
    public GameObject setting;
    public GameObject shop;

    public void ShopOnOff(bool i)
    {
        if(i) {
            shop.SetActive(true);
        }
        else {
            shop.SetActive(false);
        }
    }

    public void PopUpOnOff(bool i)
    {
        if(i) {
            popUP.SetActive(true);
        }
        else {
            popUP.SetActive(false);
        }
    }

    public void SettingOnOff(bool i)
    {
        if(i) {
            setting.SetActive(true);
        }
        else {
            setting.SetActive(false);
        }
    }
}