using System.Collections.Generic;

namespace ThreeJs4Net.Renderers.Shaders
{
    public class Attribute : Dictionary<string, object>
    {
        public Attribute()
        {
        }

        /// <summary>
        /// Lets derived classes that know how many entries they will add (e.g. BufferAttribute) create the
        /// dictionary at its final size instead of growing it.
        /// </summary>
        protected Attribute(int capacity) : base(capacity)
        {
        }
    }
}