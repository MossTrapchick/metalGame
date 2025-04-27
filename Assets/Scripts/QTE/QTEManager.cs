using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private RectTransform QTEParent;
    [SerializeField] private GameObject QTEPrefab;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float keyPressTime = 1f;
    [SerializeField] private int roundKeysCount = 4;
    [Tooltip("For preload")][SerializeField] private int maxKeysCount = 10;

    public int pressedKeysCount { get; private set; } = 0;
    public int missedKeysCount { get; private set; } = 0;

    public UnityEvent OnRoundStarted;
    public UnityEvent OnRoundFinished;

    public static UnityEvent<int> OnQTEPassed = new();
    public static UnityEvent<int> OnQTEMissed = new();

    public UnityEvent OnFullSuccessedFinished;
    public UnityEvent<int> OnHalfSuccessedFinished;
    public UnityEvent OnFailedFinished;

    private Coroutine _qteSpawnRoutine;
    private Queue<QTEKey> _qtePool = new Queue<QTEKey>();
    private List<QTEKey> _activeQTEKeys = new List<QTEKey>();

    private InputSystem_Actions inputSystemAction;
    private ReadOnlyArray<InputControl> inputControls;

    private void Awake()
    {
        OnRoundStarted.AddListener(() => { Debug.Log("Round Started"); ; });
        OnRoundFinished.AddListener(() => { Debug.Log($"Round Finished: \n Pressed keys = {pressedKeysCount}; missed keys = {missedKeysCount}"); ; });

        OnFullSuccessedFinished.AddListener(() => { Debug.Log($"Full success!!!"); ; });
        OnHalfSuccessedFinished.AddListener(value => { Debug.Log($"Win percent = {value}"); ; });
        OnFailedFinished.AddListener(() => { Debug.Log($"Full fail!!!"); ; });

        OnQTEPassed.AddListener(value => { Debug.Log($"QTE passed {value} times"); ; });
        OnQTEMissed.AddListener(value => { Debug.Log($"QTE missed {value} times"); ; });

        PreloadQTEKeys();

        inputSystemAction = new();
        inputSystemAction.Enable();
        inputSystemAction.Player.QTE.performed += pressedKey => { CheckPressedKey(pressedKey.control); };
        // inputSystemAction.Player.QTE.performed += pressedKey => { Debug.Log($"pressed key {pressedKey.control}"); };
        inputControls = inputSystemAction.Player.QTE.controls;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartRound();
    }

    private void CheckPressedKey(InputControl pressedKey)
    {
        QTEKey key = _activeQTEKeys.Find(x => x.TargetKey == pressedKey);
        key?.PressKey();
    }

    #region Pool
    private void PreloadQTEKeys()
    {
        for (int i = 0; i < maxKeysCount; i++)
        {
            QTEKey key = CreateQTEKey();
            _qtePool.Enqueue(key);
        }
    }

    private QTEKey CreateQTEKey()
    {
        GameObject newButton = Instantiate(QTEPrefab, QTEParent);
        newButton.SetActive(false);
        return newButton.GetComponent<QTEKey>();
    }

    private QTEKey GetQTEKeyFromPool()
    {
        if (_qtePool.Count > 0)
        {
            QTEKey key = _qtePool.Dequeue();
            return key;
        }
        else
        {
            Debug.LogWarning("QTEPool is empty, created new QTEPrefab.");
            return CreateQTEKey();
        }
    }

    public void ReturnQTEKeyToPool(QTEKey QTEKey)
    {
        _qtePool.Enqueue(QTEKey);
    }
    #endregion

    public void StartRound(/*int QTEKeysCount*/)
    {
        if (_qteSpawnRoutine != null)
        {
            Debug.Log("Round was already started");
            StopRound();
            return;
        }
        _qteSpawnRoutine ??= StartCoroutine(SpawnQTERound(roundKeysCount));

        OnRoundStarted?.Invoke();
    }

    private IEnumerator SpawnQTERound(int QTEKeysCount)
    {
        yield return new WaitForSeconds(startDelay);
        SpawnRandomQTE();

        while (_activeQTEKeys.Count < QTEKeysCount)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomQTE();
        }

        yield return new WaitForSeconds(keyPressTime);
        StopRound();
    }

    private void SpawnRandomQTE()
    {
        if (_activeQTEKeys.Count >= maxKeysCount) return;

        InputControl randomKey = inputControls[Random.Range(0, inputControls.Count)];

        QTEKey newKey = GetQTEKeyFromPool();
        newKey.Initialize(this, randomKey, keyPressTime, QTEParent);

        _activeQTEKeys.Add(newKey);
    }

    public void IncreasePressedKeysCount()
    {
        if (pressedKeysCount + missedKeysCount < maxKeysCount)
        {
            pressedKeysCount++;
            OnQTEPassed?.Invoke(pressedKeysCount);
        }
        else
            Debug.LogWarning("All keys are already pressed");
    }

    public void IncreaseMissedKeysCount()
    {
        if (pressedKeysCount + missedKeysCount < maxKeysCount)
        {
            missedKeysCount++;
            OnQTEMissed?.Invoke(missedKeysCount);
        }
        else
            Debug.LogWarning("All keys are already pressed");
    }

    public void StopRound()
    {
        if (_qteSpawnRoutine != null)
        {
            StopCoroutine(_qteSpawnRoutine);
            _qteSpawnRoutine = null;

            CalculateWinPercent();
            OnRoundFinished?.Invoke();

            foreach (QTEKey key in _activeQTEKeys)
            {
                key.Hide();
            }
            _activeQTEKeys.Clear();
            ResetKeysCounts();
        }
    }

    private void CalculateWinPercent()
    {
        int keysCount = pressedKeysCount + missedKeysCount;
        if (keysCount == 0) return;

        int winPercent = pressedKeysCount * 100 / keysCount;

        switch (winPercent)
        {
            case 100:
                OnFullSuccessedFinished?.Invoke();
                break;

            case 0:
                OnFailedFinished?.Invoke();
                break;

            default:
                OnHalfSuccessedFinished?.Invoke(winPercent);
                break;
        }
    }

    public void ResetKeysCounts()
    {
        pressedKeysCount = 0;
        missedKeysCount = 0;
    }
}