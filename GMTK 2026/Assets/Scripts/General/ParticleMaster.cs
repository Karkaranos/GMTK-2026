using NaughtyAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParticleMaster : MonoBehaviour
{
    [SerializeField, OnValueChanged("OnVC_Library"), Tooltip("CANNOT HAVE REPEAT IDs")]
    private List<Data> _library = new List<Data>();

    [SerializeField, BoxGroup("Simulate")]
    private string _simID;
    [SerializeField, BoxGroup("Simulate")]
    private Transform _simParent;

    public static ParticleMaster INST;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (INST == null)
            INST = this;
        else
            Debug.LogError("Multiple instances of ParticleMaster exist. There can only be one.");
    }

    public void Play(string id, Transform parent)
    {
        int index;
        if (!DoesIDExist(id, out index))
            return;

        _library[index].Play(parent);
    }

    private int Find(string id)
    {
        for (int i = 0; i < _library.Count; i++)
        {
            if (_library[i].Id == id)
                return i;
        }
        return -1;
    }

    private bool DoesIDExist(string id, out int index)
    {
        index = Find(id);

        if (index < 0)
        {
            Debug.LogError("Particle with ID " + id + " does not exist in library.");
            return false;
        }
        return true;
    }

    #region Inspector
    private void OnVC_Library()
    {
        for (int i = 0; i < _library.Count - 1; i++)
        {
            if (_library[_library.Count - 1].Id == _library[i].Id)
            {
                Debug.LogError("Duplicate ID + " + _library[i].Id + " found at indexes " + i + " & " + (_library.Count - 1) + " in particle master library.");
                return;
            }
        }
    }

    [Button]
    private void Simulate()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.Log("Editor application must be playing to simulate.");
            return;
        }

        if (_simParent != null) Play(_simID, _simParent);
        else Play(_simID, transform);
    }
    #endregion

    //=====================================================================================================================
    [System.Serializable]
    public class Data
    {
        [SerializeField, Tooltip("CASE SENSITIVE!")]
        private string _id;

        [SerializeField]
        private GameObject _prefab;

        #region GS
        public string Id { get => _id; set => _id = value; }
        #endregion

        public void Play(Transform parent)
        {
            GameObject obj = Instantiate(_prefab, parent);
        }
    }
}



