using KyuBase.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace KyuBase.Engine
{
    public abstract class SceneBase : Scene
    {
        public void Dispose()
        {
            GC.Collect();
        }

        private string _sceneID;
        private string _sceneName;

        public abstract FObject[] LoadScene(screenClass screen = null);

        public abstract Scene NextScene();

        /// <summary>
        /// You should not override this.
        /// </summary>
        /// <param name="id">set scene id</param>
        /// <returns></returns>
        public string sceneID(string id = null)
        {
            if (id != null)
                _sceneID = id;
            return _sceneID;
        }

        /// <summary>
        /// You should not override this.
        /// </summary>
        /// <param name="name">set scene name</param>
        /// <returns></returns>
        public string sceneName(string name = null)
        {
            if (name != null)
                _sceneName = name;
            return _sceneName;
        }

        ~SceneBase()
        {
            Dispose();
        }
    }
}
