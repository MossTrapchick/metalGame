using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

public class QTEKey : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private Vector2 spawnAreaMin, spawnAreaMax;

    private QTEManager _QTEManager;
    public InputControl _targetKey { get; private set; } //= KeyCode.Q;
    private float _timeToPress = 1f;
    private float _timer;
    private bool _isActive = true;

    public UnityEvent OnSuccessedQTE;
    public UnityEvent OnFailedQTE;

    // private InputSystem_Actions inputSystemAction;
    // private ReadOnlyArray<InputControl> inputControls;

    private void Awake()
    {
        keyText ??= GetComponentInChildren<TMP_Text>();
        OnSuccessedQTE.AddListener(() => { Debug.Log($"Successed QTE {keyText.text}"); });
        OnFailedQTE.AddListener(() => { Debug.Log($"Failed QTE {keyText.text}"); });
    }

    public void Initialize(QTEManager qteManager, InputControl key, float pressTime)//, InputSystem_Actions inputSystemAction, ReadOnlyArray<InputControl> inputControls)
    {
        _QTEManager = qteManager;
        OnSuccessedQTE.AddListener(AddPressedKey);
        OnFailedQTE.AddListener(AddMissedKey);

        _targetKey = key;
        _timeToPress = pressTime;
        keyText.text = key.path;

        // this.inputSystemAction = inputSystemAction;
        // inputSystemAction.Player.QTE.performed += pressedKey => { Check(); };
        // this.inputControls = inputControls;

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
        // if (_timer >= _timeToPress)
        // {
        //     Fail();
        //     // return;
        // }
        // else

        // inputControls.

        // if (Input.GetKeyDown(_targetKey))
        // if (pressedKey.)
        // {
        //     Success();
        // }
    }

    private void Success()
    {
        Hide();
        OnSuccessedQTE?.Invoke();
    }

    private void Fail()
    {
        Hide();
        OnFailedQTE?.Invoke();
    }

    public void Hide()
    {
        _isActive = false;
        gameObject.SetActive(false);

        OnSuccessedQTE.RemoveListener(AddPressedKey);
        OnFailedQTE.RemoveListener(AddMissedKey);
        _QTEManager.ReturnQTEKeyToPool(this);
    }

    private void AddPressedKey() => _QTEManager.IncreasePressedKeysCount();

    private void AddMissedKey() => _QTEManager.IncreaseMissedKeysCount();
}