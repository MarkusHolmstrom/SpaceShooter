using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class LCGRandomGenerator 
{
    private static int seed = 5725;
    private static readonly int a = 1664525;
    private static readonly int c = 10139042;
    private static readonly int m = 1415461035;

    public static float RandomLCGfloat(int min, int max)
    {
        if (max < min)
        {
            int finMax = min;
            int finMin = max;
            max = finMax;
            min = finMin;
        }
        // new seed
        seed += Mathf.Abs((int)DateTime.Now.Ticks);

        float initialRandom = (a * seed + c) % m;

        float randFloat = initialRandom % (float)max;

        if (randFloat < min)
        {
            randFloat = min;
        }
        else if (randFloat > max)
        {
            randFloat = max;
        }
        // even out the seed a little bit
        seed %= 10000;
        return randFloat;
    }

}
