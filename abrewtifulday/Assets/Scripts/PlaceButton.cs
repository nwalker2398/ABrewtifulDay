using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceButton : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject gift;
    public Camera camera;
    public ParticleSystem partSys;
    public ParticleSystem partSys2;
    public GameObject giftPic;

    private bool startedPlacement = false;
    private bool placed = false;
    private int version = 1;

    void Start()
    {
        // version = 2; // development mode -- need to change
        if (version == 2)
        {
            gift.SetActive(false);
            gift = GameObject.Find("Paint_02");
            partSys = partSys2;
            gift.SetActive(false);

            giftPic.SetActive(false);
            giftPic = GameObject.Find("Cagliostro");
        }
        else
        {
            gift = GameObject.Find("Paint_02");
            gift.SetActive(false);
            gift = GameObject.Find("Paint_01");
            gift.SetActive(false);

            giftPic = GameObject.Find("Cagliostro");
            giftPic.SetActive(false);
            giftPic = GameObject.Find("Howls");
        }
    }
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
