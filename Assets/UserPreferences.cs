using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserPreferences : MonoBehaviour
{
    [SerializeField]
    Image procentLine;
    [SerializeField]

    GameObject[] Question;
    private int QuestionNo = 0;
    private int QuestionCount = 9;
    private int procentStep;
    private List<Categories> categories = new List<Categories>();
    private List<int> answer = new List<int>();
    private void Start()
    {
        QuestionCount = Question.Length;
        procentStep = 1 / QuestionCount;
    }
    public void NextQuestion(int i = -1)
    {
        print(QuestionNo);
        switch (QuestionNo)
        {
            case 0: Destroy(Question[QuestionNo]); QuestionNo++; Question[QuestionNo].SetActive(true); return;
            case 1: Destroy(Question[QuestionNo]); QuestionNo++; Question[QuestionNo].SetActive(true); return;
            case 2: answer.Add(i); if (i < 24) { Destroy(Question[QuestionNo]); QuestionNo += 2; procentLine.fillAmount += procentStep * 2; Question[QuestionNo].SetActive(true); return; } break; //�������� ������� <24, ����� ��������� ������ ��� ����� �� ���������. ��� ������ ������!!!
            case 3: if (i == 0) answer.Add(i); ; break; //���� ���� �� //+32085 2085  153299  1299  11085 8085  - ��������� ID ��� ����������
            case 4: answer.Add(i); break;
            case 5: answer.Add(i); break;
            case 6: answer.Add(i); break;
            case 7: answer.Add(i); break;
            case 8: break;
            case 9: break; //������� ���������
            case 10: Manager.instance.ChangeWindow(0);Destroy(gameObject);return;
        }
        Destroy(Question[QuestionNo]);
        procentLine.fillAmount += procentStep;
        QuestionNo++;
        Question[QuestionNo].SetActive(true);
    }
    [SerializeField]
    InputField field;
    public void InputQuestion(string s)
    {
        int i;
        if (int.TryParse(s, out i))
            if (i > 8)
                NextQuestion(i);
        field.textComponent.color = Color.red;

    }
    int categoryselect = 0;
    [SerializeField]
    GameObject buttonReady,prefabCategories;
    [SerializeField]
    RectTransform rectTransform;
    public void CategorySelect()
    {
        if (categoryselect == 3) { buttonReady.SetActive(true); return; }
        categoryselect++;
        Instantiate(prefabCategories, rectTransform).GetComponentInChildren<Text>().text = "���������";
    }
    struct Categories
    {
        int id;
        string title;
    }
    //����� ����� ��� ���������� �����������:
    /*
    id 18299
title ���������������
id 4299
title ������������� � �������������
id 2299
title ���������
id 15299
title �����������
id 5299
title ��������� ���������
id 350299
title ���������������
id 183299
title ����������
id 319299
title ���������� ����������� ��������
id 153299
title ���������� ��������� ��������� �����
id 231299
title ��� �����
id 1299
title ���������� �����
id 7299
title ��������
id 10299
title ������������
id 12299
title ��������� � �������������������
id 3299
title ��������
id 11299
title ��������������� � ������������� �����
id 351299
title ���������
id 16299
title ��������� ����������
id 6299
title �����
id 14299
title ����� � ���������s



id 32085 
title ����
id 27085
title ����������
id 1085
title ������
id 2085
title �����
id 3085
title �����
id 4085
title ����������� ����
id 5085
title ���������� ����
id 6085
title ���������� �������
id 7085
title ��������������� �������
id 8085
title ���������(������������) ������������
id 9085
title ����������
id 10085
title ��������
id 33085
title ���������� ������
id 31085
title ��� ��������
id 11085
title �����
    */
}
