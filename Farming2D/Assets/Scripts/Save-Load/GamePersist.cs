using System.Collections.Generic;
using UnityEngine;


public class GamePersist : MonoBehaviour
{
    public static GamePersist Instance { get; private set; }
    void Start() => Load();

    void OnApplicationQuit() => Save();

    GameData _gameData = new();

    public GameObject CropPrefab;
    public GameObject FieldPrefab;

    public List<Field> fields = new();

    public Transform arableLands;

    private void Awake()
    {
        Instance = this;
    }
    void Load()
    {

        if (!PlayerPrefs.HasKey("GameData")) return;

        Player _player = Player.Instance;

        string json = PlayerPrefs.GetString("GameData");

        _gameData = JsonUtility.FromJson<GameData>(json);

        _player.transform.position = _gameData.PlayerPosition;


        if (_gameData.FieldDatas.Count > 0)
        {
            for (int i = 0; i < _gameData.FieldDatas.Count; i++)
            {
                Field field = Instantiate(FieldPrefab).GetComponent<Field>();
                fields.Add(field);
                field.Load(_gameData.FieldDatas[i]);
                field.transform.SetParent(arableLands);
            }
        }

        if (_gameData.CropDatas.Count > 0)
        {
            foreach (var cropData in _gameData.CropDatas)
            {
                Crop crop = Instantiate(CropPrefab).GetComponent<Crop>();

                crop.Load(cropData);
            }
        }

        FindObjectOfType<Inventory>().Load(_gameData.inventoryData);

    }

    void Save()
    {
        Player _player = Player.Instance;

        _gameData.PlayerPosition = _player.transform.position;

        _gameData.CropDatas.Clear();

        _gameData.FieldDatas.Clear();

        _gameData.inventoryData = new();

        foreach (var Field in fields) _gameData.FieldDatas.Add(Field.Data());
        foreach (var crop in FindObjectsOfType<Crop>()) _gameData.CropDatas.Add(crop.Data());

        _gameData.inventoryData = FindObjectOfType<Inventory>().Data();


        var json = JsonUtility.ToJson(_gameData);
        PlayerPrefs.SetString("GameData", json);
    }
}