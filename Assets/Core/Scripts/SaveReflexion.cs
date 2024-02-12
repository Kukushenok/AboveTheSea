using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �������� ��� ������������ ����, ������� ����� �����������
/// � ����������� �������������.
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
/// �������� ��� �������, ������� �������� ������������� �����������
/// ����.
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
    /// ��� ���������� ��� ������ � �������������� ����������� � ����������� �����
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
        /// ������� ���������� ����� ������ ��������, ������ �� ����� ����������
        /// </summary>
        /// <param name="saveData">������ �� ����� ����������</param>
        /// <param name="resetOthers">����� �� ��������� ���� ��������� ����� �������� �� ���������?</param>
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
        /// ������� ���������� ������ ��� ����� ���������� �� �����-�� ���������.
        /// ���� ��������� �����, �� ������ ����� �������� �� ����� ���� ���������.
        /// </summary>
        /// <param name="category">���������</param>
        /// <returns>������ ��� ����� ����������</returns>
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
        // ���������� ������� ��� ��������� ���������� � ����
        static void IndexateField(FieldInfo info)
        {
            SaveFieldInfo toAdd = new SaveFieldInfo(info);
            saveFields.Add(toAdd.GetMyKeyForData(), toAdd);
        }
        // ������� �������������
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