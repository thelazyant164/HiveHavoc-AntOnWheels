using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class QuitButton : BaseButton
    {
        protected override void OnClick() => Application.Quit();
    }
}
