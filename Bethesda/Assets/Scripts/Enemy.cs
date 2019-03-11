using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour, IAttackable
{
    [SerializeField]
    Slider healthSlider;

    public float speed;
    public float slowSpeed;
    public float stun;
    public float iFramesDuration = 0.5f;
    public float knockbackMultiplier = 1.0f;
    public Color collideColor = Color.white;
    protected Vector3 knockbackVector;
    public List<Transform> powerUps = new List<Transform>();

    protected float iFramesTimer = -1.0f;

    protected float health;
    public float fireDamagePerSecond = 0.5f;
    public float iceDamagePerSecond = 0.25f;
    public float poisonDamagePerSecond = 0.5f;
    public float poisonStatusDuration = 5.0f;
    int poisonEffectIndex;

    protected float poisonStatus = -1;
    //protected float iceStatus = -1;
    //protected float lightningStatus = -1;
    protected SquashEffect squash;
    protected Flammable flammable;
    protected FrostBite freezable;
    protected Electracuted electracuted;

    virtual protected void Awake()
    {

        flammable = GetComponent<Flammable>();
        freezable = GetComponent<FrostBite>();
        electracuted = GetComponent<Electracuted>();
        squash = GetComponent<SquashEffect>();
        speed = GetComponent<TestEnemy>().maxSpeed;
    }

    virtual protected void Start()
    {
        healthSlider.maxValue = health;
    }

    virtual protected void Update()
    {
        speed = GetComponent<TestEnemy>().maxSpeed = 6;
        if (flammable && flammable.IsBurning())
        {
            SetHealth(health - fireDamagePerSecond * Time.deltaTime);
        }

        if (freezable && freezable.IsFreezing())
        {
            SetHealth(health - iceDamagePerSecond * Time.deltaTime);
            slowSpeed = GetComponent<TestEnemy>().maxSpeed = 1.5f;
        }

        if (poisonStatus > 0)
        {
            SetHealth(health - poisonDamagePerSecond * Time.deltaTime);
            poisonStatus -= Time.deltaTime;
            if (poisonStatus <= 0)
            {
                poisonStatus = -1;
                ParticleEffectsManager.GetEffect("Poison").Stop(poisonEffectIndex);
            }
        }
        if (electracuted && electracuted.IsElc())
        {
            //healthSlider.value = health;
            //healthSlider.gameObject.SetActive(true);
            stun = GetComponent<TestEnemy>().maxSpeed = 0f;
            print("STUNNED");
        }

        if (iFramesTimer > 0)
        {
            iFramesTimer -= Time.deltaTime;
            if (iFramesTimer <= 0)
            {
                iFramesTimer = -1;
                GetComponent<Renderer>().material.SetFloat("_TintAmount", 0);
            }
        }
        slowSpeed = speed;
        stun = speed;
    }

    protected void SetHealth(float newHealth)
    {
        health = newHealth;
        healthSlider.value = health;
        healthSlider.gameObject.SetActive(true);
        if (health <= 0)
        {
            Die();
        }
    }

    virtual public void TakeDamage(DamageParams args)
    {
        if (health > 0)
        {
            if (!squash.inSquash && iFramesTimer <= 0)
            {
                if (args.amount > 0)
                {
                    SetHealth(health - args.amount);
                    if (args.damageType == DamageType.Squash)
                        squash.DoSquash(health > 0);
                    else if (args.damageType == DamageType.Hit)
                    {
                        iFramesTimer = iFramesDuration;
                        knockbackVector = knockbackMultiplier * args.knockback;
                        StartCoroutine(Flasher());
                    }
                    ExtraTakeDamage(args);
                }

                switch (args.element)
                {
                    case Element.None:
                        break;
                    case Element.Fire:
                        if (flammable)
                        {
                            flammable.StartBurning();
                        }
                        break;
                    case Element.Ice:
                        if (freezable)
                        {
                            freezable.FreezeStart();
                        }
                        break;
                    case Element.Poison:
                        poisonStatus = poisonStatusDuration;
						if (poisonStatus > 0)
						{
							poisonEffectIndex = ParticleEffectsManager.GetEffect("Poison").Spawn(GetComponent<MeshRenderer>());
						}
                        break;
                    case Element.Lightning:
                        if (electracuted)
                        {
                            electracuted.ElcStart();
                        }
                        break;
                }

                if (health <= 0)
                {
                    Die();                
                    if (Random.Range(1, 5) == 1)
                    {
                        Instantiate(powerUps[Random.Range(0, powerUps.Count - 1)], transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }

    virtual protected void ExtraTakeDamage(DamageParams args)
    {

    }

    abstract protected void Die();

    IEnumerator Flasher()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_TintColor", collideColor);
            while (iFramesTimer > 0)
            {
                renderer.material.SetFloat("_TintAmount", 0.8f);
                yield return new WaitForSeconds(.06f);
                renderer.material.SetFloat("_TintAmount", 0.0f);
                yield return new WaitForSeconds(.03f);
            }
        }
    }
}
