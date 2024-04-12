using System;

public interface Transmitter
{
    public event Action OnActivate;
    public event Action OnDeactivate;
}
