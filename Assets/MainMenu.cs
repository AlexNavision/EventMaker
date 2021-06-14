using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventMakersLibrary;
using System.Globalization;

public class MainMenu : EventMakerLibrary
{
    [SerializeField]
    GameObject RecomendationGroupObject,EventObject;
    [SerializeField]
    RectTransform mainCanvas;
    //[SerializeField]
    void Start()
    {
        instance = this;
        UpdateWindow();
    }
    public void UpdateWindow()
    {
        GetRecomendation(Manager.instance.GetUserID); //������ �������
        //GetRecomendation(Manager.instance.GetUserID);
        //GetEventOnFilters();
    }
    new public static MainMenu instance { get; private set; }
    public void CreateRecomendationGroup(RecomendationList recomendationList)
    {
        foreach (Group groups in recomendationList.groups)
        {
            GameObject recomendationGroupObject = Instantiate(RecomendationGroupObject, mainCanvas);
            recomendationGroupObject.GetComponentInChildren<Text>().text = groups.title;
            RectTransform recomendationGroupObjectTransform = recomendationGroupObject.GetComponentInChildren<HorizontalLayoutGroup>().gameObject.GetComponent<RectTransform>();
            foreach(Events events in groups.items)
            {
                Instantiate(EventObject, recomendationGroupObjectTransform).GetComponent<Event>().initialization(events);
            }
        }
    }


    [SerializeField]
    GameObject EventListprefab, EventListDatePrefab;
    [SerializeField]
    RectTransform canvasForEventList;
    DateTime now = DateTime.MinValue.Date;
    public void GetEventOnFilters()
    {
        List<string> filters = new List<string>(new string[] { ">=occurrences.date_to", "spheres.id" });
        List<string> filtersID = new List<string>(new string[] { DateTime.Today.ToString(), "78299" });
        GETEventFilterArray(filters, filtersID);
    }
    public void CreateEventOnFilter(EventList eventList)
    {
        eventList.items.Sort(CompareDate);
        foreach (Item events in eventList.items)
        {
            if (Convert.ToDateTime(events.date).Date != now)
            {
                now = Convert.ToDateTime(events.date).Date;
                if (Convert.ToDateTime(events.date).Date == DateTime.Today)
                    Instantiate(EventListDatePrefab, canvasForEventList).GetComponent<Text>().text = "  �������" + now.ToString(", dddd", CultureInfo.GetCultureInfo("ru-ru"));
                else if (Convert.ToDateTime(events.date).Date == DateTime.Today.AddDays(1))
                    Instantiate(EventListDatePrefab, canvasForEventList).GetComponent<Text>().text = "  ������" + now.ToString(", dddd", CultureInfo.GetCultureInfo("ru-ru"));
                else
                    Instantiate(EventListDatePrefab, canvasForEventList).GetComponent<Text>().text = now.ToString("  dddd, d ", CultureInfo.GetCultureInfo("ru-ru")) + GetstringMonth(now.Month); //now.Date.DayOfWeek + " " + now.Date.Day + " " + GetstringMonth(now.Date.Month);
            }
            Instantiate(EventListprefab, canvasForEventList).GetComponent<Event>().initializationLIST(events);
        }
    }
    private int CompareDate(Item first, Item second)
    {
        return first.date.CompareTo(second.date);
    }
    string GetstringMonth(int i)
    {
        switch (i)
        {
            case 1: return "������";
            case 2: return "�������";
            case 3: return "�����";
            case 4: return "������";
            case 5: return "���";
            case 6: return "����";
            case 7: return "����";
            case 8: return "�������";
            case 9: return "��������";
            case 10: return "�������";
            case 11: return "������";
            case 12: return "�������";
            default: return "������ � ��������";
        }
    }
}
