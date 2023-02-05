using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(SphereCollider))]
public class LightEmitter : MonoBehaviour
{
    [SerializeField] private float _minRadius;
    [SerializeField] private float _maxRadius;
    [SerializeField] private float _lightRadius;
    [SerializeField] private float _lightIntensityModifier;

    private List<GameObject> _litObjects;

    private Light _light;
    private SphereCollider _triggerVolume;

    private void Awake()
    {
        _litObjects = new List<GameObject>();
        _light = GetComponent<Light>();
        _light.type = LightType.Point;
        _triggerVolume = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        UpdateLightRadius();
    }

    private void Update()
    {
        UpdateLightRadius();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckForLineOfSight(other)) // early exit if not in LOS
        {
            return;
        }
        AddLitObject(other.gameObject); // add objects in that enter trigger volume and are in LOS
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_litObjects.Contains(other.gameObject)) // early exit if unlit object left trigger volume
        {
            return;
        }
        RemoveLitObject(other.gameObject); // remove objects that exit trigger volume
    }

    private void OnTriggerStay(Collider other)
    {
        if (_litObjects.Contains(other.gameObject)) // check if lit objects inside trigger volume goes out of LOS
        {
            if (!CheckForLineOfSight(other))
            {
                RemoveLitObject(other.gameObject);
            }
            return;
        }
        if (CheckForLineOfSight(other)) // check for unlit objects inside trigger volume coming into LOS
        {
            AddLitObject(other.gameObject);
        }
    }

    private bool CheckForLineOfSight(Collider target)
    {

        Vector3 direction = target.ClosestPoint(this.transform.position) - this.transform.position;
        direction = direction.normalized;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
        {
            Debug.LogWarning("Light LOS check Raycast hit nothing");
            // raycast hit nothing, rare but it can happen might be cause collision layers are not properly set up
            return false;
        }
        if (hit.transform.gameObject == target.gameObject)
        {
            // target is LOS
            return true;
        }
        // target not LOS (likely shaded by other object)
        return false;
    }

    private void AddLitObject(GameObject gameObject)
    {
        if ((bool)gameObject.GetComponent<LightElement>()?.EnterLight()) // this can fail if the shadow player exits a wall in light, that causes buggy behavior so it just skips it
        {
            _litObjects.Add(gameObject);
        }
    }

    private void RemoveLitObject(GameObject gameObject)
    {
        gameObject.GetComponent<LightElement>()?.ExitLight();
        _litObjects.Remove(gameObject);
    }

    public void UpdateLightRadius(float radius) // thinking this would be called from light character controller
    {
        _lightRadius = Mathf.Clamp(radius, _minRadius, _maxRadius);
        _light.range = _lightRadius;
        _triggerVolume.radius = _lightRadius;
        _light.intensity = _lightRadius * _lightIntensityModifier;
    }

    private void UpdateLightRadius()
    {
        _light.range = _lightRadius;
        _triggerVolume.radius = _lightRadius;
        _light.intensity = _lightRadius * _lightIntensityModifier;
    }
}