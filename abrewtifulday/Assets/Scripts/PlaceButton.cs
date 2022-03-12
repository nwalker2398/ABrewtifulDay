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
    public ParticleSystem partSys3;
    public GameObject giftPic;
    public Material newWallPaper1;
    public Material newWallPaper2;
    public Material newWallPaper3;
    public GameObject leftWall;
    public GameObject rightWall1;
    public GameObject rightWall2;
    public GameObject rightWall3;
    public GameObject backWall;

    private bool startedPlacement = false;
    private bool placed = false;
    private int version = 1;
    private Material wallpaperChoice;

    void Start()
    {
        // versions: 1 = howls picture, 2 = cagliostro picture, 3 = wallpaper
        // 4 = plant (fiddle leaf)
        version = 4; // development mode -- need to change
        if (version == 2)
        {
            gift.SetActive(false);
            gift = GameObject.Find("FiddleLeaf");
            gift.SetActive(false);
            gift = GameObject.Find("Paint_02");
            partSys = partSys2;
            gift.SetActive(false);

            giftPic.SetActive(false);
            giftPic = GameObject.Find("Cagliostro");

            startPanel = GameObject.Find("WallpaperStartPanel");
            startPanel.SetActive(false);
            startPanel = GameObject.Find("PlantStartPanel");
            startPanel.SetActive(false);
            startPanel = GameObject.Find("StartPanel");
        }
        else if (version == 1)
        {
            gift = GameObject.Find("Paint_02");
            gift.SetActive(false);
            gift = GameObject.Find("FiddleLeaf");
            gift.SetActive(false);
            gift = GameObject.Find("Paint_01");
            gift.SetActive(false);

            giftPic = GameObject.Find("Cagliostro");
            giftPic.SetActive(false);
            giftPic = GameObject.Find("Howls");

            startPanel = GameObject.Find("WallpaperStartPanel");
            startPanel.SetActive(false);
            startPanel = GameObject.Find("PlantStartPanel");
            startPanel.SetActive(false);
            startPanel = GameObject.Find("StartPanel");
        }
        else if (version == 3)
        {
            gift.SetActive(false);
            gift = GameObject.Find("FiddleLeaf");
            gift.SetActive(false);
            gift = GameObject.Find("Paint_02");
            gift.SetActive(false);

            startPanel.SetActive(false);
            startPanel = GameObject.Find("PlantStartPanel");
            startPanel.SetActive(false);
            startPanel = GameObject.Find("WallpaperStartPanel");
        }
        else if (version == 4)
        {
            gift.SetActive(false);
            gift = GameObject.Find("Paint_02");
            gift.SetActive(false);
            gift = GameObject.Find("FiddleLeaf");
            partSys = partSys3;
            gift.SetActive(false);

            startPanel.SetActive(false);
            startPanel = GameObject.Find("WallpaperStartPanel");
            startPanel.SetActive(false);
            startPanel = GameObject.Find("PlantStartPanel");
            //todo: add partsys for plant
        }
    }
    public void Place()
    {
        if (version == 1 || version == 2 || version == 4)
        {
            startPanel.SetActive(false);
            gift.SetActive(true);
            startedPlacement = true;
        }
    }

    void Update()
    {
        if (version == 3)
        {
            //paint walls
            if (startedPlacement == true)
            {
                Invoke("wallpapervoid", 2); // maybe play an animation here instead
            }
        }
        else if (version == 1 || version == 2)
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
        else if (version == 4)
        {
            RaycastHit hit;
            Ray origin = camera.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("Floor");
            if (!placed)
            {
                if (Physics.Raycast(origin, out hit, mask))
                {
                    if (hit.transform.tag == "floor")
                    {
                        gift.transform.position = hit.point;
                        PlaceGift();
                    }
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
                if (version == 1 || version == 2 || version == 4)
                {
                    partSys.Play();
                    Invoke("newVoid", 3);
                }
            }
        }
    }
    void newVoid()
    {
        endPanel.SetActive(true);
    }
    void wallpapervoid()
    {
        print("gets here");
        rightWall1.GetComponent<MeshRenderer>().material = wallpaperChoice;
        rightWall2.GetComponent<MeshRenderer>().material = wallpaperChoice;
        rightWall3.GetComponent<MeshRenderer>().material = wallpaperChoice;
        leftWall.GetComponent<MeshRenderer>().material = wallpaperChoice;
        backWall.GetComponent<MeshRenderer>().material = wallpaperChoice;
        Invoke("newVoid", 3);
    }

    public void ReplacePicture()
    {
        if (version == 1 || version == 2 || version == 4)
        {
            placed = false;
            endPanel.SetActive(false);
        }
        else if (version == 3)
        {
            startedPlacement = false;
            endPanel.SetActive(false);
            startPanel.SetActive(true);
        }
    }

    public void PickWallpaper1()
    {
        wallpaperChoice = newWallPaper1;
        startPanel.SetActive(false);
        startedPlacement = true;
    }
    public void PickWallpaper2()
    {
        wallpaperChoice = newWallPaper2;
        startPanel.SetActive(false);
        startedPlacement = true;
    }
    public void PickWallpaper3()
    {
        wallpaperChoice = newWallPaper3;
        startPanel.SetActive(false);
        startedPlacement = true;
    }
}
