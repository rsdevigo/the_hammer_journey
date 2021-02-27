using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionResult<T>
{
  public string status = "SUCCESS";
  public string error;
  public bool hasError = false;
  public T item;
}
