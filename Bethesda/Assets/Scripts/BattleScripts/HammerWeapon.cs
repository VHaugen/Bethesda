using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Fire,
    Ice,
    Lightning,
    Poison,
    None
}

[RequireComponent(typeof(AudioSource))]
public class HammerWeapon : MonoBehaviour
{

    Collider hitbox;
    AudioSource audioSource;
    Animator anim;
    Collider overHeadHitBox;
    public GameObject playerObject;
    
    void Awake()
    {
        hitbox = transform.Find("Head").GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        overHeadHitBox = transform.Find("AOE").GetComponent<Collider>();
        
       
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderWeaponAttack"))
        {
            print("Attacku!");
            anim.SetTrigger("AttackTransitionBasic");
        }
        if (Input.GetKeyDown(KeyCode.E) && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderOverheadAttack") || Input.GetButtonDown("B Button") && !anim.GetCurrentAnimatorStateInfo(1).IsName("PlaceholderOverheadAttack"))
        {
            print("ROADO ROLLA DA");
            anim.SetTrigger("AttackTransitionOverhead");
        }

    }



    public void EnableHitbox(int enable)
    {
        hitbox.enabled = enable == 0 ? false : true;
    }

    public void EnableAoeHitbox(int enable)
    {
        overHeadHitBox.enabled = enable == 0 ? false : true;
    }

    public void EnableSlowDown(int enable)
    {
        playerObject.GetComponent<PlayerMovement>().maxSpeed = 1;
    }

    public void EnableNormalSpeed(int enable)
    {
        playerObject.GetComponent<PlayerMovement>().maxSpeed = 10;
    }

    public void PlaySound(Object audioClip)
    {
        audioSource.PlayOneShot((AudioClip)audioClip);
    }

    public void ScreenShake()
    {
        CameraEffects.Get.Shake(0.25f, 0.4f);
    }
}
