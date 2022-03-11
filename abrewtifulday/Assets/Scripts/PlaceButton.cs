using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceButton : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject gift;
    public Camera camera;
    public ParticleSystem partSys;

    private bool startedPlacement = false;
    private bool placed = false;
    private int layerMask = 7; //walls
    public void Place()
    {
        startPanel.SetActive(false);
        gift.SetActive(true);
        startedPlacement = true;
    }

    void Update()
    {
        RaycastHit hit;
        Ray origin = camera.ScreenPointToRay(Input.mousePosition);
        // var normal = other.contacts[0].normal;
        LayerMask mask = LayerMask.GetMask("Walls");

        if (!placed)
        {
            if (Physics.Raycast(origin, out hit, mask))
            {
                if (hit.transform.tag == "backwall")
                {
                    gift.transform.position = hit.point;
                    PlaceGift();
                }

            }
        }
    }
    void PlaceGift()
    {
        if (startedPlacement == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                placed = true;
                partSys.Play();
                Invoke("newVoid", 3);
            }
        }
    }

    void newVoid()
    {
        endPanel.SetActive(true);
    }

    public void ReplacePicture()
    {
        placed = false;
        endPanel.SetActive(false);
    }
}
