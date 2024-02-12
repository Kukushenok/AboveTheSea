using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SavesReflexion;

/// <summary>
/// �������� ����������.
/// <para>
/// ��� ����, ����� ��������� �����-���� ������, �����:
/// <para>
/// 1) ��������� � ������ �������� <c>[SaveContainer(category)]</c>
///
/// category - ��������� ����������. ��� ����������, � ������ �����
/// ���������� ��������� ���������.
/// SaveManager.CORE_CATEGORY - ������� ��������� ����������, ����
/// ������ ��������� � ����������. � �� ����� ������� ���������.
/// SaveManager.GAME_CATEGORY - ��������� ����. ���������, ��������, ����
/// ������ ��������� ��� ���� ����������.
/// ����� �������, � ������� �������� �������� ������ ����������.
/// </para>
/// <para>
/// 2) �������� ����������� ���� � ���������� <c>[SaveData(default)]</c>
/// 
/// default - ����������� �������� � ����������.
/// <example>
/// <para>
/// 
/// ��������:
/// <code>
/// [SaveContainer(SaveManager.GAME_CATEGORY)]
/// public class PlayerData
/// {
///     [SaveData(0)] static int score;
///     public void AddScore(int amount) => score += amount;
/// }
/// </code>
/// </para>
/// �����������:
/// <code>
/// [SaveData(0)] int nonstatic;
/// [SaveData(0)] static int property {get; set;}
/// </code>
/// </example>
/// </para>
/// 
/// </para>
/// �������� ���������� � ������ ����. � ���������� ������������� ���������
/// ���������� ����������.
/// �� ���������� ���������� �������� �������� ����������, ������� �������� ������
/// �� ���� ����� � ����������� ��������� � ��������� �� � ����.
/// <para>
/// ��������:
/// 1) ���������� ������, ����� ������ ��������� ��������� ���������� � ����
/// 2) BinaryFormatter ����� ����������� (������� ����� ������ ����� ��������� ��������)
/// </para>
/// </summary>
public class SaveManager : MonoBehaviour
{
    /// <summary> �������� ��������� ���������� </summary>
    public const string CORE_CATEGORY = "main";
    /// <summary> ��������� ���������� ��� ���� </summary>
    public const string GAME_CATEGORY = "game";
    /// <summary> ��� ��������� ����� ���������� </summary>
    public const string CORE_SAVE_FILE_NAME = "general.save";
    /// <summary> ��� ����� ���������� - �������� ����� </summary>
    public const string GAME_SAVE_FILE_NAME = "game_slot_0.save";

    [Tooltip("����� �� ����������� ��� ������ �� ����?")]
    public bool saveOnQuit = false;
    /// <summary>
    /// ������� ��������� ����������� ��������� ���������� � ����.
    /// ���� ����� ������ � ����� Application.persistentDataPath
    /// </summary>
    /// <param name="filename">��� �����</param>
    /// <param name="category">��������� ���������� (���� null - �� ��� ���������)</param>
    private void SaveCategory(string filename, string category = null)
    {
        Dictionary<string, object> saveData = SaveReflexion.GetSaveDataFromFields(category);
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(Application.persistentDataPath + "/" + filename))
        {
            bf.Serialize(file, saveData);
            file.Flush();
        }
    }

    /// <summary>
    /// ������� ��������� ���� ����������.
    /// </summary>
    /// <param name="filename">��� ����� ����������</param>
    /// <param name="resetOthers">����� �� ��������� ���� ��������� ����� �������� �� ���������?</param>
    private void LoadSaves(string filename, bool resetOthers = false)
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>();
        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open))
            {
                saveData = (Dictionary<string, object>)bf.Deserialize(file);
                file.Flush();
            }
        }
        SaveReflexion.SyncSaveFieldsToData(saveData, resetOthers);
    }

    private void Awake()
    {
        LoadData();
    }
    private void SaveData()
    {
        SaveCategory(GAME_SAVE_FILE_NAME, GAME_CATEGORY);
        SaveCategory(CORE_SAVE_FILE_NAME, CORE_CATEGORY);
    }
    private void LoadData()
    {
        LoadSaves(CORE_SAVE_FILE_NAME, true);
        LoadSaves(GAME_SAVE_FILE_NAME, false);
    }
    private void OnApplicationQuit()
    {
        if (saveOnQuit)
        {
            SaveData();
        }
    }
}
