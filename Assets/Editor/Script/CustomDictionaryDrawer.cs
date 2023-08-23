using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;

[CustomPropertyDrawer(typeof(RoleDictionary))]
public sealed class RoleDictionaryDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(PollenDictionary))]
public sealed class PollenAmmoDrawer : SerializableDictionaryPropertyDrawer { }
