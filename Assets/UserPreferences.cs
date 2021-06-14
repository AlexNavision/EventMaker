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
            case 2: answer.Add(i); if (i < 24) { Destroy(Question[QuestionNo]); QuestionNo += 2; procentLine.fillAmount += procentStep * 2; Question[QuestionNo].SetActive(true); return; } break; //допустим возраст <24, тогда следующий вопрос про детей мы пропускам. Это только пример!!!
            case 3: if (i == 0) answer.Add(i); ; break; //есть дети да //+32085 2085  153299  1299  11085 8085  - возможные ID для фильтрации
            case 4: answer.Add(i); break;
            case 5: answer.Add(i); break;
            case 6: answer.Add(i); break;
            case 7: answer.Add(i); break;
            case 8: break;
            case 9: break; //выбрали категории
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
        Instantiate(prefabCategories, rectTransform).GetComponentInChildren<Text>().text = "Категория";
    }
    struct Categories
    {
        int id;
        string title;
    }
    //Малая часть для фильтрации мероприятий:
    /*
    id 18299
title Здравоохранение
id 4299
title Строительство и реконструкция
id 2299
title Транспорт
id 15299
title Образование
id 5299
title Городское хозяйство
id 350299
title Благоустройство
id 183299
title Технологии
id 319299
title Московские центральные диаметры
id 153299
title Московская программа реновации жилья
id 231299
title Мой район
id 1299
title Социальная сфера
id 7299
title Экология
id 10299
title Безопасность
id 12299
title Экономика и предпринимательство
id 3299
title Культура
id 11299
title Межрегиональные и международные связи
id 351299
title Госуслуги
id 16299
title Городское управление
id 6299
title Спорт
id 14299
title Наука и инновацииs



id 32085 
title ВДНХ
id 27085
title Библиотеки
id 1085
title Театры
id 2085
title Парки
id 3085
title Музеи
id 4085
title Выставочные залы
id 5085
title Концертные залы
id 6085
title Спортивные объекты
id 7085
title Образовательные объекты
id 8085
title Городское(общественное) пространство
id 9085
title Кинотеатры
id 10085
title Интернет
id 33085
title Культурные центры
id 31085
title Дом культуры
id 11085
title Катки
    */
}
