using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCursor : MonoBehaviour
{

    public Image selectionRectImage;
    
    private RectTransform rectTransform;
    public RectTransform canvas;
    private Camera _c;
    public List<GameObject> selectedObjects = new List<GameObject>();
    bool isDrag, onSelecting;
    Vector3 lsmp = Vector3.zero;
    Vector2 initialMousePos;

    List<GameObject> objectsInCameraView = new List<GameObject>();
    Vector3[] cc = new Vector3[4];
    RaycastHit hit;

    private void Start()
    {
        _c = Camera.main;
        lsmp = Input.mousePosition;

        rectTransform = selectionRectImage.GetComponent<RectTransform>();
        selectionRectImage.gameObject.SetActive(false);

        canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {

        bool dsel = Input.GetKey(KeyCode.LeftShift);


        if (Input.GetMouseButtonDown(1)) singelSelect(dsel);

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

            if(onSelecting) dsel = false;

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


    void singelSelect(bool msel = false)
    {
        
        Vector3 ip = Input.mousePosition;

        Ray r = _c.ScreenPointToRay(new Vector3(ip.x, ip.y, _c.nearClipPlane));

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            if (hit.collider.GetComponent<IHuman>())
            {
                onSelecting = true;

                if (msel)
                {
                    if (!selectedObjects.Contains(hit.collider.gameObject))
                    {
                        hit.collider.GetComponent<IHuman>().isSelectet = true;
                        selectedObjects.Add(hit.collider.gameObject);
                        
                    }
                    else
                    {
                        hit.collider.GetComponent<IHuman>().isSelectet = false;
                        selectedObjects.Remove(hit.collider.gameObject);
                    }
                }
                else
                {
                    for (int i = 0; i < selectedObjects.Count; i++)
                        selectedObjects[i].GetComponent<IHuman>().isSelectet = false;

                    selectedObjects.Clear();

                    hit.collider.GetComponent<IHuman>().isSelectet = true;
                    selectedObjects.Add(hit.collider.gameObject);
                   
                }
            }
            else
            {
                if (msel) return;

                for (int i = 0; i < selectedObjects.Count; i++)
                    selectedObjects[i].GetComponent<IHuman>().isSelectet = false;

                selectedObjects.Clear();

                onSelecting = false;
            }
        }
    }

    
    public void GetObjectsInSelect(bool delete = false)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_c);
        
        objectsInCameraView.Clear();


        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            Collider collider = go.GetComponent<Collider>();

            if (collider != null && GeometryUtility.TestPlanesAABB(planes, collider.bounds))
            {
               if(!objectsInCameraView.Contains(go)) objectsInCameraView.Add(go);
            }
        }


        for (int i = 0;i < objectsInCameraView.Count; i++)
        {
            Vector3 sp = _c.WorldToScreenPoint(objectsInCameraView[i].transform.position);

            rectTransform.GetWorldCorners(cc);

            if (sp.x >= cc[0].x && sp.x <= cc[2].x &&
                sp.y >= cc[0].y && sp.y <= cc[1].y)
            {
                if (delete)
                {
                    if (selectedObjects.Contains(objectsInCameraView[i]))
                    {
                        objectsInCameraView[i].GetComponent<IHuman>().isSelectet = false;
                        selectedObjects.Remove(objectsInCameraView[i]);
                    }
                   
                }
                else
                {
                    if (!selectedObjects.Contains(objectsInCameraView[i]) && objectsInCameraView[i].GetComponent<IHuman>())
                    {
                        objectsInCameraView[i].GetComponent<IHuman>().isSelectet = true;
                        selectedObjects.Add(objectsInCameraView[i]);
                    }
                }
            }
            else
            {
                if (delete || onSelecting) continue;

                if (selectedObjects.Contains(objectsInCameraView[i]) && objectsInCameraView[i].GetComponent<IHuman>())
                {
                    objectsInCameraView[i].GetComponent<IHuman>().isSelectet = false;
                    selectedObjects.Remove(objectsInCameraView[i]);
                }
            }
        }
    }
   
}
