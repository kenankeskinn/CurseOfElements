using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerManager;
using CompanionManager;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        static int currentLevel = 0; // The levels are designated as 0, 1, 2, 3, and 4. Level 0 is where the story begins and involves no combat.
        static int currentCheckpointOnCurrentLevel = 1;
        static CheckpointController[] currentCheckpoints;
        static GameObject[] currentStateScreens;

        [Header("Level-Based Operations")]
        [SerializeField] Cinemachine.CinemachineVirtualCamera playerCamera;
        [SerializeField] GameObject[] stateScreens;
        [SerializeField] CheckpointController[] checkpoints;

        #endregion

        #region Functions
        #region Game Load
        public static void StartNewGame() // Starts a new game at Level 0 and clear player saves.
        {

        }

        public static void LoadGame() // Starts the game from the last level and checkpoint where the player left off.
        {
            // Load to last scene where the player left.
            SceneManager.LoadScene(currentLevel);

            // Start player system
            PlayerContext.Instance.StartPlayerSystem();

            // Start companion system
            CompanionContext.Instance.StartCompanionSystem();

            // Set new last positions to player and companion
            if (currentCheckpointOnCurrentLevel == 0) // Teleport player and companion to start of the game.
            {
                PlayerContext.Instance.gameObject.transform.position = Vector2.zero;
                CompanionContext.Instance.gameObject.transform.position = Vector2.zero;
            }
            else // Teleport player and companion to last checkpoint where the player left.
            {
                Vector2 currentCheckpointPosition = currentCheckpoints[currentCheckpointOnCurrentLevel - 1].CheckpointPosition;
                PlayerContext.Instance.gameObject.transform.position = currentCheckpointPosition;
                CompanionContext.Instance.gameObject.transform.position = new Vector2(currentCheckpointPosition.x - 2, currentCheckpointPosition.y);
            }

            // Start Time
            Time.timeScale = 1;
        }
        #endregion

        #region Game Management
        public static void SaveGame()
        {

        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
        }

        public static void ResumeGame()
        {
            Time.timeScale = 1;
        }

        public static void QuitGame()
        {
            SaveGame();
        }
        #endregion

        #region Level Management
        public static void LevelStarted()
        {

        }

        public static void LevelCompleted()
        {

        }

        public static void LevelFailed()
        {
            // Stop Player System
            PlayerContext.Instance.CanWalk = false;
            PlayerContext.Instance.CanJump = false;
            PlayerContext.Instance.CanAttack = false;
            PlayerContext.Instance.CanTakeDamage = false;
            PlayerContext.Instance.Rigidbody.bodyType = RigidbodyType2D.Static;
            PlayerContext.Instance.gameObject.GetComponent<Collider2D>().enabled = false;
            PlayerContext.Instance.IsDead = true;

            // Stop Companion System
            CompanionContext.Instance.CanWalk = false;
            CompanionContext.Instance.CanJump = false;
            CompanionContext.Instance.CanAttack = false;
            CompanionContext.Instance.CanTakeDamage = false;
            CompanionContext.Instance.Rigidbody.bodyType = RigidbodyType2D.Static;
            CompanionContext.Instance.gameObject.GetComponent<Collider2D>().enabled = false;
            CompanionContext.Instance.IsDead = true;

            // Stop Time
            Time.timeScale = 0;

            // Open Level Failed Screen
            currentStateScreens[0].SetActive(true);
        }

        public static void NextLevel()
        {

        }

        public static void RestartLevel()
        {
            LoadGame();
        }

        public static void UpdateCurrentCheckpoint(int checkpointID)
        {
            currentCheckpointOnCurrentLevel = checkpointID;
        }
        #endregion
        #endregion

        #region Unity Functions
        private void Awake()
        {
            playerCamera.Follow = PlayerContext.Instance.gameObject.transform;

            currentStateScreens = stateScreens;
            currentCheckpoints = checkpoints;
            // currentLevel = sceneLevel
        }
        #endregion
    }
}
