using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.Input;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    public class HeroMover : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _speed;
        private Vector3 _direction;
        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _direction = Vector3.zero;
            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                _direction = _camera.transform.TransformDirection(_inputService.Axis);
                _direction.y = 0;
                _direction.Normalize();

                transform.forward = _direction;
            }

            _direction += Physics.gravity;

            _characterController.Move(_direction * _speed * Time.deltaTime);
        } 

        public void UpdateProgress(PlayerProgress progress) => 
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null) 
                    Warp(to: savedPosition);
            }
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;
    }
}