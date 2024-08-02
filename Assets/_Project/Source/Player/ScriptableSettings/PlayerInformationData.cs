using UnityEngine;

namespace Tatsu.Core
{
    [CreateAssetMenu(fileName = "PlayerInfoData", menuName = "Player/Player Data")]
    public class PlayerInformationData : ScriptableObject
    {
        [SerializeField] private string _playerName;
        [SerializeField] private Sprite _playerAvatarSprite;
        
        public string PlayerName => _playerName;
        public Sprite PlayerAvatarSprite => _playerAvatarSprite;
    }
}
