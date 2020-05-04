
using UdonSharp;
using UnityEngine;
//using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class Hookshot : UdonSharpBehaviour
{
    [UdonSynced] bool used = false;
    [UdonSynced] bool hooked = false;
    [UdonSynced] bool temp = false;
    //public Text uiText;
    public float pullingPower = 50f;
    public float speedLimit = 150f;
    public float distanceRange = 500f;
    public LayerMask targetLayers = -1;

    public Transform holdPointer;
    public Transform rayPoint;
    public Transform hook;
    public Transform pointer;
    public Transform direction;
    public LineRenderer lineRenderer;

    private Rigidbody rb;

    private VRCPlayerApi api;
    private VRC.SDK3.Components.VRCPickup pickup;

    private void Start()
    {
        rb =transform.GetComponent<Rigidbody>();
        pickup = (VRC.SDK3.Components.VRCPickup)transform.GetComponent(typeof(VRC.SDK3.Components.VRCPickup));
        hook.parent = transform.parent;
        hook.name = "Hook_" + hook.GetSiblingIndex();
    }

    public override void OnPickup()
    {
        api = pickup.currentPlayer;
       
        //hook.GetChild(0).gameObject.SetActive(true);
        Networking.SetOwner(api, hook.gameObject);
        hook.position = rayPoint.position;
        used = false;
        hooked = false;
        temp = false;

    }
    public override void OnDrop()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Drop");
        api = null;
    }
    public void Drop()
    {
        used = false;
        hooked = false;
        temp = false;
        HideHook();
    }
    public override void OnPickupUseDown()
    {
        
        //Networking.SetOwner(pickup.currentPlayer, hook.gameObject);
        used = true;
    }

    public override void OnPickupUseUp()
    {
        used = false;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideHook");
    }

    public void HideHook()
    {
        hook.GetChild(0).gameObject.SetActive(false);
        lineRenderer.SetPosition(1, Vector3.zero);
    }
    public void FixedUpdate()
    {
        if (pickup.IsHeld)
        {
            RaycastHit hit;
            //Debug.DrawRay(rayPoint.position,  rayPoint.forward * 10, Color.magenta);

            hooked = Physics.Raycast(rayPoint.position, rayPoint.forward, out hit, distanceRange,targetLayers);
            if (hooked)
            {
                pointer.position = hit.point; 
            }
            else
            {
                pointer.position = holdPointer.position;
            }

            if (used)
            {
                if (hooked || temp)
                {
                    if (!temp)
                    {
                        hook.position = hit.point;
                        hook.GetChild(0).gameObject.SetActive(true);

                        temp = true;
                    }

                    lineRenderer.SetPosition(1, rayPoint.InverseTransformPoint(hook.position));
                    if (api != null && api.isLocal)
                    {
                        direction.LookAt(hook);
                        Vector3 playerVelocity = api.GetVelocity();
                        Vector3 vel = direction.forward * pullingPower * Time.fixedDeltaTime;
                        playerVelocity = playerVelocity + vel;
                        playerVelocity.x = Mathf.Clamp(playerVelocity.x, -speedLimit, speedLimit);
                        playerVelocity.y = Mathf.Clamp(playerVelocity.y, -speedLimit, speedLimit);
                        playerVelocity.z = Mathf.Clamp(playerVelocity.z, -speedLimit, speedLimit);
                        //playerVelocity = Vector3.Min(playerVelocity, vectorMin);
                        //speed = baseSpeed + Vector3.Magnitude(rb.velocity);
                        
                        api.SetVelocity(playerVelocity);
                    }
                }
            }
            else
            {
                
                if (temp)
                {
                    hook.position = rayPoint.position;
                    lineRenderer.SetPosition(1, Vector3.zero);
                    temp = false;
                }
            }
        }
    }
}
