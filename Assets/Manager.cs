using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private string userID;
    public static Manager instance { get; private set; } //������ �� ������
    public string GetUserID
    {
        get { return userID; }
    }
    //�������, ��� ID ������������ �� ��� �����, ������� ��� �� ���������� ��������������� UUID
    private void Awake()    
    {
        instance = this;
        ///userID = SystemInfo.deviceUniqueIdentifier;
        Guid myuuid = new Guid("7fd45321-9b70-4641-8bba-573e7497374a");
        userID = myuuid.ToString();
        print(userID);
    }

    [SerializeField]
    GameObject[] Windows;// MainMenu, Ticket, Map, Message, Profile,5 MainMenu_mainCanvas,6 MainMenu_category,7 MainMenu_search,8 ticket_card,9 settings,10 eventCard
    private int OpenTapBarWindowId;
    [SerializeField]
    GameObject tapbar;
    [SerializeField]
    Image[] tapbarImage;
    [SerializeField]
    Sprite[] tapbarImageoff,tapbarImageon;
    public void TapBarChange(int i)
    {
        Windows[OpenTapBarWindowId].SetActive(false);
        tapbarImage[OpenTapBarWindowId].sprite = tapbarImageoff[OpenTapBarWindowId];
        Windows[i].SetActive(true);
        tapbarImage[i].sprite = tapbarImageon[i];
        OpenTapBarWindowId = i;
    }
    Stack<int> OpenWindows = new Stack<int>();
    public void ChangeWindow(int i) //��������� ����� ����
    {
        switch(i)
        {
            case 0: Windows[0].SetActive(true);tapbar.SetActive(true);OpenTapBarWindowId = 0;TapBarChange(0); break;
            //MainMenu_category (������� ���� -> ���������)
            case 6: Windows[i-1].SetActive(false); Windows[i].SetActive(true); OpenWindows.Push(i);SetWindowTopNavigator("��� �����������"); break;
            //MainMenu_category(��������� -> ����� ���������)    //����� � ������� MainMenu.instance.GetEventOnFilters() �� ������ ���������� ��������� ������������� �������
            case 7: Windows[i - 1].SetActive(false); Windows[i].SetActive(true); OpenWindows.Push(i); MainMenu.instance.GetEventOnFilters(); SetWindowTopNavigator("��������"); break;
            //�������� ������
            case 8: Windows[i].SetActive(true);Windows[1].SetActive(false); OpenWindows.Push(i); SetWindowTopNavigator("�����");break;
            //���������
            case 9: Windows[i].SetActive(true); OpenWindows.Push(i); SetWindowTopNavigator("���������"); break;
            //��������� �������� �����������
            case 10: Windows[i].SetActive(true); OpenWindows.Push(i); SetWindowTopNavigator("�����������"); break;
            //��������� ������ ����������
            case 11: Windows[5].SetActive(false); Windows[i].SetActive(true); OpenWindows.Push(i); SetWindowTopNavigator("������ QR ����");break;
        }
    }
    public void Back()
    {
        if (OpenWindows.Count == 0) return;
        int i = OpenWindows.Pop();
        print(i);
        switch(i)
        {
            case 11: Windows[i].SetActive(false); Windows[5].SetActive(true); DeleteWindowTopNavigator(); break;
            case 10: Windows[i].SetActive(false); DeleteWindowTopNavigator();ChangeWindow(OpenWindows.Peek()); backfromEventCard(); break;
            case 9: Windows[i].SetActive(false); DeleteWindowTopNavigator(); break;
            case 8: Windows[i].SetActive(false); Windows[1].SetActive(true); DeleteWindowTopNavigator(); break;
            case 7: Windows[i - 1].SetActive(true); Windows[i].SetActive(false); SetWindowTopNavigator("��� �����������"); break; 
            case 6: Windows[i - 1].SetActive(true); Windows[i].SetActive(false); DeleteWindowTopNavigator(); break;
        }
    }
    void backfromEventCard()
    {
        for (int i = 0; i < OpenWindows.Count-1; i++)
            OpenWindows.Push(OpenWindows.Pop());
        OpenWindows.Pop();
    }
    [SerializeField]
    GameObject WindowTopNavigator;
    [SerializeField]
    Text WindowTitleName;
    public void SetWindowTopNavigator(string text) //� �������� ������� ��������� ����� ���������� �������� id ������, ������� ��� �����. (�������� ���������, ����������, ��������� ���������� � ������)
    {
        WindowTopNavigator.SetActive(true); WindowTitleName.text = text;
    }
    public void DeleteWindowTopNavigator()
    {
        WindowTopNavigator.SetActive(false);
    }

    [SerializeField]
    GameObject[] map;private int MapTabsActive = 0; 
    public void PresentationMap()
    {
        map[MapTabsActive++].SetActive(false);
        if (MapTabsActive == 3)
            MapTabsActive = 0;
        map[MapTabsActive].SetActive(true);
    }
    [SerializeField]
    Text time;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            if (current == null)
            {
                current = StartCoroutine(GetAqiInfo());
            }
        time.text = DateTime.Now.ToString("HH:mm");
    }
    private Coroutine current;
    IEnumerator GetAqiInfo()
    {
        Back();
        yield return new WaitForSeconds(1f);
        current = null;
    }
}
