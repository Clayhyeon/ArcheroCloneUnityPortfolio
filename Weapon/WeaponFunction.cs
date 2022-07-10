using UnityEngine;


public abstract class WeaponFunction : ScriptableObject
{
    public abstract GameObject Equip(WeaponConfig config, GameObject model, CharacterParts parts);
    public abstract void Attack(Transform owner, Transform spawn);

}
