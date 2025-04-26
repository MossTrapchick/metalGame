using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private RectTransform QTEParent;
    [SerializeField] private GameObject QTEPrefab;
    [SerializeField] private List<KeyCode> possibleKeys;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float keyPressTime = 1f;
    [SerializeField] private int roundKeysCount = 4;
    [SerializeField] private int maxKeysCount = 10;

    private Coroutine _qteSpawnRoutine;
    private Queue<QTEKey> _qtePool = new Queue<QTEKey>();
    private List<KeyCode> _activeKeys = new List<KeyCode>();

    public int pressedKeysCount { get; private set; } = 0;
    public int missedKeysCount { get; private set; } = 0;

    public UnityEvent OnRoundStarted;
    public UnityEvent OnRoundFinished;
    public UnityEvent<int> OnQTEPassed;
    public UnityEvent<int> OnQTEMissed;

    private void Awake()
    {
        OnRoundStarted.AddListener(() => { Debug.Log("Round Started"); ; });
        OnRoundFinished.AddListener(() => { Debug.Log("Round Finished"); ; });
        PreloadQTEKeys();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartRound(roundKeysCount);
    }

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

    public void StartRound(int QTEKeysCount)
    {
        OnRoundStarted?.Invoke();

        if (_qteSpawnRoutine != null)
            StopRound();
        _qteSpawnRoutine ??= StartCoroutine(SpawnQTECycle(QTEKeysCount));
    }

    private IEnumerator SpawnQTECycle(int QTEKeysCount)
    {
        yield return new WaitForSeconds(startDelay);
        SpawnRandomQTE();

        while (_activeKeys.Count < QTEKeysCount)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomQTE();
        }

        yield return new WaitForSeconds(keyPressTime);
        StopRound();
    }

    private void SpawnRandomQTE()
    {
        if (_activeKeys.Count >= maxKeysCount) return;

        KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Count)];

        QTEKey newKey = GetQTEKeyFromPool();
        newKey.Initialize(this, randomKey, keyPressTime);

        _activeKeys.Add(randomKey);
    }

    public void StopRound()
    {
        if (_qteSpawnRoutine != null)
        {
            StopCoroutine(_qteSpawnRoutine);
            _qteSpawnRoutine = null;
        }

        Debug.Log($"Pressed keys = {pressedKeysCount}; missed keys = {missedKeysCount}");

        _activeKeys.Clear();
        ResetKeysCounts();
        OnRoundFinished?.Invoke();
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
            OnQTEPassed?.Invoke(missedKeysCount);
        }
        else
            Debug.LogWarning("All keys are already pressed");
    }

    public void ResetKeysCounts()
    {
        pressedKeysCount = 0;
        missedKeysCount = 0;
    }
}