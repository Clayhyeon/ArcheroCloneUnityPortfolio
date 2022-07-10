using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitDamage : MonoBehaviour
{
    private Transform _cam;
    private TextMeshProUGUI _text;
    private ControllerBase _owner;
    
    public float damage;
    
    private void OnEnable()
    {
        if (Camera.main is not null)
        {
            _cam = Camera.main.transform;
        }

        _owner = transform.root.GetComponent<ControllerBase>();
        _text = GetComponent<TextMeshProUGUI>();
    
        _text.text = damage.ToString();
        StartCoroutine(Stop());
    }
    
    private void LateUpdate()
    {
        if (_owner.dead && gameObject.activeSelf)
        {
            GameManager.Pooling.ReturnObject(transform, "HitDamage");
        }
        transform.LookAt(transform.position + _cam.rotation * Vector3.forward, _cam.rotation * Vector3.up);
    }
    
    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(.25f);
        GameManager.Pooling.ReturnObject(transform, "HitDamage");
    }

}
