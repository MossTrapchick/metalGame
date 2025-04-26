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
    // [SerializeField] private List<KeyCode> possibleKeys;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float keyPressTime = 1f;
    [SerializeField] private int roundKeysCount = 4;
    [Tooltip("For preload")][SerializeField] private int maxKeysCount = 10;

    public int pressedKeysCount /*{ get; private set; }*/ = 0;
    public int missedKeysCount /*{ get; private set; }*/ = 0;

    public UnityEvent OnRoundStarted;
    public UnityEvent OnRoundFinished;
    public UnityEvent<int> OnQTEPassed;
    public UnityEvent<int> OnQTEMissed;

    private Coroutine _qteSpawnRoutine;
    private Queue<QTEKey> _qtePool = new Queue<QTEKey>();
    // private List<KeyCode> _activeKeys = new List<KeyCode>();
    // private List<InputControl> _activeInputs = new List<InputControl>();
    private List<QTEKey> _activeQTEKeys = new List<QTEKey>();

    private InputSystem_Actions inputSystemAction; //= new();
    private ReadOnlyArray<InputControl> inputControls;

    private void Awake()
    {
        OnRoundStarted.AddListener(() => { Debug.Log("Round Started"); ; });
        OnRoundFinished.AddListener(() => { Debug.Log("Round Finished"); ; });
        PreloadQTEKeys();

        inputSystemAction = new();
        inputSystemAction.Enable();
        inputSystemAction.Player.QTE.performed += pressedKey => { CheckPressedKey(pressedKey); };
        inputSystemAction.Player.QTE.performed += pressedKey => { Debug.Log($"pressed key {pressedKey.control}"); };
        inputControls = inputSystemAction.Player.QTE.controls;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartRound(roundKeysCount);
    }

    private void CheckPressedKey(InputAction.CallbackContext pressedKey)
    {
        QTEKey key = _activeQTEKeys.Find(x => x._targetKey == pressedKey.control);
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

    public void StartRound(int QTEKeysCount)
    {
        if (_qteSpawnRoutine != null)
        {
            Debug.Log("Round was already started");
            StopRound();
            return;
        }
        _qteSpawnRoutine ??= StartCoroutine(SpawnQTERound(QTEKeysCount));

        OnRoundStarted?.Invoke();
    }

    private IEnumerator SpawnQTERound(int QTEKeysCount)
    {
        yield return new WaitForSeconds(startDelay);
        SpawnRandomQTE();

        // while (_activeInputs.Count < QTEKeysCount)
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
        // if (_activeKeys.Count >= maxKeysCount) return;
        // if (_activeInputs.Count >= maxKeysCount) return;
        if (_activeQTEKeys.Count >= maxKeysCount) return;

        // KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Count)];
        InputControl randomKey = inputControls[Random.Range(0, inputControls.Count)];//possibleKeys[Random.Range(0, possibleKeys.Count)];

        QTEKey newKey = GetQTEKeyFromPool();
        newKey.Initialize(this, randomKey, keyPressTime);//, inputSystemAction, inputControls);

        // _activeKeys.Add(randomKey);
        // _activeInputs.Add(randomKey);
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

            Debug.Log($"Pressed keys = {pressedKeysCount}; missed keys = {missedKeysCount}");
            ResetKeysCounts();
            Debug.Log($"Reseted: Pressed keys = {pressedKeysCount}; missed keys = {missedKeysCount}");

            // _activeInputs.Clear();
            foreach (QTEKey key in _activeQTEKeys)
            {
                key.Hide();
                // _activeQTEKeys.Remove(key);
            }
            _activeQTEKeys.Clear();
            OnRoundFinished?.Invoke();
        }
    }

    public void ResetKeysCounts()
    {
        pressedKeysCount = 0;
        missedKeysCount = 0;
    }
}