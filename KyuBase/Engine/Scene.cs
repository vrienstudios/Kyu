using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace KyuBase.Engine
{
    public interface Scene : IDisposable
    {
        string sceneName(string e);
        string sceneID(string e);

        /// <summary>
        /// Return your scene as an FObject here.
        /// </summary>
        /// <returns></returns>
        FObject[] LoadScene(screenClass screen = null);

        /// <summary>
        /// Return which scene to load next.
        /// </summary>
        Scene NextScene();
    }
}
