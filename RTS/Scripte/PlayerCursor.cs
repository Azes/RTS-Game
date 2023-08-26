using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCursor : MonoBehaviour
{
    public StadtCentrum sc;
    public Camera _c;
    public BuildSystem buildSystem;



    public MainUI mui;
    public Image selectionRectImage;

    private RectTransform rectTransform;
    public RectTransform canvas;

    
    public List<GameObject> selectedObjects = new List<GameObject>();


    public IBuilding build;
    
    
    bool isDrag, onSelecting;
    Vector3 lsmp = Vector3.zero;
    Vector2 initialMousePos;

    List<GameObject> objectsInCameraView = new List<GameObject>();
    Vector3[] cc = new Vector3[4];
    RaycastHit hit;

    private void Start()
    {
        _c = GetComponent<Camera>();
        buildSystem = GetComponent<BuildSystem>();

        lsmp = Input.mousePosition;

        rectTransform = selectionRectImage.GetComponent<RectTransform>();
        selectionRectImage.gameObject.SetActive(false);

        canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        
        if (MainUI.inUI || MainUI.inWorldUI || buildSystem.onBuild) return;

        bool dsel = Input.GetKey(KeyCode.LeftShift);

        
        
        if (Input.GetMouseButtonDown(1)) singelSelect(dsel, true);
        

        


        if (Input.GetMouseButtonDown(0))
            {
                initialMousePos = Input.mousePosition;
                initialMousePos.x = initialMousePos.x - (Screen.width / 2);
                initialMousePos.y = initialMousePos.y - (Screen.height / 2);

                rectTransform.anchoredPosition = initialMousePos;
                rectTransform.sizeDelta = Vector2.zero;
                lsmp = Input.mousePosition;
            }


            if (isMouseDrag(2))
            {
                if (!selectionRectImage.gameObject.activeInHierarchy)
                    selectionRectImage.gameObject.SetActive(true);

                Vector2 currentMousePos = Input.mousePosition;
                currentMousePos.x = currentMousePos.x - (Screen.width / 2);
                currentMousePos.y = currentMousePos.y - (Screen.height / 2);
                Vector2 size = currentMousePos - initialMousePos;

                // Ensure width and height are positive for proper scaling
                size.x = Mathf.Abs(size.x);
                size.y = Mathf.Abs(size.y);

                rectTransform.sizeDelta = size;

                Vector2 center = (initialMousePos + currentMousePos) * 0.5f;
                rectTransform.anchoredPosition = center;

                if (onSelecting) dsel = false;

                GetObjectsInSelect(dsel);
            }
            else
            {
                if (selectionRectImage.gameObject.activeInHierarchy)
                    selectionRectImage.gameObject.SetActive(false);

                if (Input.GetMouseButtonDown(0)) singelSelect(dsel);
            }
       
    }


    public bool isMouseDrag(float min = 0f)
    {
        bool isMouseDown;
        bool currentMouseDown = Input.GetMouseButton(0);
        Vector3 currentMousePosition = Input.mousePosition;

        bool positionChanged = Vector3.Distance(currentMousePosition, lsmp) >= min;


        if (currentMouseDown)
        {
            isMouseDown = true;
            lsmp = currentMousePosition;

            if (positionChanged) { isDrag = true; }

        }
        else
        {
            isDrag = false;
            isMouseDown = false;
        }

        return isMouseDown && isDrag;
    }

    //new
    bool setTarget;Vector3 hitpos;
    void singelSelect(bool msel = false, bool isleft = false)
    {

        Vector3 ip = Input.mousePosition;

        Ray r = _c.ScreenPointToRay(new Vector3(ip.x, ip.y, _c.nearClipPlane));

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            setTarget = true;
            hitpos = hit.point;

            if (hit.collider.GetComponent<IHuman>())
            {
                onSelecting = true;
               
                GameObject ho = hit.collider.gameObject;

                if (msel)
                {
                    
                    if (!selectedObjects.Contains(ho))
                    {
                      
                        ho.GetComponent<IHuman>().isSelectet = true;
                        selectedObjects.Add(ho);

                        mui.humans.Add(ho.GetComponent<IHuman>());

                    }
                    else
                    {
                        
                        ho.GetComponent<IHuman>().isSelectet = false;
                        selectedObjects.Remove(ho);

                        mui.humans.Remove(ho.GetComponent<IHuman>());
                    }
                }
                else
                {

                    for (int i = 0; i < selectedObjects.Count; i++)
                        selectedObjects[i].GetComponent<IHuman>().isSelectet = false;

                    mui.humans.Clear();
                    selectedObjects.Clear();

                    ho.GetComponent<IHuman>().isSelectet = true;
                    selectedObjects.Add(ho);

                    mui.humans.Add(ho.GetComponent<IHuman>());
                }
            }
            else
            {
                if (msel) return;

                onSelecting = false;

                if (hit.collider.GetComponent<IBuilding>())
                {
                    for (int i = 0; i < selectedObjects.Count; i++)
                        selectedObjects[i].GetComponent<IHuman>().isSelectet = false;
                    mui.humans.Clear();
                    selectedObjects.Clear();

                    build = hit.collider.GetComponent<IBuilding>();
                    mui.build = build;
                }
                else
                {
                    build = null;
                    mui.build = null;
                }


               // if (isleft)
               // {
               //     for (int i = 0; i < selectedObjects.Count; i++)
               //         selectedObjects[i].GetComponent<IHuman>().isSelectet = false;
               //
               //     mui.humans.Clear();
               //     selectedObjects.Clear();
               // }
               // else
               // {
               //     for (int i = 0; i < selectedObjects.Count; i++)
               //     {
               //         selectedObjects[i].GetComponent<IHuman>().setWalkTarget(hit.point, 1f, i, selectedObjects.Count, 2f);
               //     }
               // }

            }
        }
    }


    

    public void GetObjectsInSelect(bool delete = false)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_c);

        objectsInCameraView.Clear();


        foreach (GameObject go in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            Collider collider = go.GetComponent<Collider>();

            if (collider != null && GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            {
                if (!objectsInCameraView.Contains(go)) objectsInCameraView.Add(go);
            }
        }


        for (int i = 0; i < objectsInCameraView.Count; i++)
        {
           
            GameObject co = objectsInCameraView[i];

            Vector3 sp = _c.WorldToScreenPoint(co.transform.position);

            rectTransform.GetWorldCorners(cc);

            if (sp.x >= cc[0].x && sp.x <= cc[2].x &&
                sp.y >= cc[0].y && sp.y <= cc[1].y)
            {
                if (delete)
                {
                    if (selectedObjects.Contains(co))
                    {
                        co.GetComponent<IHuman>().isSelectet = false;
                        selectedObjects.Remove(co);

                        mui.humans.Remove(co.GetComponent<IHuman>());
                    }
                }
                else
                {
                    if (!selectedObjects.Contains(co) && co.GetComponent<IHuman>())
                    {
                        co.GetComponent<IHuman>().isSelectet = true;
                        selectedObjects.Add(co);

                        mui.humans.Add(co.GetComponent<IHuman>());
                    }
                }
            }
            else
            {
                if (delete || onSelecting) continue;

                if (selectedObjects.Contains(co) && co.GetComponent<IHuman>())
                {
                    co.GetComponent<IHuman>().isSelectet = false;
                    selectedObjects.Remove(co);

                    mui.humans.Remove(co.GetComponent<IHuman>());
                }
            }
        }
    }


    //hilfs methode um selections zu removen und deSelecten
    public void removeSelcetion(GameObject g)
    {
        g.GetComponent<IHuman>().isSelectet = false;
        selectedObjects.Remove(g);
    }

    //hilfs methode um alle selection zu löschen
    public void ClearSelection()
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            selectedObjects[i].GetComponent<IHuman>().isSelectet = false;
        }
        selectedObjects.Clear();
    }

    //methode um ein einzelnde einheit überandere scripte auszuwählen
    public void singleSelectObject(GameObject g)
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            selectedObjects[i].GetComponent<IHuman>().isSelectet = false;
        }

        selectedObjects.Clear();
        selectedObjects.Add(g);
        selectedObjects[0].GetComponent<IHuman>().isSelectet = true;
        
    }


    //new
    private static float doubleClickTime = 0.3f; // Zeitintervall für Doppelklick in Sekunden
    private static float lastClickTime = 0f;

    public static bool DoubleClick(int index)
    {
        if (Input.GetMouseButtonDown(index))
        {
            float currentTime = Time.time;
            if (currentTime - lastClickTime < doubleClickTime)
            {
                lastClickTime = 0f; // Setze den Zeitstempel zurück
                return true; // Doppelklick wurde erkannt
            }
            lastClickTime = currentTime;
        }
        return false; // Kein Doppelklick
    }
}
