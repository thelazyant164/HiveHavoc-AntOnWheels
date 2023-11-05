using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class ResumeButton : BaseButton
    {
        protected override void OnClick()
        {
            PauseManager pause = PauseManager.Instance;
            if (pause == null)
            {
                Debug.LogError("PauseManager not found");
                return;
            }

            pause.Unpause();
        }
    }
}
