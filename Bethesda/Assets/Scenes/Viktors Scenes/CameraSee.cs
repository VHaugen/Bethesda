using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSee : MonoBehaviour
{

    private Transform obs;

    private void LateUpdate()
    {
        ViewObs();
    }
    void ViewObs()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 6.0f))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                obs = hit.transform;
                obs.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                if (Vector3.Distance(obs.position, transform.position) >= 3f && Vector3.Distance(transform.position, obs.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime);
                }

            }
            else
            {
                obs.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, obs.position) < 4.5f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime);
                }
            }
        }
    }
}
