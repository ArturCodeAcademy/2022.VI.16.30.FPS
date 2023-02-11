using UnityEngine;

public class SpritePoolElement : PoolElement
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField, Min(0)] private float _lifeTime;
    [SerializeField, Min(0)] private float _destroyDuration;
    [SerializeField]private SpriteRenderer _spriteRenderer;

    private float _leftLifeTime;
    private float _leftExistTime;
    private Color _color;

    public override void OnGetElement()
    {
        _color = Color.white;
        gameObject.SetActive(true);
        _leftLifeTime = _lifeTime;
        _leftExistTime = _destroyDuration;
        _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
        _spriteRenderer.color = _color;
    }

    private void Update()
    {
        if (_leftLifeTime > 0)
            _leftLifeTime -= Time.deltaTime;
        else if (_leftExistTime > 0)
        {
            _leftExistTime -= Time.deltaTime;
            _color.a = _leftExistTime / _destroyDuration;
            _spriteRenderer.color = _color;
        }
        else
        {
            gameObject.SetActive(false);
            Release();
        }
    }
}
