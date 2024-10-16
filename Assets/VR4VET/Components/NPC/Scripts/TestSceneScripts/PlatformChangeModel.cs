using UnityEngine;

public class PlatformChangeModel : MonoBehaviour
{
    [SerializeField] private GameObject[] _modelPrefabs;

    [SerializeField] private Avatar[] _modelAvatar;

    [SerializeField] private RuntimeAnimatorController runtimeAnimatorController;

    [SerializeField] private int[] _WitVoiceIds;
    [HideInInspector] private int _currentModelNr;

    [SerializeField] private GameObject _npc;
    [HideInInspector] private SetCharacterModel _setCharacterModel;

    void Awake() {
        _currentModelNr = 0;
        _setCharacterModel = _npc.GetComponent<SetCharacterModel>();
    }
    private void ChangeModel() {
        _setCharacterModel.ChangeCharacter(_modelPrefabs[_currentModelNr], _modelAvatar[_currentModelNr], runtimeAnimatorController, _WitVoiceIds[_currentModelNr]);
        _currentModelNr++;
        if (_currentModelNr >= _modelPrefabs.Length || _currentModelNr >= _modelAvatar.Length || _currentModelNr >= _WitVoiceIds.Length)
        {
            _currentModelNr = 0;
        }
    }
    void OnTriggerEnter() {
        ChangeModel();
    }

}
