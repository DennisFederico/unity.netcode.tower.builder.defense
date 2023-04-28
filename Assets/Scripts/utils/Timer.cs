namespace utils {
    public class Timer {
        private float _duration;

        public Timer(float duration) {
            _duration = duration;
        }

        public void Update(float deltaTime) {
            _duration -= deltaTime;
        }

        public bool IsFinished() {
            return _duration < 0;
        }
    }
}