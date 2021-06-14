using System;
using UnityEngine;
using UnityEngine.UI;
using EventMakersLibrary;

public class Event : EventMakerLibrary
{
    [SerializeField]
    UnityEngine.UI.Image image;
    [SerializeField]
    Text title, date, price;
    // Start is called before the first frame update
    public void initialization(Events eventInfo)
    {
        title.text = eventInfo.title;
        if (eventInfo.date_from == eventInfo.date_to)
            date.text = "с " + eventInfo.date_from + " по " + eventInfo.date_to;
        else date.text = "с " + eventInfo.date_from + " по " + eventInfo.date_to;
        if (price)
            if (price.text != "0")
                price.text = eventInfo.free.ToString();
            else price.text = "Бесплатно";
        else
            if (eventInfo.date_from == eventInfo.date_to)
            date.text = "с " + eventInfo.date_from + " по " + eventInfo.date_to; //добавить место проведения
            else date.text = "с " + eventInfo.date_from + " по " + eventInfo.date_to; //добавить место проведения
        GetImage(image, eventInfo.image);
        int i = 10;
        gameObject.AddComponent<Button>().onClick.AddListener(() => Manager.instance.ChangeWindow(i));
    }
    public void initializationLIST(Item events)
    {
        title.text = events.title;
        DateTime time = Convert.ToDateTime(events.date);
        date.text = time.ToString("HH:mm");// time.Hour + ":" + time.Minute;
        GetImage(image, events.image.small.src);
        gameObject.AddComponent<Button>().onClick.AddListener(() => Manager.instance.ChangeWindow(10));
    }
    public void TestClick()
    {
        Manager.instance.ChangeWindow(10);//передаем информацию на карточку события
    }
}
