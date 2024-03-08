using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [HideInInspector]
    public string displayName = "Unnamed Entity";
    [HideInInspector]
    public Planet planet;
    [HideInInspector]
    public string dataToRead;
    private SerializableDictionary<string, string> data = new SerializableDictionary<string, string>();

    public void Awake()
    {
        Init();
        foreach(string data in dataToRead.Split(','))
        {
            
        }
    }

    public void Start()
    {
        OnSpawn();
    }

    public void Update()
    {
        WhileExists();
    }

    public virtual void Init()
    {

    }

    public virtual void OnSpawn()
    {

    }

    public virtual void WhileExists()
    {

    }

    public virtual void ForeachDataEntry(string key, string value)
    {

    }

    public void RemoveFromWorld()
    {
        Destroy(gameObject);
    }

    public void GoToCoordinates(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    public Vector2 GetCoordinates()
    {
        return transform.position;
    }

    public Vector2Int GetTileCoordinates()
    {
        return new Vector2Int((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
    }

    public void Fling(float strength, Vector2 direciton)
    {
        GetComponent<Rigidbody>().AddForce(direciton * strength, ForceMode.Impulse);
    }
}
