using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NormalNoteData
{
    public int position;
    public int line;
    public int length;
    public NormalNoteData(int position, int line, int length)
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
    public int length;
    public HoldNoteData(int position, int line, int noteType, int count, int length)
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
    public int length;
    public SlideNoteData(int position, int line, int length)
    {
        this.position = position;
        this.line = line;
        this.length = length;
    }
}

[Serializable]
public struct FlickNoteData
{
    public int position;
    public int line;
    public int length;
    public int direction;
    public FlickNoteData(int position, int line, int length, int direction)
    {
        this.position = position;
        this.line = line;
        this.length = length;
        this.direction = direction;
    }
}

[Serializable]
public struct UpFlickNoteData
{
    public int position;
    public int line;
    public int length;
    public UpFlickNoteData(int position, int line, int length)
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
    public int length;
    public DownFlickNoteData(int position, int line, int length)
    {
        this.position = position;
        this.line = line;
        this.length = length;
    }
}