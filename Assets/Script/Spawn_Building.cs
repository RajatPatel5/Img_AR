using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Spawn_Building : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager m_ImageManager;

    public ARTrackedImageManager ImageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;

    public XRReferenceImageLibrary ImageLibrary
    {
        get => m_ImageLibrary;
        set => m_ImageLibrary = value;
    }

    [SerializeField]
    [Tooltip("Prefab for tracked 1 image")]
    GameObject m_OnePrefab;

    public GameObject onePrefab
    {
        get => m_OnePrefab;
        set => m_OnePrefab = value;
    }

    GameObject m_SpawnedOnePrefab;

    public GameObject spawnedOnePrefab
    {
        get => m_SpawnedOnePrefab;
        set => m_SpawnedOnePrefab = value;
    }

    [SerializeField]
    [Tooltip("Prefab for tracked 2 image")]
    GameObject m_TwoPrefab;

    public GameObject twoPrefab
    {
        get => m_TwoPrefab;
        set => m_TwoPrefab = value;
    }

    GameObject m_SpawnedTwoPrefab;

    public GameObject spawnedTwoPrefab
    {
        get => m_SpawnedTwoPrefab;
        set => m_SpawnedTwoPrefab = value;
    }

    static Guid s_FirstImageGUID;
    static Guid s_SecondImageGUID;

    void OnEnable()
    {
        s_FirstImageGUID = m_ImageLibrary[0].guid;
        s_SecondImageGUID = m_ImageLibrary[1].guid;

        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // Handle added images
        foreach (ARTrackedImage image in obj.added)
        {
            UpdatePrefabForImage(image); // Use the new method to handle added images
        }

        // Handle updated images
        foreach (ARTrackedImage image in obj.updated)
        {
            UpdatePrefabForImage(image); // Use the new method to handle updated images
        }

        // Handle removed images
        foreach (ARTrackedImage image in obj.removed) // New block for handling removed images
        {
            if (image.referenceImage.guid == s_FirstImageGUID && m_SpawnedOnePrefab != null)
            {
                m_SpawnedOnePrefab.SetActive(false); // Deactivate prefab if the image is removed
            }
            else if (image.referenceImage.guid == s_SecondImageGUID && m_SpawnedTwoPrefab != null)
            {
                m_SpawnedTwoPrefab.SetActive(false); // Deactivate prefab if the image is removed
            }
        }
    }

    void UpdatePrefabForImage(ARTrackedImage image) 
    {
        if (image.trackingState == TrackingState.Tracking)
        {
            if (image.referenceImage.guid == s_FirstImageGUID)
            {
                if (m_SpawnedTwoPrefab != null)
                {
                    m_SpawnedTwoPrefab.SetActive(false);
                }
                if (m_SpawnedOnePrefab == null)
                {
                    m_SpawnedOnePrefab = Instantiate(m_OnePrefab, image.transform.position, image.transform.rotation);
                }
                else
                {
                    m_SpawnedOnePrefab.SetActive(true);
                    m_SpawnedOnePrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                }
            }
            else if (image.referenceImage.guid == s_SecondImageGUID)
            {
                if (m_SpawnedOnePrefab != null)
                {
                    m_SpawnedOnePrefab.SetActive(false);
                }
                if (m_SpawnedTwoPrefab == null)
                {
                    m_SpawnedTwoPrefab = Instantiate(m_TwoPrefab, image.transform.position, image.transform.rotation);
                }
                else
                {
                    m_SpawnedTwoPrefab.SetActive(true);
                    m_SpawnedTwoPrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                }
            }
        }
        else
        {
            if (image.referenceImage.guid == s_FirstImageGUID && m_SpawnedOnePrefab != null)
            {
                m_SpawnedOnePrefab.SetActive(false); // Deactivate prefab if the image is not tracking
            }
            else if (image.referenceImage.guid == s_SecondImageGUID && m_SpawnedTwoPrefab != null)
            {
                m_SpawnedTwoPrefab.SetActive(false); // Deactivate prefab if the image is not tracking
            }
        }
    }

    public int NumberOfTrackedImages()
    {
        m_NumberOfTrackedImages = 0;
        foreach (ARTrackedImage image in m_ImageManager.trackables)
        {
            if (image.trackingState == TrackingState.Tracking)
            {
                m_NumberOfTrackedImages++;
            }
        }
        return m_NumberOfTrackedImages;
    }

    int m_NumberOfTrackedImages;
}
