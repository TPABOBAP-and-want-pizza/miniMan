using System;

public interface Transmitter
{
    public event Action OnSetActive;
    public event Action OnSetUnactive;
}
