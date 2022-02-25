using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformConfigurationData
{
    //Most of this content was created from slide 6 in the powerpoint
    public int M = 0;
    public int N = 0;

    public float deltaSpace = 0.1f;
    public float carriedHeight = 1.0f;

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3}", M, N, deltaSpace, carriedHeight);
    }
}
