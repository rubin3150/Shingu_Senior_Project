using UnityEngine;

public class _UIController : MonoBehaviour
{
    public GameObject popUP;
    public GameObject setting;
    public GameObject shop;
    /*
    public GameObject[] mainCampPage;
    */

    public void ShopOnOff(bool _bool)
    {
        if(_bool) 
            shop.SetActive(true);
        else
            shop.SetActive(false);
    }

    public void PopUpOnOff(bool _bool)
    {
        if(_bool) 
            popUP.SetActive(true);
        else
            popUP.SetActive(false);
    }

    public void SettingOnOff(bool _bool)
    {
        if(_bool) 
            setting.SetActive(true);
        else
            setting.SetActive(false);
    }

    /*
    public void mainCampOnOff(bool _bool, int _int)
    {
        if(_bool)
            mainCampPage[_int].SetActive(true);
        else
            mainCampPage[_int].SetActive(true);
    }
    */
}