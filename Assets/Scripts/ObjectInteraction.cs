using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectInteraction : MonoBehaviour
{
    private Touch touch, touch_b;
    private Quaternion rotationY;
    private float rotateSpeedModifier = 0.1f;
    private double diag, zoomS, previousZ=0;
    private Vector3 initZ;
    private MeshRenderer lastMesh;

    public Text DebugT;
    public InitPart sp;
    public GiveINFO giveI;
    public Camera cam;
    // Update is called once per frame

    private double distance(Vector2 p1, Vector2 p2)
    {
        float d1 = Math.Abs(p1.x - p2.x);
        float d2 = Math.Abs(p1.y - p2.y);
        return Math.Sqrt((d1 * d1) + (d2 * d2));
    }
    private void Start()
    {
        //Screen.width()
        diag = distance(new Vector2(Screen.width, 0), new Vector2(0, Screen.height));
    }
    void Update()
    {
        if (Input.touchCount == 1){
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(
                 touch.deltaPosition.y * rotateSpeedModifier,
                -touch.deltaPosition.x * rotateSpeedModifier,
                0f);
                Quaternion result = rotationY * transform.rotation;
                transform.rotation = result;

            }
            if (touch.phase == TouchPhase.Stationary)
            {
                
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (lastMesh != null)
                    sp.setHide(lastMesh);
                if (Physics.Raycast(ray, out hit))
                {
                    lastMesh = hit.transform.GetComponent<MeshRenderer>();
                    sp.setVisible(lastMesh);
                    switch (hit.transform.name)
                    {
                        case "mask_048_0":
                            giveI.PlayImg();
                            break;

                        case "mask_048_1":
                            giveI.PlayVid();
                            break;

                        case "mask_048_2":
                            giveI.showText("un machin");
                            break;
                        case "mask_048_3":
                            giveI.PlayImg();
                            break;
                        case "mask_048_4":
                            giveI.showText("un autre machin");
                            break;
                        case "mask_048_5":
                            giveI.PlayImg();
                            break;
                        case "mask_048_6":
                            giveI.PlayVid
                             
                                
                               
                               
                                
                                ();
                            break;
                    }

                }
                else
                {
                    giveI.StopAll();
                    Debug.Log( "Sélectionnez une zone");
                }
            }
        }
        if (Input.touchCount == 2)
        {
            touch = Input.GetTouch(0);
            touch_b = Input.GetTouch(1);

            Vector2 p1 = Input.GetTouch(0).position;
            Vector2 p2 = Input.GetTouch(1).position;

            double zoom = distance(p1, p2) / diag;
            if (touch.phase == TouchPhase.Began && touch_b.phase == TouchPhase.Began)
            {
                previousZ = zoom;
                initZ.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            if (touch.phase == TouchPhase.Moved && touch_b.phase == TouchPhase.Moved)
            {
                DebugT.text = (initZ.x + (float)zoom).ToString() ;
                
                
                if (zoom - previousZ > 0f)
                    transform.localScale = new Vector3 (initZ.x + (float)zoom, initZ.y + (float)zoom, initZ.z + (float)zoom);

                if (zoom - previousZ < 0f)
                    transform.localScale = new Vector3 (initZ.x - (float)zoom, initZ.y - (float)zoom, initZ.z - (float)zoom);
            }
            if (touch.phase == TouchPhase.Ended&& touch_b.phase == TouchPhase.Ended)
            {
                initZ.Set(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
