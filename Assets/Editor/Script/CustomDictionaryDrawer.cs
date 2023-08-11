using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Com.Unnamed.RacingGame.UI;
using Com.Unnamed.RacingGame.Input;

[CustomPropertyDrawer(typeof(RoleDictionary))]
public sealed class RoleDictionaryDrawer : SerializableDictionaryPropertyDrawer { }
