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
        seed += DateTime.Now.Millisecond;

        int initialRandom = (a * seed + c) % m;
        float randFloat = initialRandom % max;
        if (randFloat > -10 && randFloat < 10)
        {
            randFloat += min;
        }
        seed /= 1000;
        // generate more decimals
        //int divider = (max * 100);
        //int randCent = Mathf.Abs(initialRandom) % divider;

        //float randFloat = (float)randCent / (float)divider;
        Debug.Log(randFloat);
        return randFloat;
    }

}
