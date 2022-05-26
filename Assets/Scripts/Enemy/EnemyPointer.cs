using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EnemyPointer : MonoBehaviour
{
    GameManager gameManager;
    SpawnManager spawnManager;
    Camera _camera;
    private GameObject _pointerIcon;

     
    private void Start()
    {
        gameManager = GameManager.instance;
        spawnManager = SpawnManager.instance;
        _camera = Camera.main;

        // spawn pointer prefab and make a parent canvas
        _pointerIcon = Instantiate(spawnManager.pointerIconPrefab, spawnManager.pointerIconPrefab.transform.position, spawnManager.pointerIconPrefab.transform.rotation);
        _pointerIcon.transform.SetParent(gameManager._canvas.transform);
    }

    void Update()
    {
        Transform playerTransform = gameManager.player.GetComponentInChildren<BulletCreator>().gunBlockRb.gameObject.transform;
        Vector3 fromPlayerToEnemy = transform.position - playerTransform.position;
        Ray ray = new Ray(playerTransform.position, fromPlayerToEnemy);
        //Debug.DrawRay(playerTransform.position, fromPlayerToEnemy, Color.cyan);

        // [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera); 

        float minDistance = Mathf.Infinity;
        int planeIndex = 0;

        for(int i = 0; i < 4; i++)
        {
            if(planes[i].Raycast(ray, out float distance))
            {       
                if(distance < minDistance)
                {
                    minDistance = distance;
                    planeIndex = i;
                }
            }
        }

        //minDistance = Mathf.Clamp(minDistance, 0, fromPlayerToEnemy.magnitude); 

        if (fromPlayerToEnemy.magnitude < minDistance)
        {
            _pointerIcon.SetActive(false);
        }
        else
        {
            _pointerIcon.SetActive(true);

            Vector3 worldPosition = ray.GetPoint(minDistance);

            _pointerIcon.transform.position = _camera.WorldToScreenPoint(worldPosition);
            _pointerIcon.transform.rotation = GeticonRotation(planeIndex);
        }

    }

    Quaternion GeticonRotation(int planeIndex)
    {
        if (planeIndex == 0)
        {
            return Quaternion.Euler(0f, 0f, 90f);
        } else if (planeIndex == 1)
        {
            return Quaternion.Euler(0f, 0f, -90f);
        } else if (planeIndex == 2)
        {
            return Quaternion.Euler(0f, 0f, 180f);
        } else if (planeIndex == 3)
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }
        return Quaternion.identity;
    }

    private void OnDestroy()
    {
        Destroy(_pointerIcon);
    }


}
