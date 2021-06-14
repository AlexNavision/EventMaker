using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ticket_Favorits : MonoBehaviour
{
    [SerializeField]
    GameObject[] Windows;// 0 - MyTicket; 1 - Calendar; 2 - MyFavoritTicket  OR   0 - ticketCard ; 1 - ticketCardQRcode
    [SerializeField]
    GameObject[] Tabs; //иконки сверху
    private int OpenWindow = 0;
    public void OpenWindowChange(int tabs) //пофиксить эту фигню
    {
        if (tabs == OpenWindow) return;
        Tabs[OpenWindow].GetComponentInChildren<Text>().color = Color.gray;
        Tabs[OpenWindow].GetComponentInChildren<Image>().enabled = false;
        Windows[OpenWindow].SetActive(false);
        OpenWindow = tabs;
        Windows[OpenWindow].SetActive(true);
        Tabs[OpenWindow].GetComponentInChildren<Text>().color = Color.red;
        Tabs[OpenWindow].GetComponentInChildren<Image>().enabled = true;
    }
    public void OpenQRcodeTicket()
    {
        Windows[OpenWindow].SetActive(false);
        if (OpenWindow == 0) OpenWindow++;
        else OpenWindow--;
        Windows[OpenWindow].SetActive(true);
    }
}
