using System;

public interface IPoolElement
{
    public Action OnElementUsed { get; set; }
    public void ResetElement() { }
}
