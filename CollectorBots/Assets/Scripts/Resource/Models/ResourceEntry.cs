using System;

//структура для отображения в инспекторе
[Serializable]
public struct ResourceEntry
{
    public ResourceType Type;//тип ресурса. Энам
    public int Amount;//стоимость или количество
}