using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroid.Generator
{
    /// <summary>
    /// Astreoid Generator for the environment aspects. This script will run at the start of the game to form a map.
    /// This will create a feeling of movement within space. We can compare speed of our vehicle relative to the static environment.
    /// </summary>
    public class AsteroidGenerator : MonoBehaviour
    {

        [Header("Asteroid Generation Settings")]
        [Header("Asteroid Visuals")]
        [SerializeField] private int _asteroidCount = 100;
        [SerializeField] private GameObject[] _asteroidPrefabs;
        [Header("Asteroid Transform")]
        [SerializeField] private Vector3 _asteroidRandomPositionRange = new Vector3(2000, 2000, 2000);
        [SerializeField] private Vector2 _asteroidScaleRange = new Vector2(100, 500);

        private void Start() => GenerateAsteroids();

        private void GenerateAsteroids()
        {
            float defaultScale = _asteroidPrefabs[0].transform.localScale.x;

            for (int i = 0; i < _asteroidCount; i++)
            {
                CreateRandomAsteroids()
                    .SetRandomPosition(_asteroidRandomPositionRange)
                    .SetRandomRotation()
                    .SetRandomScale(_asteroidScaleRange.x, _asteroidScaleRange.y);
            }
        }

        private Asteroid CreateRandomAsteroids()
        {
            int r = Random.Range(0, _asteroidPrefabs.Length - 1);
            GameObject obj = Instantiate(_asteroidPrefabs[r]);

            return new Asteroid(obj);
        }
    }

}

namespace Asteroid
{
    public class Asteroid
    {
        private GameObject _gameObject;

        public Asteroid(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public Asteroid SetRandomRotation()
        {
            _gameObject.transform.rotation = Random.rotation;
            return this;
        }

        public Asteroid SetRandomPosition(Vector3 range)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-range.x, range.x),
                Random.Range(-range.y, range.y),
                Random.Range(-range.z, range.z)
                );
            _gameObject.transform.position = randomPosition;
            return this;
        }
        public Asteroid SetRandomScale(float minScale, float maxScale)
        {
            float scale = Random.Range(minScale, maxScale);
            _gameObject.transform.localScale = new Vector3(scale, scale, scale);
            return this;
        }
    }
}
