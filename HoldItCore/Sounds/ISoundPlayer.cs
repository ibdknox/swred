using System;

public interface ISoundPlayer
{
    Action Play(String soundfile, int volume, bool loop);
}