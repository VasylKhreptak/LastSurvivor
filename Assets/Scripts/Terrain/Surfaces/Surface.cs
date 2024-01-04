using Terrain.Surfaces.Core;
using UnityEngine;

namespace Terrain.Surfaces
{
    public class Surface : MonoBehaviour, ISurface
    {
        [Header("Preferences")]
        [SerializeField] private SurfaceType _type;

        public SurfaceType Type => _type;
    }
}