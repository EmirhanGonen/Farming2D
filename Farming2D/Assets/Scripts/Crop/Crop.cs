using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public delegate void GrowDelegate();

    public GrowDelegate Grow;

    public CropSO _cropSO;

    private Sprite[] _growStageSprites;
    private float[] _growDurations;

    private SpriteRenderer _spriteRenderer;

    private float elapsedTime;
    public int stage;
    public bool isGrown;

    private GameObject _output;
    private ParticleSystem grownParticle;

    Transform _player;

    public Field field;

    private void Awake()
    {
        SetScriptableObjectValue();
        Grow += GrowingProcess;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = Player.Instance.transform;
        grownParticle = FindObjectOfType<ParticleSystem>();
    }

    private void Update()
    {
        Grow?.Invoke();
    }

    public void GrowingProcess()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < _growDurations[stage]) return;

        grownParticle.transform.position = transform.position;

        grownParticle.Play();

        if (stage < _growDurations.Length) stage++;

        _spriteRenderer.sprite = _growStageSprites[stage];

        isGrown = stage == _growStageSprites.Length - 1;

        if (isGrown) Grow -= GrowingProcess;
    }
    private void SetScriptableObjectValue()
    {
        _growStageSprites = _cropSO.growStageSprite;
        _growDurations = _cropSO.growStageUpTime;
        _output = _cropSO.Output;
    }

    private void OnMouseUpAsButton()
    {
        if (Vector3.Distance(_player.position, transform.position) > 4f) return;

        if (!isGrown) return;


        Product output = Instantiate(_output, transform.position, Quaternion.identity).GetComponent<Product>();
        //output.name = output.Prefab.name;

        if (field) field.isFull = false;

        Destroy(gameObject);
    }

    public CropData Data()
    {
        CropData data = new()
        {
            elapsedTime = elapsedTime,
            isGrown = isGrown,
            Stage = stage,
            cropSO = _cropSO,
            fieldIndex = GamePersist.Instance.fields.IndexOf(field),
        };

        return data;
    }

    public void Load(CropData cropData)
    {
        _cropSO = cropData.cropSO;
        elapsedTime = cropData.elapsedTime;
        stage = cropData.Stage;
        isGrown = cropData.isGrown;


        if (isGrown) Grow -= GrowingProcess;
        field = GamePersist.Instance.fields[cropData.fieldIndex];
        transform.position = field.transform.position;
        SetScriptableObjectValue();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _growStageSprites[stage];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Field field)) this.field = field;
    }
}