using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// @author Ruben Verheul
/// 07/01/2024
/// This script handles the creation of modular lamp platforms
/// Link to documentation: N.A.
/// </summary>

[ExecuteAlways]
public class LampManager : MonoBehaviour
{
    public GameObject LeftPiece, MiddlePiece, RightPiece, GrapplingPiece, SolutionPiece, CannonPiece;
    [Header("Platform Configurations")]
    [Tooltip("The length of the platform, excluding the edges. This value should not go below 0")]
    [Min(0)]
    public int Length = 3;
    public bool HasGrapplePoint = false, HasCannon = false, FlipHorizontally = false;

    private Transform _middleParent;
    private float _platformLength = 2.56f;
    private float _offsetLeft = -1.5f;
      private float _colliderSizeOffset = 0.40f, _colliderOffset = 1.45f;

    public void CreatePlatform()
    {
        //Checking whether all the pieces are being able to be fetched.
        if (LeftPiece == null || MiddlePiece == null || RightPiece == null) 
        {
            Debug.LogError("All piece references must be assigned!");
            return;
        }

        //Clearing leftover pieces
        ClearPieces();

        //Creating and positioning parent containers
        _middleParent = new GameObject("MiddlePieces").transform;
        _middleParent.SetParent(transform);
        _middleParent.localPosition = Vector3.zero;
        
        //Instantiating the most left pieces of the platform.
        var leftInstance = FlipHorizontally
        ? (HasCannon ? Instantiate(CannonPiece) : Instantiate(RightPiece))
        : (HasGrapplePoint ? Instantiate(GrapplingPiece) : Instantiate(LeftPiece));


        leftInstance.transform.SetParent(transform, false);
        leftInstance.transform.localPosition = new Vector3(_offsetLeft, 0, 0);
        FlipSprite(leftInstance);

        float totalWidth = 0;

        for (int i = 0; i < Length; i++)
        {
            totalWidth += _platformLength;
            Vector3 middlePosition = new Vector3((i * _platformLength) + MiddlePiece.transform.localScale.x, 0, 0);


            bool placeSolutionPiece = FlipHorizontally ? (HasCannon && i == 0) : (HasCannon && i == Length - 1);
            var middleInstance = placeSolutionPiece ? Instantiate(SolutionPiece) : Instantiate(MiddlePiece);

            middleInstance.transform.SetParent(_middleParent, false);
            middleInstance.transform.localPosition = middlePosition;
            FlipSprite(middleInstance);
        }

        totalWidth += _platformLength;

        //Instantiating the most right pieces of the platform.
        var rightInstance = FlipHorizontally
        ? (HasGrapplePoint ? Instantiate(GrapplingPiece) : Instantiate(LeftPiece))
        : (HasCannon ? Instantiate(CannonPiece) : Instantiate(RightPiece));

        rightInstance.transform.SetParent(transform, false);
        rightInstance.transform.localPosition = new Vector3((Length * _platformLength) + MiddlePiece.transform.localScale.x, 0, 0);
        FlipSprite(rightInstance);

        AddPlatformCollider(totalWidth);
    }

    //Function for adding a custom platform collider2D
    private void AddPlatformCollider(float totalWidth)
    {
        var existingCollider = GetComponent<BoxCollider2D>();
        if (existingCollider != null)
        {
            DestroyImmediate(existingCollider);
        }

        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(totalWidth + _platformLength - _colliderSizeOffset, _platformLength / 3f);
        collider.offset = new Vector2((totalWidth / 2f) - _colliderOffset - 0.1f, -0.9f);
    }

    //Logic for clearing pieces
    private void ClearPieces()
    {
        if (_middleParent != null)
        {
            #if UNITY_EDITOR
            DestroyImmediate(_middleParent.gameObject, true); //DestroyImmediate will be used so changes will be updated in real-time when working in the editor.
            #else
            Destroy(_middleParent.gameObject);
            #endif
            _middleParent = null;
        }

        //Acquiring all the pieces in order to delete later on, but outside of the existing list, to combat errors.
        var children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            #if UNITY_EDITOR
            DestroyImmediate(child.gameObject, true);
            #else
            Destroy(child.gameObject);
            #endif
        }
    }

    private void FlipSprite(GameObject piece)
    {
        if (FlipHorizontally)
        {
            var spriteRenderer = piece.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                piece.transform.localScale = new Vector3(-piece.transform.localScale.x, piece.transform.localScale.y, piece.transform.localScale.z);
            }
        }
    }

    //Makes sure to update and validate changes when working in the editor.
    void OnValidate()
    {
        if (Length < 0)
        {
            Length = 0;
            Debug.LogWarning($"[{gameObject.name}] Platform length cannot be less than 0. Resetting to 0...");
        }

        if (Application.isPlaying) return;
        
        #if UNITY_EDITOR
            EditorApplication.delayCall += () => //Waits until the editor is done compiling everything, so that changes will be applied effectively.
            {
                if (Selection.activeGameObject == this.gameObject)
                {
                    ClearPieces();
                    CreatePlatform();
                }
            };
        #endif
    }
}
