using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SavesReflexion;

/// <summary>
/// Менеджер сохранений.
/// <para>
/// Для того, чтобы сохранить какие-либо данные, нужно:
/// <para>
/// 1) Приписать к классу аттрибут <c>[SaveContainer(category)]</c>
///
/// category - категория сохранения. Оно обозначает, к какому файлу
/// сохранения контейнер относится.
/// SaveManager.CORE_CATEGORY - базовая категория сохранения, игра
/// всегда загружает её изначально. В неё можно отнести настройки.
/// SaveManager.GAME_CATEGORY - категория игры. Инвентарь, прогресс, очки
/// должны находится под этой категорией.
/// Таким образом, в будущем возможно создание слотов сохранений.
/// </para>
/// <para>
/// 2) Объявить СТАТИЧЕСКОЕ ПОЛЕ с аттрибутом <c>[SaveData(default)]</c>
/// 
/// default - изначальное значение в сохранении.
/// <example>
/// <para>
/// 
/// Например:
/// <code>
/// [SaveContainer(SaveManager.GAME_CATEGORY)]
/// public class PlayerData
/// {
///     [SaveData(0)] static int score;
///     public void AddScore(int amount) => score += amount;
/// }
/// </code>
/// </para>
/// НЕПРАВИЛЬНО:
/// <code>
/// [SaveData(0)] int nonstatic;
/// [SaveData(0)] static int property {get; set;}
/// </code>
/// </example>
/// </para>
/// 
/// </para>
/// Загрузка происходит в начале игры. В контейнеры АВТОМАТИЧЕСКИ заносится
/// сохранённая информация.
/// За сохранение информации отвечает менеджер сохранений, который собирает данные
/// со всех полей в определённой категории и сохраняет их в файл.
/// <para>
/// Проблемы:
/// 1) Невозможно понять, когда именно произошло занесение информации в поля
/// 2) BinaryFormatter нынче небезопасен (хацкеры могут вместо сейва подкинуть троянчик)
/// </para>
/// </summary>
public class SaveManager : MonoBehaviour
{
    /// <summary> Основная категория сохранения </summary>
    public const string CORE_CATEGORY = "main";
    /// <summary> Категория сохранения для игры </summary>
    public const string GAME_CATEGORY = "game";
    /// <summary> Имя основного файла сохранения </summary>
    public const string CORE_SAVE_FILE_NAME = "general.save";
    /// <summary> Имя файла сохранения - игрового слота </summary>
    public const string GAME_SAVE_FILE_NAME = "game_slot_0.save";

    [Tooltip("Нужно ли сохраняться при выходе из игры?")]
    public bool saveOnQuit = false;
    /// <summary>
    /// Функция сохраняет определённую категорию сохранения в файл.
    /// Файл будет создан в папке Application.persistentDataPath
    /// </summary>
    /// <param name="filename">Имя файла</param>
    /// <param name="category">Категория сохранения (если null - то все категории)</param>
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
    /// Функция загружает файл сохранения.
    /// </summary>
    /// <param name="filename">Имя файла сохранения</param>
    /// <param name="resetOthers">Нужно ли присвоить всем остальным полям значение по умолчанию?</param>
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
