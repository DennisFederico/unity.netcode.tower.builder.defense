namespace managers {
    public static class GameSceneManager {
        private enum Scene {
            MainMenuScene,
            GameScene,
        }
        
        public static void LoadMainMenuScene() {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scene.MainMenuScene.ToString());
        }
        
        public static void LoadGameScene() {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Scene.GameScene.ToString());
        }
    }
}