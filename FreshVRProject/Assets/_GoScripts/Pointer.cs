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
    //public TMPro.TextMeshProUGUI debug;


    private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;
    private static Vector3 hitVector;

    private WeaponActions wpnActions;
    public GameObject Weapon;
    public GameObject Teleporter;
    public GameObject GrabHand;
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
        if (touch.x < 0.5f && (touch.y < 0))
        {
            wpnActions.ChangeIndex(-1);
            wpnActions.ShowCurrentWeapon();
        }
        else
        if (touch.x > 0.5f && (touch.y < 0))
        {

            wpnActions.ChangeIndex(1);
            wpnActions.ShowCurrentWeapon();


        }



    }

    private void ProcessTouchpadUp()
    {

    }

    
    private void ProcessTriggerPulled()
    {
        /*
        if (!m_CurrentObject)
            return;
        */
        wpnActions.PlaySound(wpnActions.WeaponSounds[wpnActions.CurrentWeaponIndex]);

        if (wpnActions.CurrentWeaponIndex == 0)
        {
            m_LineRenderer.enabled = false;
            VRTeleporter tp = Teleporter.GetComponent<VRTeleporter>();
            tp.ToggleDisplay(true);

        }
        if(wpnActions.CurrentWeaponIndex == 1)
        {
            m_LineRenderer.enabled = true;


            GameObject hitObject = UpdatePointerStatus();
            if (hitObject.CompareTag("Door"))
            {

                Animator DoorAnim = hitObject.GetComponent<Animator>();
                bool isClosed = DoorAnim.GetBool("isClosed");
                isClosed = !isClosed;
                DoorAnim.SetBool("isClosed", isClosed);
            }
        }
        else
        if (wpnActions.CurrentWeaponIndex == 2)
        {

            m_LineRenderer.enabled = true;

            m_LineRenderer.startColor = Color.red;
            m_LineRenderer.endColor = Color.clear;
            GameObject hitObject = UpdatePointerStatus();
            if (hitObject.CompareTag("ScorpionEnemy"))
            {
                hitObject.GetComponent<EnemyTeleport>().hit = true;
            }
        }else
        if (wpnActions.CurrentWeaponIndex == 3)
        {

            m_LineRenderer.enabled = true;

            m_LineRenderer.startColor = Color.red;
            m_LineRenderer.endColor = Color.clear;
            GameObject hitObject = UpdatePointerStatus();
            if (hitObject.CompareTag("ScorpionEnemy"))
            {
                hitObject.GetComponent<EnemyTeleport>().hit = true;
            }
        }else
        if (wpnActions.CurrentWeaponIndex == 4)
        {
            m_LineRenderer.enabled = true;

            m_LineRenderer.startColor = Color.blue;
            m_LineRenderer.endColor = Color.clear;
            GameObject hitObject = UpdatePointerStatus();
            if (hitObject.CompareTag("Collectable"))
            {
                //Animator GrabAnim = GrabHand.GetComponent<Animator>();
                //GrabAnim.SetInteger("Pose", 1);
                wpnActions.Weapons[wpnActions.CurrentWeaponIndex].transform.Rotate(Vector3.forward, 180.0f);
                hitObject.transform.SetParent(Weapon.transform);
                

                //hitObject.GetComponent<Rigidbody>().AddForce(hitObject.transform.forward * 10.0f, ForceMode.Impulse);
            }



        }




        /*
        if (hitObject.CompareTag("Door"))
        {
            
            Animator DoorAnim = hitObject.GetComponent<Animator>();
            bool isClosed = DoorAnim.GetBool("isClosed");
            isClosed = !isClosed;
            DoorAnim.SetBool("isClosed", isClosed);
        }

        */

    }
    private void ProcessTriggerRelease()
    {
        if (wpnActions.CurrentWeaponIndex == 0)
        {
            m_LineRenderer.enabled = false;
            VRTeleporter tp = Teleporter.GetComponent<VRTeleporter>();

            tp.Teleport();
            tp.ToggleDisplay(false);

        }else
        if (wpnActions.CurrentWeaponIndex == 1)
        {

        }else
        if(wpnActions.CurrentWeaponIndex == 2)
        {

        }else
        if(wpnActions.CurrentWeaponIndex == 3)
        {

        }else
        if(wpnActions.CurrentWeaponIndex == 4)
        {
            GameObject hitObject = UpdatePointerStatus();
            if (hitObject.CompareTag("Collectable"))
            {
                //Animator GrabAnim = GrabHand.GetComponent<Animator>();
                //GrabAnim.SetInteger("Pose", 1);
                wpnActions.Weapons[wpnActions.CurrentWeaponIndex].transform.Rotate(Vector3.forward, 180.0f);
                hitObject.transform.SetParent(null);


                //hitObject.GetComponent<Rigidbody>().AddForce(hitObject.transform.forward * 10.0f, ForceMode.Impulse);
            }
        }
       
        if (!m_CurrentObject)
            return;
        
       // m_LineRenderer.enabled = false;
        //m_LineRenderer.startColor = Color.white;
       // m_LineRenderer.endColor = Color.clear;
    }

}
