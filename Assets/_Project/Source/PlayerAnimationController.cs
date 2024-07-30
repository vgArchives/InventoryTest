using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tatsu.Core
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;

        protected void Awake()
        {
            _playerAnimator = GetComponent<Animator>();
        }
    }
}
