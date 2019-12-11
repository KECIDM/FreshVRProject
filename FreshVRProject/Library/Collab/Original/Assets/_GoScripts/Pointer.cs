 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class Pointer : MonoBehaviour
{
    public float m_Distance = 10.0f;
    public LineRenderer m_LineRenderer = null;
    public LayerMask m_EverythingMask;
    public LayerMask m_InteractableMask;
    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;
    public GameObject reticle;
    public TMPro.TextMeshProUGUI debug;


   private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;
    private static Vector3 hitVector;

    private WeaponActions wpnActions;
    public GameObject Weapon;
    public GameObject Teleporter;
    public Transform ShotOrigin;







    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
        PlayerEvents.OnTouchpadDown += ProcessTouchpadDown;
        PlayerEvents.OnTouchpadUp += ProcessTouchpadUp;
        PlayerEvents.OnTriggerPull += ProcessTriggerPulled;
        PlayerEvents.OnTriggerRelease += ProcessTriggerRelease;
        wpnActions = Weapon.GetComponent<WeaponActions>();
        m_CurrentOrigin = ShotOrigin;
       // Teleporter.SetActive(false);
       
       
    }


    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
        PlayerEvents.OnTouchpadDown -= ProcessTouchpadDown;
        PlayerEvents.OnTouchpadUp -= ProcessTouchpadUp;
        PlayerEvents.OnTriggerPull -= ProcessTriggerPulled;
        PlayerEvents.OnTriggerRelease -= ProcessTriggerRelease;
    }

    private void Start()
    {
        //SetLineColor();
     
    }



    private void Update()
    {

        Vector3 hitPoint = UpdateLine();

        m_CurrentObject = UpdatePointerStatus();

        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_CurrentObject);

        if (hitPoint != null)
        {
            
            reticle.transform.position = hitPoint;


        }
        else
        {
            reticle.transform.position = m_CurrentOrigin.position;
        }
    }




    private Vector3 UpdateLine()
    {
        //Create ray
        RaycastHit hit = CreateRaycast(m_InteractableMask);

        //Default ray end
        Vector3 endPosition = m_CurrentOrigin.position + (m_CurrentOrigin.forward * m_Distance);

        //Check hit
        if (hit.collider != null)
            endPosition = hit.point;

        //Set position
        m_LineRenderer.SetPosition(0, m_CurrentOrigin.position);
        m_LineRenderer.SetPosition(1, endPosition);
 
        return endPosition ;
    }

    private RaycastHit CreateRaycast(LayerMask layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(m_CurrentOrigin.position, m_CurrentOrigin.forward);
        Physics.Raycast(ray, out hit, m_Distance, layer);

        return hit;
    }

    private void SetLineColor()
    {
        if (!m_LineRenderer)
            return;

        Color endColor = Color.white;
        endColor.a = 0.0f;

        m_LineRenderer.endColor = endColor;
    }


    private void UpdateOrigin(OVRInput.Controller controller,GameObject controllerObject)
    {
        //Set origin of pointer
        if (!ShotOrigin)
        {
            m_CurrentOrigin = controllerObject.transform;
        }
        else
        {
            m_CurrentOrigin = ShotOrigin;
        }
        
        //Is the laser visible?
        if (controller == OVRInput.Controller.Touchpad)
        {
            m_LineRenderer.enabled = false;
        }
        else
        {
            m_LineRenderer.enabled = true;
            SetLineColor();
        }
        
    }

    private GameObject UpdatePointerStatus()
    {
        //Create Ray
        RaycastHit hit = CreateRaycast(m_InteractableMask);

        //Check Hit
        if (hit.collider)
            return hit.collider.gameObject;
        //Return
        return null;
    }

    private void ProcessTouchpadDown()
    {
       
        Vector2 touch = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.GetActiveController());


        if ((touch.x > 0.5f) && (touch.y < 0))
        {
            wpnActions.ChangeIndex(1);
        }
        else
        if (touch.x < 0.5f && (touch.y < 0))
        {
            wpnActions.ChangeIndex(-1);
        }

        wpnActions.ShowCurrentWeapon();
        
        if(wpnActions.CurrentWeaponIndex == 0)
        {
            if((touch.y > 0.75))
            {
                //teleport
                //Teleporter.SetActive(true);
                VRTeleporter tp = Teleporter.GetComponent<VRTeleporter>();
                tp.ToggleDisplay(true);
                debug.text = "Teleport?";
               
            }

        }
    }

    private void ProcessTouchpadUp()
    {
        //teleport
        if (wpnActions.CurrentWeaponIndex == 0)
        {
            VRTeleporter tp = Teleporter.GetComponent<VRTeleporter>();
            if (tp.displayActive)
            {
                tp.Teleport();
                tp.ToggleDisplay(false);
               // Teleporter.SetActive(false);
            }
        }
    }

    
    private void ProcessTriggerPulled()
    {
        /*
        if (!m_CurrentObject)
            return;
        */
        wpnActions.PlaySound(wpnActions.WeaponSounds[wpnActions.CurrentWeaponIndex]);       
        
        m_LineRenderer.enabled = true;

        m_LineRenderer.startColor = Color.red;
        m_LineRenderer.endColor = Color.clear;
        GameObject hitObject = UpdatePointerStatus();
        if (hitObject.CompareTag("ScorpionEnemy"))
        {
            hitObject.GetComponent<EnemyTeleport>().hit = true;
        }
        if (hitObject.CompareTag("Door"))
        {
            
            Animator DoorAnim = hitObject.GetComponent<Animator>();
            bool isClosed = DoorAnim.GetBool("isClosed");
            isClosed = !isClosed;
            DoorAnim.SetBool("isClosed", isClosed);
        }



    }
    private void ProcessTriggerRelease()
    {
        /*if (!m_CurrentObject)
            return;
        */
        m_LineRenderer.enabled = false;
        //m_LineRenderer.startColor = Color.white;
       // m_LineRenderer.endColor = Color.clear;
    }

}
