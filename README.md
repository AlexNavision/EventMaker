# EventMakers. Приложение для Андроид - MOS.YOU (Возможна компиляция для IOS) 
INTRODUCTION

Прототип был сделан на игровом движке UnityEngine. Почему? Так быстрее, не нужно думать об оптимизации - движок все сделал за нас.
Трудности с интеграцией/переписыванием кода? Нет. Код написан на C#. Особенность UnityEngine - это ориентированность на объекты и компоненты.
Все что находится на экране - это Объекты (GameObject). Мы обращаемся напрямую к компонентам этих объектов (Image = изображения, Text = текст) и прочее.
Что нужно будет переписывать? (чего нет на других фрейморках) - Это все что использует библиотеку (using UnityEngine.UI), то есть нужно будет заменить все элементы пользовательского интерфейса.

FILE LOCATION

В папке Assets находим все объекты с типом .CS - Это скрипты. Мой код только в них.

Event.cs - скрипт для Мероприятий. Хранит информацию о мероприятии и выводит её пользователю.

EventCard.cs - скрипт для отдельной карточки мероприятия. не реализовано, должна была выводить на экран информацию о мероприятии, при входе на карточку мероприятия.

EventMakerLibrary.cs - содержит пространство имен "EventMakersLibrary". В классе EventMakerLibrary находятся все основные методы для обработки информации с сервера, отправки запросов на сервер. 

MainMenu.cs - 
