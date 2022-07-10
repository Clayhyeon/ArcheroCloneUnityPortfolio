using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
   [SerializeField] private Transform owner;
   [SerializeField] private Transform cam;
   [SerializeField] private Slider hp;

   [SerializeField] private float maxHp;
   [SerializeField] private TextMeshProUGUI damageText;
   [SerializeField] private Vector3 damageTextOffset;
   
   private void Start()
   {
      owner = transform.root;
      owner.GetComponent<ControllerBase>().OnStateDead += DeadOwner;
      owner.GetComponent<ControllerBase>().OnStateDamaged += DamagedOwner;

      maxHp = owner.GetComponent<ControllerBase>().Hp;
      
      hp = GetComponent<Slider>();
      
      if (Camera.main is not null)
      {
         cam = Camera.main.transform;
      }
   }

   private void LateUpdate()
   {
      transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
   }

   private void DeadOwner(ControllerBase ownerController)
   {
      gameObject.SetActive(false);
   }

   private void DamagedOwner(ControllerBase ownerController)
   {
      hp.value = Mathf.Lerp(hp.value, ownerController.Hp / maxHp, Time.deltaTime * 50.0f);
      
      
      var damage = GameManager.Pooling.GetObject(transform.root.position, Quaternion.identity, "HitDamage");
      damage.GetComponent<HitDamage>().damage = ownerController.damage;
      
      damage.SetActive(false);
      damage.transform.SetParent(transform.parent);
      damage.transform.position += damageTextOffset;
      damage.SetActive(true);
   }
}
