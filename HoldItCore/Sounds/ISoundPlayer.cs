using System;

public interface ISoundPlayer
{
    Action Play(String soundfile, bool loop);
}