using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NormalNoteData
{
    public int position;
    public int line;
    public float length;
    public NormalNoteData(int position, int line, float length)
    {
        this.position = position;
        this.line = line;
        this.length = length;
    }
}

[Serializable]
public struct HoldNoteData
{
    public int position;
    public int line;
    public int noteType;
    public int count;
    public float length;
    public HoldNoteData(int position, int line, int noteType, int count, float length)
    {
        this.position = position;
        this.line = line;
        this.noteType = noteType;
        this.count = count;
        this.length = length;
    }
}

[Serializable]
public struct SlideNoteData
{
    public int position;
    public int line;
    public float length;
    public SlideNoteData(int position, int line, float length)
    {
        this.position = position;
        this.line = line;
        this.length = length;
    }
}

[Serializable]
public struct UpFlickNoteData
{
    public int position;
    public int line;
    public float length;
    public UpFlickNoteData(int position, int line, float length)
    {
        this.position = position;
        this.line = line;
        this.length = length;
    }
}

[Serializable]
public struct DownFlickNoteData
{
    public int position;
    public int line;
    public float length;
    public DownFlickNoteData(int position, int line, float length)
    {
        this.position = position;
        this.line = line;
        this.length = length;
    }
}