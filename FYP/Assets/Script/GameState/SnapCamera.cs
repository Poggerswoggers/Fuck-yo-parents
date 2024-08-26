using Cinemachine;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class SnapCamera : GameBaseState
{
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

    bool camMode;

    Vector3 origin;
    Vector3 difference;
    bool drag;
    [Header("Camera Pan")]
    [SerializeField] float xBound;
    [SerializeField] float yBound;
    Vector2 CamOrigin;

    [Header("Dialogue indicator")]
    [SerializeField] Transform visualCue;
    [SerializeField] Vector3 visualCueOffset;



    public static Action SnapAction;

    [Header("Camera Flash")]
    [SerializeField] CameraFlash camFlash;

    private void Start()
    {
        CamOrigin = outCamGameObject.transform.position;
    }

    public override void EnterState(GameStateManager gameStateManager)
    {
        ScoreManager.Instance.EnableLevelUI();
        Cursor.visible = false;
        CameraReticle.gameObject.SetActive(true);
        gSm = gameStateManager;
        gSm.NSm = null;
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

        if (!camMode && !EventSystem.current.IsPointerOverGameObject())
        {
            CameraPan();
            WhenMouseIsMoving();
            SnapSystem();
        }

        //Position visual cue
        PositionVisualCue();

        CameraReticle.gameObject.SetActive(!EventSystem.current.IsPointerOverGameObject());
        Cursor.visible = EventSystem.current.IsPointerOverGameObject();
    }
    public override void ExitState(GameStateManager gameStateManager)
    {
        CameraReticle.gameObject.SetActive(false);
    }

   
    public void RayToCollider()
    {
        taggedGameObject = new GameObject[contact.Length];

        for(int i=0; i<contact.Length; i++){
            taggedGameObject[i] = contact[i].collider.gameObject;
        }
    }

    void PositionVisualCue()
    {
        if (closestGameObject && !camMode)
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
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CameraReticle.position = mousePosition;
    }


    void SnapSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (AudioManager.instance != null){
                AudioManager.instance.PlaySFX(AudioManager.instance.camSnap);
            }

            camFlash.FlashCamera();
            if (taggedGameObject.Length > 0)
            {
                camMode = true;
                SnapAction?.Invoke();
                ScoreManager.Instance.DisableLevelUI();
                LeanTween.delayedCall(0.5f,ZoomToTarget);
            }
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
        gSm.NSm = nSm;
        LeanTween.move(CameraReticle.gameObject,
            closestGameObject.position + zoomCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset,
            0.5f).setOnComplete(()=>gSm.ChangeState(gSm.dialogueStat));
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
