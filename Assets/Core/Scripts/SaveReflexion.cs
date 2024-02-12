using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Аттрибут для статического поля, которое будет сохраняться
/// и загружаться автоматически.
/// </summary>
public class SavedFieldAttribute : Attribute
{
    public object defaultValue { get; }
    public SavedFieldAttribute(object defaultValue = null)
    {
        this.defaultValue = defaultValue;
    }
}

/// <summary>
/// Аттрибут для классов, которые содержат автоматически сохраняемые
/// поля.
/// </summary>
public class SaveContainerAttribute: Attribute
{
    public string category { get; }
    public SaveContainerAttribute(string category = SaveManager.CORE_CATEGORY)
    {
        this.category = category;
    }
}

namespace SavesReflexion
{
    /// <summary>
    /// Тут происходят все чудеса с автоматическим присвоением и сохранением полей
    /// </summary>
    public static class SaveReflexion
    {
        private struct SaveFieldInfo
        {
            FieldInfo info;
            public string category { get; }
            private object defaultValue;
            public SaveFieldInfo(FieldInfo info)
            {
                this.info = info;
                SavedFieldAttribute attr = info.GetCustomAttribute<SavedFieldAttribute>();
                SaveContainerAttribute manager = info.DeclaringType.GetCustomAttribute<SaveContainerAttribute>();
                if (attr == null)
                {
                    category = string.Empty;
                    defaultValue = null;
                }
                else
                {
                    category = manager == null? null: manager.category;
                    defaultValue = attr.defaultValue;
                }
            }
            public string GetMyKeyForData()
            {
                return info.DeclaringType.FullName + "/" + info.Name;
            }
            public void ResetValue()
            {
                SetValue(defaultValue);
            }
            public object GetValue()
            {
                return info.GetValue(null);
            }
            public void SetValue(object data)
            {
                info.SetValue(null, data);
            }
        }
        private static Dictionary<string, SaveFieldInfo> saveFields;

        /// <summary>
        /// Функция выставляет полям нужные значения, взятые из файла сохранения
        /// </summary>
        /// <param name="saveData">Данные из файла сохранения</param>
        /// <param name="resetOthers">Нужно ли присвоить всем остальным полям значение по умолчанию?</param>
        public static void SyncSaveFieldsToData(Dictionary<string, object> saveData, bool resetOthers)
        {
            foreach (KeyValuePair<string, SaveFieldInfo> fieldKV in saveFields)
            {
                if (saveData.ContainsKey(fieldKV.Key))
                {
                    fieldKV.Value.SetValue(saveData[fieldKV.Key]);
                }
                else if (resetOthers)
                {
                    fieldKV.Value.ResetValue();
                }
            }
        }
        /// <summary>
        /// Функция возвращает данные для файла сохранения по какой-то категории.
        /// Если категория пуста, то данные будет состоять из полей всех категорий.
        /// </summary>
        /// <param name="category">Категория</param>
        /// <returns>Данные для файла сохранения</returns>
        public static Dictionary<string, object> GetSaveDataFromFields(string category = null)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            Func<SaveFieldInfo, bool> filter = x => true;
            if (category != null) filter = x => x.category == category;
            foreach (KeyValuePair<string, SaveFieldInfo> data in saveFields)
            {
                if (filter(data.Value))
                {
                    result.Add(data.Key, data.Value.GetValue());
                }
            }
            return result;
        }
        // Внутренняя функция для занесения информации о поле
        static void IndexateField(FieldInfo info)
        {
            SaveFieldInfo toAdd = new SaveFieldInfo(info);
            saveFields.Add(toAdd.GetMyKeyForData(), toAdd);
        }
        // Функция инициализации
        static SaveReflexion()
        {
            saveFields = new Dictionary<string, SaveFieldInfo>();
            var data = from a in AppDomain.CurrentDomain.GetAssemblies()
                       from t in a.GetTypes()
                       where t.GetCustomAttribute<SaveContainerAttribute>() != null
                       from f in t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                       where Attribute.IsDefined(f, typeof(SavedFieldAttribute))
                       select f;
            foreach (var fieldInfo in data)
            {
                IndexateField(fieldInfo);
            }
        }
    }
}