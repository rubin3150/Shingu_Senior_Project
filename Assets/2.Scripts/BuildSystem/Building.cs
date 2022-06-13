using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    public BuildingType buildingType = BuildingType.None;
    public ResourceType resourceType = ResourceType.None;
    
    private int resource;

    private int cost;
    private float buildTime;
    private int maxResource;
    private string description;
    private bool isCreation = true;
    private bool isCollect;
    private Collider _collider;
    private MeshRenderer _mesh;

    private Coroutine thisCoroutine;
    private GameObject model;
    private GameObject effect;

    private void OnEnable() 
    {
        cost = buildingData.cost;
        buildTime = buildingData.buildTime;
        maxResource = buildingData.maxResource;
        description = buildingData.description;
        _collider = this.GetComponent<Collider>();
        _mesh = this.GetComponent<MeshRenderer>();
        CreateReosource();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Stop();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CreateReosource();
        }
    }

    public void thisType()
    {
        switch (buildingType)
        {
            case BuildingType.None:
                return;
            case BuildingType.music_box:

            break;
            case BuildingType.windmil:

            break;
            case BuildingType.candlestick:

                break;
            case BuildingType.fountain:

                break;
            case BuildingType.gazebo:

                break;
            case BuildingType.Tower:

                break;
            case BuildingType.wood_house:

                break;
            case BuildingType.blacksmith_shop:

                break;
            case BuildingType.bee:

                break;
            case BuildingType.mushroom_house:

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
                thisCoroutine = StartCoroutine(Create(2, 1f));
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

    public void Stop()
    {
        StopCoroutine(thisCoroutine);
    }

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

    private IEnumerator Create(int _int, float _time)
    {
        while(isCreation)
        {
            resource += _int;
            yield return new WaitForSeconds(_time);
        }
    }

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

    public IEnumerator GetResource(GameObject _gameObject, float _time)
    {
        ResourceSystem.Instance.GetResource(resourceType, resource);
        _mesh.enabled = false;
        _collider.enabled = false;
        yield return new WaitForSeconds(_time);
        _mesh.enabled = true;
        _collider.enabled = true;
    }
}