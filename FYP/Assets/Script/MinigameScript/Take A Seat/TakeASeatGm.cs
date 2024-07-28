using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeASeatGm : BaseMiniGameClass
{
    [SerializeField] float speed;

    [SerializeField] Transform grabbedCommuter;
    Vector3 offset;

    [Header("Object Pool")]
    public static TakeASeatGm SharedInstance;
    public List<GameObject> pooledCommunters { get; set; }
    public GameObject objectToPool;
    public int amountToPool;

    [Header("Start/End Pos")]
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [Header("Config")]
    [SerializeField] float timeBetweenCommuters;
    float _timeBetweenCommuters;

    //Reference
    CommunterQueue communterQueue;

    public override void EndSequenceMethod()
    {
        
    }

    public override void StartGame()
    {
        LeanTween.reset();
        _timeBetweenCommuters = timeBetweenCommuters;

        communterQueue = new CommunterQueue(startPos.position, endPos.position, this);
        isGameActive = true;
    }

    public override void UpdateGame()
    {
        SpawnCommuter();
        PlayerInput();
    }
    void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            Debug.Log(hit.transform);
            if (hit.transform.GetComponent<Commuter>())
            {
                grabbedCommuter = hit.transform;
                LeanTween.cancel(grabbedCommuter.gameObject);
                offset = grabbedCommuter.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0) && grabbedCommuter)
        {
            grabbedCommuter = null;
        }

        if (grabbedCommuter)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos += offset;
            grabbedCommuter.position = mousePos;
        }
    }

    void SpawnCommuter()
    {
        _timeBetweenCommuters -= Time.deltaTime;
        if (_timeBetweenCommuters < 0)
        {
            _timeBetweenCommuters = timeBetweenCommuters;
            communterQueue.InitialiseCommuter(GetPooledObject());
        }
    }

    protected override IEnumerator InstructionCo()
    {
        StartGame();
        yield return null;
    }

    void Awake()
    {
        SharedInstance = this;

        pooledCommunters = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledCommunters.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledCommunters[i].activeInHierarchy)
            {
                return pooledCommunters[i];
            }
        }
        return null;
    }
}
