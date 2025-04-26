using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class QTEKey : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private Vector2 spawnAreaMin, spawnAreaMax;

    private QTEManager _QTEManager;
    private KeyCode _targetKey = KeyCode.Q;
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

    public void Initialize(QTEManager qteManager, KeyCode key, float pressTime)
    {
        _QTEManager = qteManager;
        OnSuccessedQTE.AddListener(() => { qteManager.IncreasePressedKeysCount(); });
        OnFailedQTE.AddListener(() => { qteManager.IncreaseMissedKeysCount(); });

        _targetKey = key;
        _timeToPress = pressTime;
        keyText.text = key.ToString();

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

        _timer += Time.deltaTime;

        if (_timer >= _timeToPress)
        {
            Fail();
            return;
        }

        if (Input.GetKeyDown(_targetKey))
        {
            Success();
        }
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

    private void Hide()
    {
        _isActive = false;
        gameObject.SetActive(false);
        _QTEManager.ReturnQTEKeyToPool(this);
    }
}