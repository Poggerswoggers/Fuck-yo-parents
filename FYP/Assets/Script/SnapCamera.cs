using Cinemachine;
using System.Collections;
using UnityEngine;


public class SnapCamera : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    public GameObject blackout;

    [SerializeField] Transform CameraReticle;

    RaycastHit2D[] contact;
    [SerializeField] GameObject[] taggedGameObject;
    [SerializeField] Transform closestGameObject;


    //Virtual Camera
    [SerializeField] CinemachineVirtualCamera outCam;
    [SerializeField] CinemachineVirtualCamera zoomCam;

    bool camMode;


    public float LerpTime = 1f;


    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minCamSize = 2f;
    [SerializeField] private float maxCamSize = 5f;

    private void Start()
    {

    }

    private void Update()
    {
        Zoom();

        contact = Physics2D.BoxCastAll(CameraReticle.position, new Vector2(2,2), 0, Vector2.zero);

        
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
        float newZoomLevel = outCam.m_Lens.OrthographicSize - mouseScrollWheel;

        // Get Position before and after zooming
        Vector3 mouseOnWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        outCam.m_Lens.OrthographicSize = Mathf.Clamp(newZoomLevel, minCamSize, maxCamSize);
        Vector3 mouseOnWorld1 = cam.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log(mouseOnWorld + "  " + mouseOnWorld1);

        // Calculate Difference between Positions before and after Zooming
        Vector3 posDiff = mouseOnWorld - mouseOnWorld1;

        // Add Difference to Camera Position
        Vector3 camPos = outCam.transform.position;
        Vector3 targetPos = new Vector3(
            camPos.x + posDiff.x,
            camPos.y + posDiff.y,
            camPos.z);

        // Apply Target-Position to Camera
        outCam.transform.position = targetPos;
    }
    public void RayToCollider()
    {
        contact = Physics2D.BoxCastAll(CameraReticle.position, new Vector2(2, 2), 0, Vector2.zero);

        taggedGameObject = new GameObject[contact.Length];

        for(int i=0; i<contact.Length; i++)
        {
            //Debug.Log(contact[i].collider);
            taggedGameObject[i] = contact[i].collider.gameObject;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(CameraReticle.position, new Vector3(2, 2, 0));
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
        /*if(Input.GetMouseButtonDown(0) && taggedGameObject.Length >0)
        {
            camMode = true;
            //blackout.SetActive(true);

            closestGameObject = GetClosestEnemy(taggedGameObject);
            closestGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            ZoomToTarget();
        }*/
    }

    void ZoomToTarget()
    {
        //zoomCam.m_Lens.OrthographicSize = 1.5f;
        zoomCam.Priority = 2;
        zoomCam.Follow = closestGameObject;
        CameraReticle.position = Vector2.zero;
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

    public void backButton()
    {
        Panel.SetActive(false);
        Camera.main.orthographicSize = 5f;
        camMode = false;
        closestGameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        //blackout.SetActive(false);
    }

}
