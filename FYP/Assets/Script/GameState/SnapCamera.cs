using Cinemachine;
using System.Collections;
using UnityEngine;


public class SnapCamera : GameBaseState
{
    [SerializeField] int snapTries;

    //UI stuff
    [SerializeField] Transform CameraReticle;
    [SerializeField] Vector2 camSize;
    [SerializeField] LayerMask npcLayer;
    RaycastHit2D[] contact;
    [SerializeField] GameObject[] taggedGameObject;
    [SerializeField] Transform closestGameObject;


    //Virtual Camera
    [SerializeField] CinemachineVirtualCamera outCam;
    [SerializeField] CinemachineVirtualCamera zoomCam;
    [SerializeField] GameObject outCamGameObject;

    //AudioManager audioManager;

    bool camMode;

    [Header("Camera Zooming")]
    //[SerializeField] private Camera cam;
    //[SerializeField] private float zoomSpeed = 20f;
    //[SerializeField] float zoomSens;
    //[SerializeField] private float minCamSize = 2f;
    //[SerializeField] private float maxCamSize = 5f;
    float newZoomLevel;

    Vector3 origin;
    Vector3 difference;
    bool drag;
    [Header("Camera Pan")]
    [SerializeField] float xBound;
    [SerializeField] float yBound;
    Vector2 CamOrigin;
    //Boundary Object
    [SerializeField] Transform boundaryObj;

    [Header("Dialogue indicator")]
    [SerializeField] Transform visualCue;
    [SerializeField] Vector3 visualCueOffset;

    private void Start()
    {
        CamOrigin = outCamGameObject.transform.position;
        //newZoomLevel = outCam.m_Lens.OrthographicSize;
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public Vector2 CalculateBounds()
    {
        var bound = boundaryObj.GetComponent<SpriteRenderer>().bounds;
        return bound.extents;
    }
    public override void EnterState(GameStateManager gameStateManager)
    {
        Cursor.visible = false;
        CameraReticle.gameObject.SetActive(true);
        gSm = gameStateManager;
        LeanTween.reset();
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {
        contact = Physics2D.BoxCastAll(CameraReticle.position, camSize, 0, Vector2.zero, 0, npcLayer);

        if (contact != null && contact.Length > 0){
            RayToCollider();
        }
        if (contact.Length == 0){
            taggedGameObject = new GameObject[0];
        }


        if (!camMode)
        {
            CameraPan();
            WhenMouseIsMoving();
            SnapSystem();
            //Zoom();

        }
    }
    public override void ExitState(GameStateManager gameStateManager)
    {
        CameraReticle.gameObject.SetActive(false);
    }

    public void Zoom()
    {
        // Get MouseWheel-Value and calculate new Orthographic-Size
        // (while using Zoom-Speed-Multiplier)
        //SnewZoomLevel -= Input.GetAxis("Mouse ScrollWheel") * zoomSens;
        //newZoomLevel = cam.orthographicSize - mouseScrollWheel;

        //newZoomLevel = Mathf.Clamp(newZoomLevel, minCamSize, maxCamSize);
        //float camSize = Mathf.MoveTowards(outCam.m_Lens.OrthographicSize, newZoomLevel, zoomSpeed * Time.deltaTime);
        //outCam.m_Lens.OrthographicSize = newZoomLevel;


        /*if(mouseScrollWheel !=0)
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
        }*/

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
    void PositionVisualCue()
    {
        if (closestGameObject)
        {
            visualCue.gameObject.SetActive(true);
            visualCue.position = closestGameObject.position+visualCueOffset;
        }
        else
        {
            visualCue.gameObject.SetActive(false);
        }
    }

    void WhenMouseIsMoving()
    {
        closestGameObject = GetClosestEnemy(taggedGameObject);
        PositionVisualCue();     
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CameraReticle.position = mousePosition;
    }


    void SnapSystem()
    {
        if (Input.GetMouseButtonDown(0) && taggedGameObject.Length >0)
        {         
            //audioManager.PlaySFX(audioManager.camSnap);
            ZoomToTarget();
            camMode = true;
            
        }
    }

    void CameraPan()
    {
        if(Input.GetMouseButton(1))
        {
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - outCam.transform.position;
            if(drag == false)
            {
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }
        if(drag)
        {
            //X Y pan max
            float maxX = Mathf.Clamp(origin.x - difference.x, -xBound, xBound);
            float maxY = Mathf.Clamp(origin.y - difference.y, CamOrigin.y-yBound, CamOrigin.y+yBound);

            outCam.transform.position = new Vector3(maxX, maxY, outCam.transform.position.z);         
        }
    }
    void ZoomToTarget()
    {
        zoomCam.Priority = 2;
        zoomCam.Follow = closestGameObject;
        
        //If closest = taret npc, remove the target Npc
        ScoreManager.Instance.Updatetargets(closestGameObject.transform);

        //Set its layer to be infront
        closestGameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;

        //Switch the npc state
        NpcStateManager nSm = closestGameObject.GetComponent<NpcStateManager>();      
        nSm.SwitchState(nSm.promptState);
   
        //Set dialogue stuff to be active and pass npc parameters
        gSm.nSm = nSm;
        LeanTween.move(CameraReticle.gameObject,
            closestGameObject.position + zoomCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset,
            0.5f).setOnComplete(()=>gSm.ChangeStat(gSm.dialogueStat));
    }


    
    Transform GetClosestEnemy(GameObject[] enemies)
    {
        if (enemies.Length == 0) return null;
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
        return bestTarget;
    }

    public void BackToOutCam()
    {
        zoomCam.Priority = 1;
        zoomCam.Follow = null;
        camMode = false;

        //Set its layer to be normal
        closestGameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;

        NpcStateManager nSm = closestGameObject.GetComponent<NpcStateManager>();
        nSm.SwitchState(nSm.roamState);
    }

}
