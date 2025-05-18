using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using static UnityEditor.PlayerSettings;

public class Reposition : MonoBehaviour

{
    private void OnTriggerExit2D(Collider2D collision)

    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        Vector3 playerposition = GameManager.Instance.player.transform.position;

        Vector3 myposition = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerposition.x - myposition.x;
                float diffY = playerposition.y - myposition.y;

                float dirX = playerposition.x - myposition.x;

                float dirY = playerposition.y - myposition.y;
                dirX = dirX < 0 ? -1 : 1;
                dirY = dirY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);

                diffY = Mathf.Abs(diffY);

                if (Mathf.Abs(diffX - diffY) <= 0.1f)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                    transform.Translate(Vector3.up * dirY * 40);
                }
                else if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":

                break;
        }
    }
}