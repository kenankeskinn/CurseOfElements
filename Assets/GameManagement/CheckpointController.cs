using UnityEngine;

namespace GameManagement
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField][Range(1, 15)] int checkpointID = 1;
        [SerializeField] bool canUseCheckpoint = false;
        [SerializeField] bool isCheckpointUsed = false; // Just for safety can remove after see no problem

        public int CheckpointID { get { return checkpointID; } }
        public Vector2 CheckpointPosition { get { return new Vector2(transform.position.x - 1.5f, transform.position.y); } }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && canUseCheckpoint && !isCheckpointUsed)
            {
                GameManager.UpdateCurrentCheckpoint(checkpointID);

                // Disable Checkpoint
                canUseCheckpoint = false;
                isCheckpointUsed = true;
            }
        }

        public void EnableCheckpoint()
        {
            canUseCheckpoint = true;
        }
    }
}
