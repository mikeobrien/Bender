using System.Collections.Generic;
using System.Linq;

namespace Bender.Nodes
{
    public class Metadata
    {
        public readonly static Metadata Empty = new Metadata();

        protected List<object> _metadata;

        public Metadata(params object[] metadata)
        {
            if (metadata.Any()) Add(metadata);
        }

        public Metadata Add(Metadata metadata)
        {
            return Add(metadata._metadata);
        }

        public Metadata Add(params object[] metadata)
        {
            return Add(metadata.AsEnumerable());
        }

        public Metadata Add(IEnumerable<object> metadata)
        {
            if (metadata == null) return this;
            if (_metadata == null) _metadata = new List<object>();
            _metadata.AddRange(metadata);
            return this;
        }

        public bool Contains<T>()
        {
            return _metadata != null && _metadata.Any(x => x.GetType() == typeof(T));
        }

        public T Get<T>()
        {
            if (_metadata == null) return default(T);
            return (T)_metadata.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public IEnumerable<T> GetAll<T>()
        {
            if (_metadata == null) return Enumerable.Empty<T>();
            return _metadata.Where(x => x.GetType() == typeof(T)).Cast<T>().ToList();
        }
    }
}
