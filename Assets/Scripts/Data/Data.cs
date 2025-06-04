using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NormalNoteData
{
    public int position;
    public int line;
    public NormalNoteData(int position, int line)
    {
        this.position = position;
        this.line = line;
    }
}

[Serializable]
public struct HoldNoteData
{
    public int position;
    public int line;
    public int noteType;
    public int count;
    public HoldNoteData(int position, int line, int noteType, int count)
    {
        this.position = position;
        this.line = line;
        this.noteType = noteType;
        this.count = count;
    }
}

[Serializable]
public struct SlideNoteData
{
    public int position;
    public int line;
    public SlideNoteData(int position, int line)
    {
        this.position = position;
        this.line = line;
    }
}

[Serializable]
public struct UpFlickNoteData
{
    public int position;
    public int line;
    public UpFlickNoteData(int position, int line)
    {
        this.position = position;
        this.line = line;
    }
}

[Serializable]
public struct DownFlickNoteData
{
    public int position;
    public int line;
    public DownFlickNoteData(int position, int line)
    {
        this.position = position;
        this.line = line;
    }
}