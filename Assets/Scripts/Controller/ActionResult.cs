using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionResult<T>
{
  public string status;
  public string error;
  public bool hasError;
  public T item;
}
