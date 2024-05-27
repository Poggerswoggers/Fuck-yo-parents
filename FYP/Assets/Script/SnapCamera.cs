using Cinemachine;
using System.Collections;
using UnityEngine;


public class SnapCamera : MonoBehaviour
{

    //UI stuff
    [SerializeField] float uiDelay;
    public GameObject blackout;

    //
    [SerializeField] DialogueManager dm;


    [SerializeField] Transform CameraReticle;
    [SerializeField] Vector2 camSize;
    public LayerMask npcLayer;
    RaycastHit2D[] contact;
    [SerializeField] GameObject[] taggedGameObject;
    [SerializeField] Transform closestGameObject;


    //Virtual Camera
    [SerializeField] CinemachineVirtualCamera outCam;
    [SerializeField] CinemachineVirtualCamera zoomCam;
    [SerializeField] GameObject outCamGameObject;

    bool camMode;


    public float LerpTime = 1f;

    [Header("Camera Zooming")]
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minCamSize = 2f;
    [SerializeField] private float maxCamSize = 5f;
    [SerializeField] float newZoomLevel;
    Vector3 cameraOrigin;


    //Boundary Object
    public Transform boundaryObj;

    NpcBaseState bs;

    public Vector2 CalculateBounds()
    {
        var bound = boundaryObj.GetComponent<SpriteRenderer>().bounds;
        return bound.extents;
    }

    private void Start()
    {
        CalculateBounds();
        cameraOrigin = outCamGameObject.transform.position;
    }

    private void Update()
    {
        //Zoom();

        contact = Physics2D.BoxCastAll(CameraReticle.position, camSize, 0, Vector2.zero, 0, npcLayer);

        
        if (contact != null && contact.Length > 0)
        {
            RayToCollider();
        }
        if(contact.Length ==0)
        {
            taggedGameObject = new GameObject[0];
        }


        if (!camMode)
        {
            WhenMouseIsMoving();
            SnapSystem();

        }



        /*if (Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            WhenMouseIsMoving();

            timeElapsed = 0f;
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed>= lockDuration)
            {
                WhenMouseIsntMoving();
            }
        }*/



    }

    public void Zoom()
    {
        // Get MouseWheel-Value and calculate new Orthographic-Size
        // (while using Zoom-Speed-Multiplier)
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        newZoomLevel = cam.orthographicSize - mouseScrollWheel;

        if(mouseScrollWheel !=0)
        {

            outCamGameObject.SetActive(false);

            // Get Position before and after zooming
            Vector3 mouseOnWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            cam.orthographicSize = Mathf.Clamp(newZoomLevel, minCamSize, maxCamSize);
            Vector3 mouseOnWorld1 = cam.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log(mouseOnWorld + "  " + mouseOnWorld1);

            // Calculate Difference between Positions before and after Zooming
            Vector3 posDiff = mouseOnWorld - mouseOnWorld1;

            // Add Difference to Camera Position
            Vector3 camPos = cam.transform.position;
            Vector3 targetPos = new Vector3(
                camPos.x + posDiff.x,
                camPos.y + posDiff.y,
                camPos.z);

            // Apply Target-Position to Camera
            cam.transform.position = targetPos;
        }

    }
    public void RayToCollider()
    {
        //contact = Physics2D.BoxCastAll(CameraReticle.position, new Vector2(2, 2), 0, Vector2.zero);

        taggedGameObject = new GameObject[contact.Length];

        for(int i=0; i<contact.Length; i++)
        {
            //Debug.Log(contact[i].collider);
            taggedGameObject[i] = contact[i].collider.gameObject;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(CameraReticle.position,camSize);
    }
    void WhenMouseIsMoving()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CameraReticle.position = mousePosition;
    }

    void WhenMouseIsntMoving()
    {
        Debug.Log("StoppedMoving");
    }

    void SnapSystem()
    {
        if(Input.GetMouseButtonDown(0) && taggedGameObject.Length >0)
        {
            camMode = true;
            //blackout.SetActive(true);

            closestGameObject = GetClosestEnemy(taggedGameObject);
            //closestGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            StartCoroutine(ZoomToTarget());
        }
    }

    IEnumerator ZoomToTarget()
    {
        //zoomCam.m_Lens.OrthographicSize = 1.5f;
        zoomCam.Priority = 2;
        zoomCam.Follow = closestGameObject;

        NpcStateManager nSm = closestGameObject.GetComponent<NpcStateManager>();
        //bs = nSm.GetCurrentState();
        nSm.SwitchState(nSm.promptState);

        LeanTween.move(CameraReticle.gameObject, closestGameObject.position + zoomCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset, 0.5f);

        yield return new WaitForSeconds(0.2f);
        dm.gameObject.SetActive(true);
        dm.LoadDialooguePanel(this);
    }


    
    Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform bestTarget = null;
        float mindist = Mathf.Infinity;
        foreach(GameObject col in enemies)
        {
            Vector2 dirToTarget = col.transform.position - CameraReticle.position;
            float dSqrTarget = dirToTarget.sqrMagnitude;
            if(dSqrTarget < mindist)
            {
                mindist = dSqrTarget;
                bestTarget = col.transform;
            }
        }
        Debug.Log(bestTarget);
        return bestTarget;
    }

    public void BackToOutCam()
    {
        zoomCam.Priority = 1;
        zoomCam.Follow = null;
        camMode = false;
        //closestGameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        //blackout.SetActive(false);

        NpcStateManager nSm = closestGameObject.GetComponent<NpcStateManager>();
        nSm.SwitchState(nSm.roamState);

    }

}
