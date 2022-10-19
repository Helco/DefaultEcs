using System;
using System.Runtime.CompilerServices;

namespace DefaultEcs
{
    /// <summary>
    /// Provides a fast access to the components of type <typeparamref name="T"/>.
    /// Note that all entity modification operations are not safe (anything different than a simple <see cref="Entity.Get{T}"/>) and may invalidate the <see cref="Components{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    public readonly ref struct Components<T>
    {
        private readonly int[] _mapping;
        private readonly T[] _components;
#if DEFAULTECS_SAFE
        /// <summary>
        /// During iteration, the arrays in ComponentPool might be reallocated, then the references here
        /// are stale and do not propagate changes and will throw exception if used with a high entity id
        /// </summary>
        private readonly Func<(int[] mapping, T[] components)> _getCurrentReferences;

        internal Components(int[] mapping, T[] components, Func<(int[] mapping, T[] components)> getCurrentReferences)
        {
            _getCurrentReferences = getCurrentReferences;
#else
        internal Components(int[] mapping, T[] components)
        {
#endif
            _mapping = mapping;
            _components = components;
        }

        /// <summary>
        /// Gets the component of type <typeparamref name="T"/> on the provided <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> for which to get the component of type <typeparamref name="T"/>.</param>
        /// <returns>A reference to the component of type <typeparamref name="T"/>.</returns>
        /// <exception cref="DefaultEcsException">dfgdfg</exception>
        public ref T this[Entity entity]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if DEFAULTECS_SAFE
                ThrowIfReallocated();
#endif
                return ref _components[_mapping[entity.EntityId]];
            }
        }

#if DEFAULTECS_SAFE
        internal void ThrowIfReallocated()
        {
            var (newMapping, newComponents) = _getCurrentReferences();
            if (!ReferenceEquals(newMapping, _mapping) || !ReferenceEquals(newComponents, _components))
            {
                throw new DefaultEcsException($"Underlying pool of Components<{typeof(T).Name}> was reallocated, this instance cannot be used anymore");
            }
        }
#endif
    }
}
