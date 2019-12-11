using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject Weapon;
    private WeaponActions wpnActions;
    public GameObject Door;
    private Animator DoorAnim;
    private AudioSource audio;
    public AudioClip DoorOpenSnd;
    public AudioClip DoorCloseSnd;
    public bool Closed = true;

    // Start is called before the first frame update
    void Start()
    {
        wpnActions = Weapon.GetComponent<WeaponActions>();
        DoorAnim = Door.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Closed = DoorAnim.GetBool("isClosed");
        var renderer = GetComponent<Renderer>();
        if(Closed == true)
        {
            renderer.material.SetColor("_Emissive", Color.red);
        }
        else
        {
            renderer.material.SetColor("_Emissive", Color.green);
        }

        
    }
    public void ToggleDoor()
    {
        if (wpnActions.CurrentWeaponIndex == 1)
        {
            Closed = DoorAnim.GetBool("isClosed");
            Closed = !(Closed);
            DoorAnim.SetBool("isClosed", Closed);
        }
    }

    public void PlayOpenSnd()
    {
        audio.PlayOneShot(DoorOpenSnd);
    }

    public void PlayCloseSnd()
    {
        audio.PlayOneShot(DoorCloseSnd);
    }
}
