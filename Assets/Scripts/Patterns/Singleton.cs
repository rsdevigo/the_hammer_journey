﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;

  public static T instance
  {
    get
    {
      return _instance;
    }
  }

  protected virtual void Awake()
  {
    if (_instance == null)
    {
      _instance = this as T;
    }
    else if (_instance != this)
    {
      Destroy(this);
    }
  }

  protected virtual void OnDestroy()
  {
    if (_instance != this)
    {
      return;
    }

    _instance = null;
  }
}
