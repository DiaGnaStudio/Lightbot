using UnityEngine;

public class LightBlock : Block
{
    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private Material lightOn;
    [SerializeField] private Material lightOff;

    public State CurrentState { get; private set; } = State.IsOff;

    public override void Reaching()
    {
        //
    }

    public void SwitchState()
    {
        if (CurrentState == State.IsOff)
        {
            m_Renderer.material = lightOn;
            CurrentState = State.IsOn;
        }
        else
        {
            m_Renderer.material = lightOff;
            CurrentState = State.IsOff;
        }
    }

    public enum State
    {
        IsOff,
        IsOn,
    }
}