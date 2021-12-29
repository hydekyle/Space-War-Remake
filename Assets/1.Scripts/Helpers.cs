using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static List<T> ShuffleList<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
        return _list;
    }

    public static Vector2 SizeAttractorClosed()
    {
        return Vector2.one * 2.5f;
    }

    public static Vector2 SizeAttractorOpened(Ship ship)
    {
        return Vector2.one * (3 + ship.stats.movility);
    }

    public static void PlaySFX(AudioClip audioClip)
    {
        GameManager.audioSourceSFX.PlayOneShot(audioClip);
    }
}
