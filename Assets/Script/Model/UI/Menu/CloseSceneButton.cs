using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class CloseSceneButton : BaseButton
    {
        [SerializeField]
        private string sceneName;

        protected override void OnClick() => SceneManager.UnloadSceneAsync(sceneName);
    }
}
