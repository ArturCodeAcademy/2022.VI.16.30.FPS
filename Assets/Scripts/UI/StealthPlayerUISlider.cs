using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StealthForPlayer))]
public class StealthPlayerUISlider : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _alphaChannelBackground;
    [SerializeField] Color _worried;
    [SerializeField] Color _worriedBackground;
    [SerializeField] Color _angry;
    [Space(5)]
    [SerializeField] List<Image> _stealthIcons;
    [SerializeField] List<Image> _stealthIconsBackground;
    [SerializeField] List<Slider> _sliders;
    [SerializeField] Image _stealthCircle;

    private Transform _player;
    private Transform _sender;
    private StealthForPlayer _stealth;
    private Coroutine _coroutine;

    void Awake()
    {
        OnCalmDown(default);
    }

    void Start()
    {
        _player = Player.Instance.transform;
        _stealth = GetComponent<StealthForPlayer>();
        _stealth.OnGetWorried.AddListener(OnGetWorried);
        _stealth.OnReact.AddListener(OnReact);
        _stealth.OnLoseTarget.AddListener(OnLoseTarget);
        _stealth.OnCalmDown.AddListener(OnCalmDown);
    }

    void Update()
    {
        if (_sender == null)
            return;

        Vector3 playerXZ = _player.forward;
        playerXZ.y = 0;
        Vector3 targetXZ = _sender.position - _player.position;
        targetXZ.y = 0;

        _stealthCircle.transform.rotation = Quaternion.Euler(Vector3.forward * Vector3.SignedAngle(playerXZ, targetXZ, Vector3.down));
    }

    void OnDisable()
    {
        _stealth.OnGetWorried.RemoveListener(OnGetWorried);
        _stealth.OnReact.RemoveListener(OnReact);
        _stealth.OnLoseTarget.RemoveListener(OnLoseTarget);
        _stealth.OnCalmDown.RemoveListener(OnCalmDown);
    }

    void OnGetWorried(StealthEventArgs args)
    {
        _sender = args.Sender;
        _stealthCircle.gameObject.SetActive(true);
        _stealthIcons.ForEach(x => x.gameObject.SetActive(true));
        _stealthIconsBackground.ForEach(x => x.gameObject.SetActive(true));

        _stealthIcons.ForEach(x => x.color = _worried);
        Color bg = _worriedBackground;
        bg.a *= _alphaChannelBackground;
        _stealthIconsBackground.ForEach(x => x.color = bg);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FillSlider(args));
    }

    void OnCalmDown(StealthEventArgs args)
    {
        _sender = null;
        _stealthCircle.gameObject.SetActive(false);
        _stealthIcons.ForEach(x => x.gameObject.SetActive(false));
        _stealthIconsBackground.ForEach(x => x.gameObject.SetActive(false));
    }
    
    void OnLoseTarget(StealthEventArgs args)
    {
        _stealthIcons.ForEach(x => x.gameObject.SetActive(true));
        _stealthIconsBackground.ForEach(x => x.gameObject.SetActive(true));

        _stealthIcons.ForEach(x => x.color = _worried);
        Color bg = _worriedBackground;
        bg.a *= _alphaChannelBackground;
        _stealthIconsBackground.ForEach(x => x.color = bg);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(UnfillSlider(args));
    }

    void OnReact(StealthEventArgs args)
    {
        _stealthIcons.ForEach(x => x.color = _angry);
        _stealthIconsBackground.ForEach(x => x.color = _angry);
    }

    IEnumerator FillSlider(StealthEventArgs args)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float time = args.ElapsedReactionTime;
        while(time < args.ReactionTime)
        {
            time += Time.deltaTime;
            _sliders.ForEach(x => x.value = time / args.ReactionTime);
            yield return wait;
        }
    }

    IEnumerator UnfillSlider(StealthEventArgs args)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float time = args.ElapsedReactionTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            _sliders.ForEach(x => x.value = time / args.ReactionTime);
            yield return wait;
        }
    }
}
