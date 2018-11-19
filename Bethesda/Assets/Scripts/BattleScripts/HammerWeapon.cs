<<<<<<< HEAD:Bethesda/Assets/Scripts/BattleScripts/HammerWeapon.cs
﻿using System.Collections;
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
=======
﻿using System.Collections;
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
	AfterImages afterImages;
	Transform impact;
	Vector3 impactOffset;
    public GameObject playerObject;

    void Awake()
    {
        hitbox = transform.Find("Head").GetComponent<Collider>();

		impact = transform.Find("Impact");
		impactOffset = impact.localPosition;

        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        overHeadHitBox = transform.Find("AOE").GetComponent<Collider>();
		afterImages = GetComponentInChildren<AfterImages>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderWeaponAttack"))
        {
            print("Attacku!");
            anim.SetTrigger("AttackTransitionBasic");
			afterImages.Show();
		}
        if (Input.GetKeyDown(KeyCode.E) && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlaceholderOverheadAttack") || Input.GetButtonDown("B Button") && !anim.GetCurrentAnimatorStateInfo(1).IsName("PlaceholderOverheadAttack"))
        {
            print("ROADO ROLLA DA");
            anim.SetTrigger("AttackTransitionOverhead");
        }

    }


	public void ShowImpact(int show)
	{
		if (show == 0)
		{
			impact.gameObject.SetActive(false);
			impact.parent = transform;
			impact.localPosition = impactOffset;
			impact.localRotation = Quaternion.identity;
		}
		else
		{
			impact.gameObject.SetActive(true);
			impact.parent = null;
			impact.rotation = Quaternion.Euler(90, 90, Random.Range(0f, 360f));;
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
>>>>>>> 1980b61f15e882dd088fc8751a6ffada80a7d9be:Bethesda/Assets/Scripts/HammerWeapon.cs
