using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gridSize = 1.0f; // 每次移动的网格大小
    private Vector2 targetPosition;

    private void Start()
    {
        // 初始化玩家位置
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        // 如果玩家当前正在移动，不接受新输入
        if (transform.position.x != targetPosition.x || transform.position.y != targetPosition.y)
        {
            return;
        }

        Vector2 inputDirection = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            inputDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            inputDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            inputDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            inputDirection = Vector2.right;
        }

        if (inputDirection != Vector2.zero)
        {
            Vector2 potentialPosition = targetPosition + inputDirection * gridSize;

            // 检查目标位置是否有门
            Door door = CheckForDoor(potentialPosition);
            if (door != null)
            {
                if (door.type == DoorType.Open)
                {
                    Debug.Log("Door is open, moving to the next position.");
                    targetPosition = potentialPosition;
                }
                else
                {
                    Debug.Log("Need key to open this door!");
                }
            }
            else
            {
                // 如果没有门，直接移动
                targetPosition = potentialPosition;
            }
        }
    }

    private void MovePlayer()
    {
        // 平滑移动玩家到目标位置
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
    }

    private Door CheckForDoor(Vector2 position)
    {
        // 检查目标位置是否有门（假设门有一个Collider2D组件）
        Collider2D hit = Physics2D.OverlapPoint(position);
        if (hit != null)
        {
            Door door = hit.GetComponent<Door>();
            return door;
        }
        return null;
    }
}


