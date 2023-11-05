using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class OpenSceneButton : BaseButton
    {
        [SerializeField]
        private string sceneName;

        [SerializeField]
        private LoadSceneMode mode = LoadSceneMode.Additive;

        protected override void OnClick() => SceneManager.LoadSceneAsync(sceneName, mode);
    }
}
