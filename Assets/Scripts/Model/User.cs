using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;

[Table("users")]
public class User
{
  [PrimaryKey, AutoIncrement]
  [Column("id")]
  public int Id { get; set; }

  [Unique, NotNull]
  [Column("username")]
  public string Username { get; set; }

  [NotNull]
  [Column("password")]
  public string Password { get; set; }

}
