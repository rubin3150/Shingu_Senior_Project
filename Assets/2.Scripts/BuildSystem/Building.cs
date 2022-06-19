using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public BuildingType buildingType = BuildingType.None;
    public ResourceType resourceType = ResourceType.None;
    
    public int resource;
    public bool isCollect;

    [HideInInspector] public int cost1;
    private int cost2;
    private int cost3;
    private int cost4;
    private int cost5;
    private float buildTime;
    private int maxResource;
    //private string description;

    private Collider _collider;
    private MeshRenderer _mesh;

    private Coroutine thisCoroutine;
    private GameObject model;
    private GameObject effect;

    private void OnEnable() 
    {
        cost1 = buildingData.cost1;
        cost2 = buildingData.cost2;
        cost3 = buildingData.cost3;
        cost4 = buildingData.cost4;
        cost5 = buildingData.cost5;

        buildTime = buildingData.buildTime;
        maxResource = buildingData.maxResource;
        //description = buildingData.description;e 
        _collider = this.GetComponent<Collider>();
        _mesh = this.GetComponent<MeshRenderer>();
        CreateReosource();
    }

    private void Update()
    {
        switch (buildingType)
       {
           case BuildingType.None:
               return;
           case BuildingType.music_box:
                if (resource >= 150)
                isCollect = true;
                if (resource >= 300)
                resource = 300;
           break;
        }
    }
    
    public void CreateReosource()
    {
        switch (resourceType)
        {
            case ResourceType.None:
                return;
            case ResourceType.childlikeEnergy:
                Invoke(nameof(DealyFunc), buildTime);
                return;
            case ResourceType.log:
                resource = maxResource;
                
                return;
            case ResourceType.flower:
                resource = maxResource;
                
                return;
            case ResourceType.ore:
                resource = maxResource;
                
                return;
            case ResourceType.mushroom:
                resource = maxResource;
                
                return;
        }
    }

    public void DealyFunc()
    {
        thisCoroutine = StartCoroutine(Create(2, 1f));
    }

    [System.Obsolete]
    public void buildBuilding(GameObject _model, GameObject _effect)
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        _collider.enabled = false;
        model = Instantiate(_model, transform);
        effect = Instantiate(_effect, transform);
        model.transform.localPosition = new Vector3(0, -0.5f, 0);
        model.transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y, 0);
        model.transform.localScale = new Vector3(0.31f, 0.31f * this.transform.localScale.x, 0.31f);
        effect.transform.localScale = new Vector3(0.7f, 0.31f * this.transform.localScale.x, 0.7f);
        StartCoroutine(DestroyEffect(model, effect, buildTime));
    }

    public IEnumerator Create(int _int, float _time)
    {
        yield return new WaitForSeconds(_time);
        resource += _int;
        Debug.Log("달빛 에너지를 생산 중입니다");
        StartCoroutine(Create(2, 1f));
    }

    [System.Obsolete]
    private IEnumerator DestroyEffect(GameObject _model, GameObject _effect, float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(_model);
        Destroy(_effect);
        _collider.enabled = true;
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public IEnumerator GetResource(float _time)
    {
        ResourceSystem.Instance.GetResource(resourceType, resource);
        _mesh.enabled = false;
        _collider.enabled = false;
        yield return new WaitForSeconds(_time);
        _mesh.enabled = true;
        _collider.enabled = true;
    }

    public void GetBuildingResource(Building _gameObject)
    {
        if(_gameObject.resourceType == ResourceType.childlikeEnergy)
        {
            ResourceSystem.Instance.GetResource(resourceType, resource);
            //StartCoroutine(Create(2, 1f));
        }
    }
}