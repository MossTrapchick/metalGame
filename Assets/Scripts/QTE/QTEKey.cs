using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class QTEKey : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private Vector2 spawnAreaMin, spawnAreaMax;

    private QTEManager _QTEManager;
    public InputControl _targetKey { get; private set; }
    private float _timeToPress = 1f;
    private float _timer;
    private bool _isActive = true;

    public UnityEvent OnSuccessedQTE;
    public UnityEvent OnFailedQTE;

    private void Awake()
    {
        keyText ??= GetComponentInChildren<TMP_Text>();
        OnSuccessedQTE.AddListener(() => { Debug.Log($"Successed QTE {keyText.text}"); });
        OnFailedQTE.AddListener(() => { Debug.Log($"Failed QTE {keyText.text}"); });
    }

    public void Initialize(QTEManager qteManager, InputControl key, float pressTime)//, InputSystem_Actions inputSystemAction, ReadOnlyArray<InputControl> inputControls)
    {
        _QTEManager = qteManager;
        // OnSuccessedQTE.AddListener(AddPressedKey);
        // OnFailedQTE.AddListener(AddMissedKey);

        _targetKey = key;
        _timeToPress = pressTime;
        keyText.text = key.displayName;

        SpawnKey();
    }

    private void SpawnKey()
    {
        transform.localPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y));

        _timer = 0f;
        _isActive = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_isActive) return;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeToPress && _isActive)
        {
            Fail();
            // return;
        }
    }

    public void PressKey()
    {
        if (!_isActive) return;
        if (_timer < _timeToPress && _isActive)
        {
            Success();
        }
    }

    private void Success()
    {
        Hide();
        _QTEManager.IncreasePressedKeysCount();
        OnSuccessedQTE?.Invoke();
    }

    private void Fail()
    {
        Hide();
        _QTEManager.IncreaseMissedKeysCount();
        OnFailedQTE?.Invoke();
    }

    public void Hide()
    {
        _isActive = false;
        gameObject.SetActive(false);

        // OnSuccessedQTE.RemoveListener(AddPressedKey);
        // OnFailedQTE.RemoveListener(AddMissedKey);
        _QTEManager.ReturnQTEKeyToPool(this);
    }

    // private void AddPressedKey() => _QTEManager.IncreasePressedKeysCount();
    // private void AddMissedKey() => _QTEManager.IncreaseMissedKeysCount();
}