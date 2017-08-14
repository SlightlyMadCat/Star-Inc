using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchRecognize : MonoBehaviour {

    public CloneCenter cloneCenter;
    public UIManager uiManager;
    public Text testTap;                //дебаг кол-ва нажатий
    List<Vector3> tapPos = new List<Vector3>();         //список точек нажатия - чтобы не дублировалось при одиночном тапе
    //public float spawnDelay;        //промежуток между спауном - при продолжительном нажатии
    //float curDelay = 0;
    //bool canSpawn = false;
    //float numTaps = 0;
    public float addClonePerSec;
    //для распознавания нажатий на здания
    List<Vector3> startPos = new List<Vector3>();           //точка начала
    //public GameObject marker;

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
                    for (int i = 0; i < Input.touches.Length; i++)
                    {
                        if (IsPointerOverUIObject())
                            return;

                        if(Input.touches[i].phase == TouchPhase.Began)
                        {
                            RaycastHit hitInfo;
                            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo, Mathf.Infinity);

                            if (hitInfo.transform.tag == "Person" || hitInfo.transform.tag == "Person2" || hitInfo.transform.tag == "Person3" || hitInfo.transform.tag == "Person4" || hitInfo.transform.tag == "Person5")
                            {
                                hitInfo.transform.GetComponent<CrowdTouch>().SpawnCloneInParent();
                            }
                        }

                        /*if (Input.touches[i].phase == TouchPhase.Began && tapPos.Contains(Input.GetTouch(i).position) == false)
                        {
                            RaycastHit hitInfo;
                            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo, Mathf.Infinity);

                            if (hitInfo.transform.tag == "Crowd_0")
                            {
                                tapPos.Add(Input.GetTouch(i).position);
                            }
                        }*/

                        /*if (Input.touches[i].phase == TouchPhase.Stationary)
                        {
                            RaycastHit hitInfo;
                            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo, Mathf.Infinity);

                            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))            //ui block
                                return;

                            if (hitInfo.transform.tag == "TapField")
                            {
                                numTaps += addClonePerSec;
                            }
                        }*/

                        /*if (Input.touches[i].phase == TouchPhase.Ended)
                        {
                            if (tapPos.Contains(Input.GetTouch(i).position))
                            {
                                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))            //ui block
                                {
                                    return;
                                }
                                else
                                {
                                    RaycastHit hitInfo;
                                    bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo, Mathf.Infinity);

                                    //print(hitInfo.transform.name);

                                    //marker.transform.position = hitInfo.point;
                                    //cloneCenter.AttractPeople(1, hitInfo.point, hitInfo.transform.gameObject);
                                    tapPos.Remove(Input.GetTouch(i).position);
                                    //numTaps++;
                                }
                            }
                        }*/
                    }

                    /*if(numTaps >= 1)
                    {
                        cloneCenter.AttractPeople(numTaps);
                        numTaps = 0;
                    }*/

            //РАСПОЗНОВАНИЕ НАЖАТИЙ НА ЗДАНИЯ

            foreach (Touch _touch in Input.touches)
            {
                if (IsPointerOverUIObject())
                    return;
                if (_touch.phase == TouchPhase.Began /*|| _touch.phase == TouchPhase.Moved*/)
                {
                    startPos.Add(_touch.position);
                } else if (_touch.phase == TouchPhase.Ended)
                {
                    if (startPos.Contains(_touch.position))
                    {
                        BuildingRecognize(_touch);
                    } else
                    {
                        float dist = Mathf.Infinity;

                        foreach (Vector3 _oldPos in startPos)
                        {
                            if(Vector3.Distance(_touch.position, _oldPos) < dist)
                            {
                                dist = Vector3.Distance(_touch.position, _oldPos);
                            }
                        }

                        if (dist < 35)
                            BuildingRecognize(_touch);
                    }
                }
            }
        }
    }

    void BuildingRecognize(Touch _touch)
    {

        if (Input.touchCount > 2)
            return;

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(_touch.position), out hitInfo, Mathf.Infinity);

        if (hit)
        {
            int id = _touch.fingerId;
            if (IsPointerOverUIObject())                //блок при нажатии на ui
            {
                return;
                // ui touched
            }

            if (hitInfo.transform.tag == "Hostel" && hitInfo.transform.GetComponent<StarHostel>().isBuilded)
            {
                uiManager.ShowHostelsBtn();         //запуск панели ui
            }

            if (hitInfo.transform.tag == "BusStation")         //запуск панели станции
            {
                uiManager.ShowBusStationMenu(1);
            }

            if (hitInfo.transform.name == "ResearchLab")            //окно исследований
            {
                uiManager.ShowResearch(1);
            }

            if (hitInfo.transform.name == "RecordingStudio")            //студия звукозаписи
            {
                uiManager.ShowRecordingScreen(1);
            }
        }

        startPos.Remove(_touch.position);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
