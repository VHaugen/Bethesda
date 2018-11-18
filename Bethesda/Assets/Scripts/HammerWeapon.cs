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
            anim.Play("PlaceholderWeaponAttack");
			afterImages.Show();

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("ROADO ROLLA DA");
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

    public void PlaySound(Object audioClip)
    {
        audioSource.PlayOneShot((AudioClip)audioClip);
    }

    public void ScreenShake()
    {
        CameraEffects.Get.Shake(0.25f, 0.4f);
    }
}
