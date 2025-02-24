using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// @author Ruben Verheul
/// 02/11/2024
/// This script handles the creation of modular conveyor belts
/// Link to documentation: N.A.
/// </summary>

[ExecuteAlways]
public class ConveyorBeltManager : MonoBehaviour
{
    public GameObject LeftPiece, MiddlePiece, RightPiece, WheelPiece, BottomLeftPiece, BottomMiddlePiece, BottomRightPiece, BottomWheelPiece;
    
    [Header("Conveyor Belt Configurations")]
    [Tooltip("The length of the platform, excluding the edges. This value should not go below 0")]
    [Min(0)]
    public int Length = 3;

    [Tooltip("If set to true, the platform generates a supporting platform on the bottom")]
    public bool Foundation = false;
    
    [Tooltip("The height of the foundation of the platform, excluding the platform itself. This value should not go below 1")]
    [Min(1)]
    public int Height = 1;

    private Transform _middleParent, _bottomParent;
    private float _platformLength = 2.5f;
    private float _offsetLeft = -1.5f, _offsetFoundationY = -2.56f;
    private float _colliderSizeOffset = 0.40f, _colliderOffset = 1.45f;

    public void CreateConveyorBelt()
    {
        //Checking whether all the pieces are being able to be fetched.
        if (LeftPiece == null || MiddlePiece == null || RightPiece == null || WheelPiece == null || BottomLeftPiece == null || BottomMiddlePiece == null || BottomRightPiece == null || BottomWheelPiece == null) 
        {
            Debug.LogError("All piece references must be assigned!");
            return;
        }

        //Clearing leftover conveyorbelt pieces
        ClearConveyorBelt();

        //Creating and positioning parent containers
        _middleParent = new GameObject("MiddlePieces").transform;
        _middleParent.SetParent(transform);
        _middleParent.localPosition = Vector3.zero;

        if (Foundation)
        {
            _bottomParent = new GameObject("BottomPieces").transform;
            _bottomParent.SetParent(transform);
            _bottomParent.localPosition = Vector3.zero;
        }

        //Instantiating the most left pieces of the platform.
        var leftInstance = Instantiate(LeftPiece);
        leftInstance.transform.SetParent(transform, false);
        leftInstance.transform.localPosition = new Vector3(_offsetLeft, 0, 0);

        if (Foundation)
        {
            for (int i = 1; i <= Height; i++)
            {
                var bottomLeftInstance = Instantiate(BottomLeftPiece);
                bottomLeftInstance.transform.SetParent(transform, false);
                bottomLeftInstance.transform.localPosition = new Vector3(_offsetLeft, _offsetFoundationY * i, 0);
            }
        }

        float totalWidth = 0;


        //Instantiating the middle pieces of the platform.
        for (int i = 0; i < Length; i++)
        {
            totalWidth += _platformLength;
            Vector3 middlePosition = new Vector3((i * _platformLength) + MiddlePiece.transform.localScale.x, 0, 0);

            if (ShouldPlaceWheelPiece(i)) //Checking whether a wheel piece needs to be placed.
            {
                //Placing wheel piece and the corresponding bottom piece.
                var wheelInstance = Instantiate(WheelPiece);
                wheelInstance.transform.SetParent(_middleParent, false);
                wheelInstance.transform.localPosition = middlePosition;

                if (Foundation)
                {
                    for (int j = 1; j <= Height; j++)
                    {
                        var bottomInstance = Instantiate(j == 1 ? BottomWheelPiece : BottomMiddlePiece);
                        bottomInstance.transform.SetParent(_bottomParent, false);
                        bottomInstance.transform.localPosition = new Vector3(middlePosition.x, _offsetFoundationY * j, 0);
                    }
                }
            }
            else 
            {
                //Otherwise, placing a normal middle piece and the corresponding bottom piece.
                var middleInstance = Instantiate(MiddlePiece);
                middleInstance.transform.SetParent(_middleParent, false);
                middleInstance.transform.localPosition = middlePosition;

                if (Foundation)
                {
                    for (int j = 1; j <= Height; j++)
                    {
                        var bottomMiddleInstance = Instantiate(BottomMiddlePiece);
                        bottomMiddleInstance.transform.SetParent(_bottomParent, false);
                        bottomMiddleInstance.transform.localPosition = new Vector3(middlePosition.x, _offsetFoundationY * j, 0);
                    }
                }
            }
        }

        totalWidth += _platformLength;

        //Instantiating the most right pieces of the platform.
        var rightInstance = Instantiate(RightPiece);
        rightInstance.transform.SetParent(transform, false);
        rightInstance.transform.localPosition = new Vector3((Length * _platformLength) + MiddlePiece.transform.localScale.x, 0, 0);

        if (Foundation)
        {
            for (int i = 1; i <= Height; i++)
            {
                var bottomRightInstance = Instantiate(BottomRightPiece);
                bottomRightInstance.transform.SetParent(_bottomParent, false);
                bottomRightInstance.transform.localPosition = new Vector3((Length * _platformLength) + BottomMiddlePiece.transform.localScale.x, _offsetFoundationY * i, 0);
            }
        }
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
        collider.size = new Vector2(totalWidth + _platformLength - _colliderSizeOffset - 1f, _platformLength);
        collider.offset = new Vector2((totalWidth / 2f) - _colliderOffset, -0.5f);
    }

    //Logic for choosing wheel piece places
    private bool ShouldPlaceWheelPiece(int index)
    {
        if (index == 0 || index == Length - 1)
        {
            return false;
        }

        if (Length > 4)
        {
            return index % 2 == 0;
        }

        return false;
    }

    //Logic for clearing the conveyorbelt
    public void ClearConveyorBelt()
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

    //Makes sure to update and validate changes when working in the editor.
    void OnValidate()
    {
        if (Length < 0)
        {
            Length = 0;
            Debug.LogWarning($"[{gameObject.name}] Conveyor belt length cannot be less than 0. Resetting to 0...");
        }

        if (Height < 1)
        {
            Height = 1;
            Debug.LogWarning($"[{gameObject.name}] Conveyor belt height cannot be less than 1. Resetting to 1...");
        }

        if (Application.isPlaying) return;
        
        #if UNITY_EDITOR
            EditorApplication.delayCall += () => //Waits until the editor is done compiling everything, so that changes will be applied effectively.
            {
                if (Selection.activeGameObject == this.gameObject)
                {
                    ClearConveyorBelt();
                    CreateConveyorBelt();
                }
            };
        #endif
    }
}
